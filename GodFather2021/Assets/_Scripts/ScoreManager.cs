using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private float currentScore = 0;
    private float bestScore = 0;
    private float multiplier = 1;

    public TMP_Text currentScoreText;
    public TMP_Text bestScoreText;

    // Start is called before the first frame update
    void Start()
    {
        multiplier = 1;
        bestScore = PlayerPrefs.GetInt("BestScore");
        currentScore = 0;
        currentScoreText.text = currentScore.ToString();
        bestScoreText.text = bestScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isGameOver && !GameManager.instance.uIManager.isPaused)
        {
            if (Application.isEditor)
            {
                currentScore += 1;

                if (Input.GetKeyDown(KeyCode.N))
                {
                    CompareScore();
                }
            }

            currentScoreText.text = currentScore.ToString();
            if (currentScore > bestScore)
            {
                bestScoreText.text = currentScore.ToString();
            }
        }
    }

    public bool CompareScore()
    {
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("BestScore", Mathf.RoundToInt(currentScore));
            return true;
        }
        else
            return false;
    }

    public int GetScore()
    {
        return Mathf.RoundToInt(currentScore);
    }

    public int GetBestScore()
    {
        return Mathf.RoundToInt(bestScore);
    }
}
