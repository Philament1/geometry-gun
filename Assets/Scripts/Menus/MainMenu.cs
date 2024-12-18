using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject persistent = GameObject.Find("Persistent");
        GameObject originalPersistant = GameObject.Find("Persistent Original");
        if (originalPersistant != null)
        {
            Destroy(persistent);
        }
        else
        {
            persistent.name = "Persistent Original";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
