using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    static Text statusText;
    static GameObject roomMenu;

    Text roomNameText, playerNameText;

    string AntiNullInput(Text text, string defaultInput)
    {
        if (text.text == "")
        {
            return defaultInput;
        }
        else
        {
            return text.text;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        statusText = GameObject.Find("Status").GetComponent<Text>();
        roomNameText = GameObject.Find("Room Name Input").transform.GetChild(1).GetComponent<Text>();
        playerNameText = GameObject.Find("Player Name Input").transform.GetChild(1).GetComponent<Text>();
        roomMenu = GameObject.Find("Room Menu");
        roomMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenu()
    {
        Destroy(GameObject.Find("Network Controller"));
        SceneManager.LoadScene("MainMenu");
    }

    public static void SetStatus(string message)
    {
        statusText.text += $"\n{message}";
        if (message == "Joined lobby")
        {
            roomMenu.SetActive(true);
        }
        else if (message.StartsWith("Joined "))
        {
            roomMenu.SetActive(false);
        }
    }

    public void CreateRoom()
    {
        NetworkController.controller.CreateRoom(AntiNullInput(roomNameText, "PATEL825"), AntiNullInput(playerNameText, "Player"));
    }

    public void JoinRoom()
    {
        NetworkController.controller.JoinRoom(AntiNullInput(roomNameText, "PATEL825"), AntiNullInput(playerNameText, "Player"));
    }
}
