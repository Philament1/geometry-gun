using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeSelectOnline : ShapeSelect
{
    static ShapeSelectOnline thisScript;
    static bool isReady = false;
    static GameObject localTick, otherTick;

    SpriteRenderer preview;
    GameObject descDisplay;
    Text shapeName, shapeDesc;
    int shapeIndex = 0;
    float lastInput = 0;
    string horizontalAxis, verticalAxis, fireKey;

    // Start is called before the first frame update
    void Start()
    {
        _Start();

        thisScript = this;
        GameInfo.isOnline = true;

        string localPlayerName, otherPlayerName, playerNum;
        
        if (NetworkController.IsP1)
        {
            localPlayerName = "Player 1";
            otherPlayerName = "Player 2";
            playerNum = "P1";
            localTick = tick1;
            otherTick = tick2;
        }
        else
        {
            localPlayerName = "Player 2";
            otherPlayerName = "Player 1";
            playerNum = "P2";
            localTick = tick2;
            otherTick = tick1;
        }

        GameObject.Find($"{localPlayerName} Name").GetComponent<Text>().text = NetworkController.LocalPlayerName;
        GameObject.Find($"{otherPlayerName} Name").GetComponent<Text>().text = NetworkController.OtherPlayerName;
        preview = GameObject.Find($"{localPlayerName} Preview").GetComponent<SpriteRenderer>();
        GameObject.Find($"{otherPlayerName} Preview").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Question Mark");
        shapeName = GameObject.Find($"{localPlayerName} Shape Name").GetComponent<Text>();
        shapeDesc = GameObject.Find($"{localPlayerName} Shape Description").GetComponent<Text>();
        descDisplay = GameObject.Find($"{localPlayerName} Description Display");
        descDisplay.SetActive(false);
        GameObject.Find($"{otherPlayerName} Select").SetActive(false);
        horizontalAxis = playerNum + " Horizontal";
        verticalAxis = playerNum + " Vertical";
        fireKey = playerNum + " Fire";

        for (int i = 0; i < Shapes.shapes.Length; i++)
        {
            Shapes.ShapeInfo shape = Shapes.shapes[i];
            if (shape.isOnline)
            {
                AddShapeSpriteInfo(new ShapeSpriteInfo(shape.name));
            }
        }
        ShapesToArray();

        UpdateShape();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw(horizontalAxis);

        if (horizontalInput != 0 && lastInput == 0 && !isReady)
        {
            shapeIndex = (shapeIndex + Mathf.RoundToInt(horizontalInput)) % shapes.Length;
            if (shapeIndex < 0)
            {
                shapeIndex = shapes.Length - 1;
            }
            UpdateShape();
        }

        lastInput = horizontalInput;

        if (Input.GetAxisRaw(verticalAxis) > 0)
        {
            descDisplay.SetActive(true);
        }
        else
        {
            descDisplay.SetActive(false);
        }

        if (Input.GetButtonDown(fireKey) && Time.time - startTime >= 1f)
        {
            if (NetworkController.IsP1)
            {
                GameInfo.p1Shape = shapes[shapeIndex].name;
            }
            else
            {
                GameInfo.p2Shape = shapes[shapeIndex].name;
            }

            if (otherTick.activeSelf)
            {
                if (NetworkController.LoadScene("Game"))
                {
                    return;
                }
            }
            else
            {
                isReady = true;
                localTick.SetActive(true);
            }
            NetworkController.SendCallMethodRPC("ShapeSelectOnline", "OtherPlayerReady", new object[] { shapes[shapeIndex].name });
        }
    }

    //RPC
    public static void OtherPlayerReady(int timestamp, string shape)
    {
        if (NetworkController.IsP1)
        {
            GameInfo.p2Shape = shape;
        }
        else
        {
            GameInfo.p1Shape = shape;
        }

        if (isReady)
        {
            MyGameManager.RunGame();
            NetworkController.LoadScene("Game");
        }
        else
        {
            otherTick.SetActive(true);
        }
    }

    void UpdateShape()
    {
        preview.sprite = shapes[shapeIndex].sprite;
        shapeName.text = shapes[shapeIndex].name;
        shapeDesc.text = Shapes.shapes[shapeIndex].description;
    }
}
