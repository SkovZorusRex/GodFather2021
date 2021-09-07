using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int currentScore = 0;
    private int bestScore = 0;

    public TMP_Text currentScoreText;
    public TMP_Text bestScoreText;

    // Start is called before the first frame update
    void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore");
        currentScore = 0;
        currentScoreText.text = currentScore.ToString();
        bestScoreText.text = bestScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor)
        {
            Debug.Log("Playing in Editor");
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

    public void CompareScore()
    {
        if (currentScore > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", currentScore);
        }
    }

    public int GetScore()
    {
        return currentScore;
    }

    public int GetBestScore()
    {
        return bestScore;
    }
}
