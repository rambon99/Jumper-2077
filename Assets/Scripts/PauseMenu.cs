using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] ScriptObj saveData;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] Button cont, save, menu;
    [SerializeField] Slider volume, music;
    [SerializeField] Dropdown resolution;

    public bool alive = true;
    // Start is called before the first frame update
    void Start()
    {
        saveData.RestartCoins();
        volume.value = GameManager.volume;
        music.value = GameManager.musicVol;
        GameManager.ChangeMusic(music.value);
        GameManager.ChangeVolume(volume.value);
        save.onClick.AddListener(saveData.SaveCoins);
        cont.onClick.AddListener(Pause);
        menu.onClick.AddListener(() => GameManager.ChangeScene(0));
        volume.onValueChanged.AddListener(GameManager.ChangeVolume);
        music.onValueChanged.AddListener(GameManager.ChangeMusic);
        resolution.onValueChanged.AddListener(GameManager.ChangeResolution);
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && alive)
        {
            Pause();
        }
    }

    private void Pause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
