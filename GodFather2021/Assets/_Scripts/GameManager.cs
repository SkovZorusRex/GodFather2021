using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public bool isGameOver = false;

    public ScoreManager scoreManager;
    public UIManager uIManager;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("BarrezVous");
        isGameOver = false;

        if (scoreManager == null)
        {
            scoreManager = FindObjectOfType<ScoreManager>();
        }

        if (uIManager == null)
        {
            uIManager = FindObjectOfType<UIManager>();
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        uIManager.GameOver();
    }
}
