using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    private int currentScene;
    public GameObject hudUI;
    public GameObject pauseUI;

    [Header("Game Over")]
    public GameObject gameOverUI;
    public TMP_Text scoreText;
    public TMP_Text bestScoreText;
    public GameObject newBestScoreText;

    public bool isPaused = false;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }
    private void Update()
    {
        if (currentScene != 0 && GameManager.instance.isGameOver == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
            {
                Pause();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
            {
                Resume();
            }
        }
    }
    public void Pause()
    {
        Debug.Log("Pause");
        isPaused = true;
        pauseUI.SetActive(true);
        hudUI.SetActive(false);
        //Stop scoring
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        Debug.Log("Unpause");
        pauseUI.SetActive(false);
        hudUI.SetActive(true);
        //resume scoring
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GameOver()
    {
        if(GameManager.instance.scoreManager.CompareScore())
        {
            newBestScoreText.SetActive(true);
        }
        scoreText.text = "Your Score : " + GameManager.instance.scoreManager.GetScore().ToString();
        bestScoreText.text = "Best Score : " + GameManager.instance.scoreManager.GetBestScore().ToString();
        pauseUI.SetActive(false);
        hudUI.SetActive(false);
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Debug.Log("Restarting");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
