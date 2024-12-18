using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Persistent : MonoBehaviour
{
    VideoPlayer videoPlayer;
    AudioSource audioSource;
    bool increaseMusicPitch = false;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += NewScene;

        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();

        UpdateSettings();
    }

    // Update is called once per frame
    void Update()
    {
        if (increaseMusicPitch)
        {
            if (audioSource.pitch < Songs.current.maxPitch)
            {
                audioSource.pitch += Songs.current.pitchIncreaseRate * Time.deltaTime;
            }
            if (audioSource.pitch > Songs.current.maxPitch)
            {
                audioSource.pitch = Songs.current.maxPitch;
            }
        }
    }

    public void UpdateSettings()
    {
        audioSource.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        Songs.current = Songs.songs[PlayerPrefs.GetInt("Song Index", 0)];
        if (Songs.current.name != audioSource.clip.name)
        {
            audioSource.clip = Resources.Load<AudioClip>($"Audio/Music/{Songs.current.name}");
            audioSource.Play();
        }
    }

    void NewScene(Scene current, Scene next)
    {
        videoPlayer.targetCamera = Camera.main;

        if (PlayerPrefs.GetInt("Music Pitch Increase", 1) == 1)
        {
            if (next.name == "Game")
            {
                audioSource.pitch = Songs.current.minPitch;
                increaseMusicPitch = true;
            }
            else
            {
                audioSource.pitch = 1f;
                increaseMusicPitch = false;
            }
        }
    }
}
