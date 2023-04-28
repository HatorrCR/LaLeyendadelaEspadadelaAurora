using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject vidasUI;
    public PlayerController player;
    public TextMeshProUGUI textcoin;
    public int coin;

    public GameObject panelPause;
    public GameObject panelGameOver;
    public GameObject panelLoading;


    private void Awake()
    {
        if (instance == null) //Singelton
            instance = this;

        else
            Destroy(this.gameObject);
    }

    public void ActualCoin()
    {
        coin++;
        textcoin.text = coin.ToString();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        panelPause.SetActive(true);
    }

    public void NoPause()
    {
        Time.timeScale = 1;
        panelPause.SetActive(false);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSelector()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void ReloadScene(string esceneload)
    {
        SceneManager.LoadScene(esceneload);
    }

    public void GameOver()
    {
        panelGameOver.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadingSceneSelect()
    {
        StartCoroutine(LoadingScene());
    }
    private IEnumerator LoadingScene()
    {
        panelLoading.SetActive(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelSelect");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
