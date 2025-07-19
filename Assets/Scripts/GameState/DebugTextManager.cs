using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private TextMeshProUGUI difficultyDebugText;
    [SerializeField] private Dad dadObject;
    [SerializeField] private BallObject ballObject;
    

    // Update is called once per frame
    void Update()
    {
        UpdateText();
        UpdateDifficultyText();
    }

    private void UpdateText()
    {
        var text = "";
        text += "dad at location: " + dadObject.GetAtDestination();
        text += "\n";
        text += "time at location: " + dadObject.GetTimeAtDestination();
        debugText.text = text;
    }

    private void UpdateDifficultyText()
    {
        var difficulty = "kick power: " + ballObject.kickPower + ", gravity: " + ballObject.yAcceleration;
        difficultyDebugText.text = difficulty;
    }
}
