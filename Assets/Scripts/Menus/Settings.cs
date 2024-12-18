using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    Slider volSlider;
    InputField volInput;
    Toggle pitchIncreaseToggle;
    Dropdown songDropdown;

    // Start is called before the first frame update
    void Start()
    {
        volSlider = GameObject.Find("Volume Slider").GetComponent<Slider>();
        volInput = GameObject.Find("Volume Input").GetComponent<InputField>();
        pitchIncreaseToggle = GameObject.Find("Pitch Increase Toggle").GetComponent<Toggle>();
        songDropdown = GameObject.Find("Song Dropdown").GetComponent<Dropdown>();

        float volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        volSlider.value = volume;
        volInput.text = volume.ToString("0.00");
        pitchIncreaseToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("Music Pitch Increase", 1));
        for (int i = 0; i < Songs.songs.Length; i++)
        {
            songDropdown.options.Add(new Dropdown.OptionData(Songs.songs[i].name));
        }
        songDropdown.value = PlayerPrefs.GetInt("Song Index", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Apply()
    {
        PlayerPrefs.SetFloat("Volume", volSlider.value);
        PlayerPrefs.SetInt("Music Pitch Increase", System.Convert.ToInt32(pitchIncreaseToggle.isOn));
        PlayerPrefs.SetInt("Song Index", songDropdown.value);

        GameObject.Find("Persistent Original").GetComponent<Persistent>().UpdateSettings();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SliderChanged(string setting)
    {
        switch(setting)
        {
            case "Volume":
                volInput.text = volSlider.value.ToString("0.00");
                break;
        }
    }

    public void InputFieldChanged(string setting)
    {
        switch (setting)
        {
            case "Volume":
                volSlider.value = System.Convert.ToSingle(volInput.text);
                break;
        }
    }
}
