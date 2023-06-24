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
    private bool runing;
    public static GameManager instance;
    public GameObject vidasUI;
    public PlayerController player;
    public TextMeshProUGUI textcoin;
    public int coin;
    public TMP_Text saveText;

    public GameObject panelPause;
    public GameObject panelGameOver;
    public GameObject panelLoading;
    public GameObject panelFinalGame;

    public bool nextLevel;
    public int actualLevel;
    public List<Transform> positionsN = new List<Transform>();
    public List<Transform> positionsB = new List<Transform>();
    public GameObject transitionPanel;

    private void Awake()
    {
        if (instance == null) //Singelton
            instance = this;

        else
            Destroy(this.gameObject);
        if(PlayerPrefs.GetInt("lives") != 0) 
            ChargeData();
    }

    public void ActiveTransitionPanel()
    {
        transitionPanel.GetComponent<Animator>().SetTrigger("Hidden");
    }

    public void ChangePoss()
    {
        if (nextLevel)
        {
            if (actualLevel+1 < positionsN.Count)
            {
                player.transform.position = positionsN[actualLevel + 1].transform.position;
                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player.GetComponent<Animator>().SetBool("Walk", false);
                player.endMap = false;
            } 
        }
        else
        {
            if (positionsB.Count < actualLevel-1)
            {
                player.transform.position = positionsB[actualLevel - 1].transform.position;
                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player.GetComponent<Animator>().SetBool("Walk", false);
                player.endMap = false;
            }
        }
    }

    public void SaveData()
    {
        float x, y;
        x = player.transform.position.x;
        y = player.transform.position.y;

        int lives = player.lives;
        string sceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt("coins", coin);
        PlayerPrefs.SetFloat("x", x);
        PlayerPrefs.SetFloat("x", y);
        PlayerPrefs.SetInt("lives", lives);
        PlayerPrefs.SetString("sceneName", sceneName);

        if (!runing)
            StartCoroutine(SaveText());
    }

    private IEnumerator SaveText()
    {
        runing = true;
        saveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        saveText.gameObject.SetActive(false);
        runing = false;
    }

    public void ChargeLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void ChargeData()
    {
        coin = PlayerPrefs.GetInt("coins");
        player.transform.position = new Vector2(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y"));
        player.lives = PlayerPrefs.GetInt("lives");
        textcoin.text = coin.ToString();
        /*if(PlayerPrefs.GetString("sceneName") == string.Empty)
            SceneManager.LoadScene("LevelSelect");
        else
            SceneManager.LoadScene(PlayerPrefs.GetString("sceneName"));
        */
        int livesRest = 5 - player.lives;
        player.ActuLivesUI(livesRest);
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
    public void FinalGame()
    {
        panelFinalGame.SetActive(true);
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
