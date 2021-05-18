using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button newGame, continueGame, warningYes, warningNo, quit;
    [SerializeField] Slider volume, music;
    [SerializeField] GameObject warning;
    [SerializeField] ScriptObj saveData;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.ChangeMusic(GameManager.musicVol);
        volume.value = GameManager.volume;
        music.value = GameManager.musicVol;
        newGame.onClick.AddListener(StartNew);
        quit.onClick.AddListener(GameManager.QuitGame);
        if (saveData.CheckCoins())
        {
            Time.timeScale = 1;
            continueGame.onClick.AddListener(() => GameManager.ChangeScene(1));
        }
        else
        {
            continueGame.interactable = false;
        }
        warningYes.onClick.AddListener(newChangeScene);
        warningNo.onClick.AddListener(() => warning.SetActive(false));

    }

    void StartNew()
    {
        if (saveData.CheckCoins())
        {
            warning.SetActive(true);
        }
        else
        {
            newChangeScene();
        }
    }

    void newChangeScene()
    {
        saveData.DeleteCoins();
        GameManager.ChangeScene("BeginScene");
    }
}
