using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cutscenes : MonoBehaviour
{
    [SerializeField] Button changeScene;
    [SerializeField] Image loadBar;
    [SerializeField] float vel;
    [SerializeField] int sceneInd;
    AsyncOperation loadin;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.ChangeMusic(GameManager.musicVol);
        changeScene.onClick.AddListener(ChangeSceneButton);
        StartCoroutine(ScreenLoad());
        loadBar.fillAmount = 0;
    }

    private void ChangeSceneButton()
    {
        loadin.allowSceneActivation = true;
    }

    IEnumerator ScreenLoad()
    {
        float t = 0;
        changeScene.gameObject.SetActive(false);
        loadin = SceneManager.LoadSceneAsync(sceneInd);
        loadin.allowSceneActivation = false;
        //yield return new WaitForSeconds(2);
        while (loadBar.fillAmount <= 0.88f)
        {
            loadBar.fillAmount = Mathf.Lerp(loadBar.fillAmount, loadin.progress, Time.deltaTime * vel);
            yield return null;
        }
        //loadBar.fillAmount = 1;
        loadBar.gameObject.SetActive(false);
        changeScene.gameObject.SetActive(true);
    }
}
