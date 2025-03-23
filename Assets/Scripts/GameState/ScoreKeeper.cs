using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{


    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;
    
    void Start()
    {
        score = 0;
    }

    private void Update()
    {
        scoreText.text = score.ToString();
    }

    public int AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        return score;
    }

    public void SetScore(int newScore)
    {
        score = newScore;
    }

    public int ResetScore()
    {
        var lastScore = score;
        SetScore(0);
        return lastScore;
    }

    public int GetScore()
    {
        return score;
    }
}
