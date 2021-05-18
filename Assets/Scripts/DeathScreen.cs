using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] Button restart;
    [SerializeField] GameObject deathScreen;

    // Start is called before the first frame update
    void Start()
    {
        restart.onClick.AddListener(RestartGame);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        GameManager.ChangeScene("LevelB");
    }

    public void DeathMenu()
    {
        deathScreen.SetActive(true);
        GetComponent<PauseMenu>().alive = false;
        Cursor.lockState = CursorLockMode.None;
    }
}
