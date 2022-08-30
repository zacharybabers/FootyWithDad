using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private Dad dadObject;
    

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        var text = "";
        text += "dad at location: " + dadObject.GetAtDestination();
        text += "\n";
        text += "time at location: " + dadObject.GetTimeAtDestination();
        debugText.text = text;
    }
}
