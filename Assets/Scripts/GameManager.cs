using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static float volume = 1;
    public static float musicVol = 1;

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        ChangeVolume(volume);
        ChangeMusic(musicVol);
    }


    public static void ChangeVolume(float vol)
    {
        foreach (AudioSource audioS in GameObject.FindObjectsOfType<AudioSource>())
        {
            if (audioS.gameObject.tag != "Music")
            {
                audioS.volume = vol;
            }
        }
        volume = vol;
    }

    public static void ChangeVolume(Slider sld)
    {
        foreach (AudioSource audioS in GameObject.FindObjectsOfType<AudioSource>())
        {
            if (audioS.gameObject.tag != "Music")
            {
                audioS.volume = sld.value;
            }
        }
        volume = sld.value;
    }

    public static void ChangeMusic(float vol)
    {
        foreach (GameObject audioS in GameObject.FindGameObjectsWithTag("Music"))
        {
            audioS.GetComponent<AudioSource>().volume = vol;
        }
        musicVol = vol;
    }

    public static void ChangeMusic(Slider sld)
    {
        foreach (GameObject audioS in GameObject.FindGameObjectsWithTag("Music"))
        {
            audioS.GetComponent<AudioSource>().volume = sld.value;
        }
        musicVol = sld.value;
    }

    public static void ChangeScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public static void ChangeScene(string s)
    {
        SceneManager.LoadScene(s);
    }

    public static void ChangeResolution(Dropdown d)
    {
        switch (d.value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, true);
                break;
            case 1:
                Screen.SetResolution(1366, 768, true);
                break;
            case 2:
                Screen.SetResolution(1440, 900, true);
                break;
            case 3:
                Screen.SetResolution(1536, 864, true);
                break;
        }
    }

    public static void ChangeResolution(int d)
    {
        switch (d)
        {
            case 0:
                Screen.SetResolution(1920, 1080, true);
                break;
            case 1:
                Screen.SetResolution(1366, 768, true);
                break;
            case 2:
                Screen.SetResolution(1440, 900, true);
                break;
            case 3:
                Screen.SetResolution(1536, 864, true);
                break;
        }
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
