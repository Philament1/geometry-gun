using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks
{
    public static NetworkController controller;

    static Text pingText;

    public static int Timestamp { get => PhotonNetwork.ServerTimestamp; }

    public static bool IsP1 { get => PhotonNetwork.IsMasterClient; }

    public static string LocalPlayerName { get => PhotonNetwork.LocalPlayer.NickName; }

    public static string OtherPlayerName { get => PhotonNetwork.PlayerListOthers[0].NickName; }

    public static GameObject InstantiateGO(string prefabName, Vector3 pos, bool isP1)
    {
        GameObject newObj = PhotonNetwork.Instantiate(prefabName, pos, Quaternion.identity);
        if (!isP1)
        {
            newObj.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.PlayerListOthers[0]);
        }
        return newObj;
    }

    public static bool LoadScene(string sceneName)
    {
        bool isSuccesful = false;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(sceneName);
            isSuccesful = true;
        }

        return isSuccesful;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        controller = this;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
        SceneManager.activeSceneChanged += NewScene;
    }

    // Update is called once per frame
    void Update()
    {
        if (pingText != null)
        {
            pingText.text = $"Ping: {PhotonNetwork.GetPing()}ms";
        }
    }

    public static void SendCallMethodRPC(string className, string methodName, object[] parameters)
    {
        List<object> timestampParams = new List<object>();
        timestampParams.Add(Timestamp);
        for (int i = 1; i <= parameters.Length; i++)
        {
            timestampParams.Add(parameters[i - 1]);
        }
        object[] timestampParamsArray = timestampParams.ToArray();
        controller.photonView.RPC("CallMethodRPC", RpcTarget.Others, className, methodName, timestampParamsArray);
    }

    [PunRPC]
    void CallMethodRPC(string className, string methodName, object[] parameters)
    {
        Type.GetType(className).GetMethod(methodName).Invoke(null, parameters);
    }


    public override void OnConnectedToMaster()
    {
        Lobby.SetStatus($"Connected to {PhotonNetwork.CloudRegion}\nConnecting to lobby...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Lobby.SetStatus("Joined lobby");
    }

    public void CreateRoom(string roomName, string playerName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        PhotonNetwork.NickName = playerName;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Lobby.SetStatus(message);
    }

    public void JoinRoom(string roomName, string playerName)
    {
        PhotonNetwork.JoinRoom(roomName);
        PhotonNetwork.NickName = playerName;
    }

    public override void OnJoinedRoom()
    {
        Lobby.SetStatus($"Joined {PhotonNetwork.CurrentRoom.Name} as {PhotonNetwork.NickName}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Lobby.SetStatus(message);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PhotonNetwork.LoadLevel("ShapeSelectOnline");
    }

    void NewScene(Scene current, Scene next)
    {
        GameObject pingTextGO = GameObject.Find("Ping");
        if (pingTextGO != null)
        {
            pingText = pingTextGO.GetComponent<Text>();
        }
    }
}
