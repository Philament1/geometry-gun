using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShapeSelectLocal : ShapeSelect
{
    SpriteRenderer p1Preview, p2Preview;
    Text p1ShapeName, p2ShapeName, p1ShapeDesc, p2ShapeDesc;
    GameObject p1DescDisplay, p2DescDisplay;
    bool p1Ready, p2Ready;
    int p1ShapeIndex = 0;
    int p2ShapeIndex = 0;
    float p1LastInput = 0;
    float p2LastInput = 0;

    // Start is called before the first frame update
    void Start()
    {
        _Start();

        GameInfo.isOnline = false;
        p1Preview = GameObject.Find("Player 1 Preview").GetComponent<SpriteRenderer>();
        p2Preview = GameObject.Find("Player 2 Preview").GetComponent<SpriteRenderer>();
        p1ShapeName = GameObject.Find("Player 1 Shape Name").GetComponent<Text>();
        p2ShapeName = GameObject.Find("Player 2 Shape Name").GetComponent<Text>();
        p1ShapeDesc = GameObject.Find("Player 1 Shape Description").GetComponent<Text>();
        p2ShapeDesc = GameObject.Find("Player 2 Shape Description").GetComponent<Text>();
        p1DescDisplay = GameObject.Find("Player 1 Description Display");
        p2DescDisplay = GameObject.Find("Player 2 Description Display");
        for (int i = 0; i < Shapes.shapes.Length; i++)
        {
            AddShapeSpriteInfo(new ShapeSpriteInfo(Shapes.shapes[i].name));
        }
        ShapesToArray();
        p1Preview.sprite = shapes[p1ShapeIndex].sprite;
        p1ShapeName.text = shapes[p1ShapeIndex].name;
        p1ShapeDesc.text = Shapes.shapes[p1ShapeIndex].description;
        p2Preview.sprite = shapes[p2ShapeIndex].sprite;
        p2ShapeName.text = shapes[p2ShapeIndex].name;
        p2ShapeDesc.text = Shapes.shapes[p2ShapeIndex].description;

        p1DescDisplay.SetActive(false);
        p2DescDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float p1HorizontalInput = Input.GetAxisRaw("P1 Horizontal");
        float p2HorizontalInput = Input.GetAxisRaw("P2 Horizontal");

        if (p1HorizontalInput != 0 && p1LastInput == 0 && !p1Ready)
        {
            p1ShapeIndex = (p1ShapeIndex + Mathf.RoundToInt(p1HorizontalInput)) % shapes.Length;
            if (p1ShapeIndex < 0)
            {
                p1ShapeIndex = shapes.Length - 1;
            }
            p1Preview.sprite = shapes[p1ShapeIndex].sprite;
            p1ShapeName.text = shapes[p1ShapeIndex].name;
            p1ShapeDesc.text = Shapes.shapes[p1ShapeIndex].description;
        }
        if (p2HorizontalInput != 0 && p2LastInput == 0 && !p2Ready)
        {
            p2ShapeIndex = (p2ShapeIndex + Mathf.RoundToInt(p2HorizontalInput)) % shapes.Length;
            if (p2ShapeIndex < 0)
            {
                p2ShapeIndex = shapes.Length - 1;
            }
            p2Preview.sprite = shapes[p2ShapeIndex].sprite;
            p2ShapeName.text = shapes[p2ShapeIndex].name;
            p2ShapeDesc.text = Shapes.shapes[p2ShapeIndex].description;
        }

        p1LastInput = p1HorizontalInput;
        p2LastInput = p2HorizontalInput;

        if (Input.GetAxisRaw("P1 Vertical") > 0)
        {
            p1DescDisplay.SetActive(true);
        }
        else
        {
            p1DescDisplay.SetActive(false);
        }

        if (Input.GetAxisRaw("P2 Vertical") > 0)
        {
            p2DescDisplay.SetActive(true);
        }
        else
        {
            p2DescDisplay.SetActive(false);
        }

        if (Time.time - startTime >= 1f)
        {
            if (Input.GetButtonDown("P1 Fire"))
            {
                if (tick2.activeSelf)
                {
                    StartGame();
                }
                else
                {
                    p1Ready = true;
                    tick1.SetActive(true);
                }
            }
            if (Input.GetButtonDown("P2 Fire"))
            {
                if (tick1.activeSelf)
                {
                    StartGame();
                }
                else
                {
                    p2Ready = true;
                    tick2.SetActive(true);
                }
            }
        }
    }

    public void MainMenu()
    {
        GameInfo.p1Score = 0;
        GameInfo.p2Score = 0;
        SceneManager.LoadScene("MainMenu");
    }

    void StartGame()
    {
        GameInfo.p1Shape = shapes[p1ShapeIndex].name;
        GameInfo.p2Shape = shapes[p2ShapeIndex].name;
        MyGameManager.RunGame();
        SceneManager.LoadScene("Game");
    }
}
