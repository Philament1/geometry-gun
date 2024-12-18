using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyGameManager : MonoBehaviour
{
    static Text p1ScoreText, p2ScoreText;
    static GameObject p1WinText, p2WinText;

    static bool isRunning;

    Vector2 spawnPos = new Vector2(4.6f, 0);

    // Start is called before the first frame update
    void Start()
    {
        GameObject p1ScoreObject, p2ScoreObject;
        p1ScoreObject = GameObject.Find("P1 Score");
        p2ScoreObject = GameObject.Find("P2 Score");
        p1ScoreText = p1ScoreObject.GetComponent<Text>();
        p2ScoreText = p2ScoreObject.GetComponent<Text>();
        p1WinText = p1ScoreObject.transform.GetChild(0).gameObject;
        p2WinText = p2ScoreObject.transform.GetChild(0).gameObject;
        p1ScoreText.text = GameInfo.p1Score.ToString();
        p2ScoreText.text = GameInfo.p2Score.ToString();

        GameObject p1, p2;
        if (NetworkController.IsP1 || !GameInfo.isOnline)
        {
            string p1PrefabPath = $"Prefabs/Shapes/{GameInfo.p1Shape}";
            string p2PrefabPath = $"Prefabs/Shapes/{GameInfo.p2Shape}";

            if (GameInfo.isOnline)
            {
                p1 = NetworkController.InstantiateGO(p1PrefabPath, spawnPos * Vector2.left, true);
                p2 = NetworkController.InstantiateGO(p2PrefabPath, spawnPos, false);
            }
            else
            {
                p1 = Instantiate(Resources.Load<GameObject>(p1PrefabPath), spawnPos * Vector2.left, Quaternion.identity);
                p2 = Instantiate(Resources.Load<GameObject>(p2PrefabPath), spawnPos, Quaternion.identity);            } //WHAT THE BHARNEEDHARAN IS THIS WHY THE BHARNEEDHARAN IS IT ON THE SAME LINE OH MY BHARNEEDHARAN

            if (GameInfo.isOnline)
            {
                NetworkController.SendCallMethodRPC("MyGameManager", "LoadComplete", new object[0]);
            }
            LoadComplete(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //RPC
    public static void LoadComplete(int timestamp)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            bool isControllable = true;
            bool isP1;
            if (players[i].transform.position.x < 0)
            {
                isP1 = true;
                players[i].transform.name = "Player 1";
                if (GameInfo.isOnline && !NetworkController.IsP1)
                {
                    isControllable = false;
                }
            }
            else
            {
                isP1 = false;
                players[i].transform.name = "Player 2";                             //i think i fixed online ?
                if (GameInfo.isOnline && NetworkController.IsP1)
                {
                    isControllable = false;
                }
            }
            players[i].GetComponent<Shape>().SetP1(isP1, isControllable);
        }
    }

    public static void PlayerWon(bool isP1)
    {
        if (isRunning)
        {
            if (isP1)
            {
                GameInfo.p2Score++;
                p2ScoreText.text = GameInfo.p2Score.ToString();
                p2WinText.SetActive(true);
            }
            else
            {
                GameInfo.p1Score++;
                p1ScoreText.text = GameInfo.p1Score.ToString();
                p1WinText.SetActive(true);
            }
            isRunning = false;
        }

        if (GameInfo.isOnline)
        {
            NetworkController.LoadScene("ShapeSelectOnline");
        }
        else
        {
            SceneManager.LoadScene("ShapeSelectLocal");
        }
    }
    public static void RunGame()
    {
        isRunning = true;
    }

    //RPC
    public static void InstantiateBullet(int timestamp, string prefabName, bool isP1, float speed, int damage, float bulletWidth, float bulletHeight, Vector2 bulletSpawnPosition, string bulletName)
    {
        GameObject firer = null;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (isP1 && players[i].transform.position.x < 0 || !isP1 && players[i].transform.position.x > 0)
            {
                firer = players[i];
                break;
            }
        }

        GameObject bullet = Instantiate(Resources.Load<GameObject>($"Prefabs/Bullets/{prefabName}"), bulletSpawnPosition, Quaternion.identity);
        bullet.transform.localScale = new Vector3(bulletWidth, bulletHeight, 1);
        bullet.name = bulletName;
        bullet.GetComponent<Bullet>().Initialise(isP1, speed, damage, firer);

        //anti-lag... pretty cool, right? Yes, Robert, it is. Thanks bro. <3

        float latency = (NetworkController.Timestamp - timestamp) / 1000f;
        bullet.transform.Translate(speed * latency * Vector2.right);
        print($"Fired network bullet with a latency compensation of {latency} s");
    }

    //RPC
    public static void SyncBulletCollision(int timestamp, string bulletName, string targetName)
    {
        try
        {
            Transform bullet = GameObject.Find(bulletName).transform;
            Transform target = GameObject.Find(targetName).transform;
            bullet.position = target.position;
        } catch { }
    }
}
