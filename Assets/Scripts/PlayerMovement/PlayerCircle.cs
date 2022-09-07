using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerCircle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer circleSprite;

    private CircleState circleState = CircleState.BaseColor;
    private float shiftTimeLeft;
    private float currentTotalShiftTime;
    private Color targetColor;
    private Color startingColor;
    
    // Start is called before the first frame update
    private void Start()
    {
        startingColor = circleSprite.color;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateCircleState();
        shiftTimeLeft -= Time.deltaTime;
        Mathf.Clamp(shiftTimeLeft, 0f, Mathf.Infinity);
    }

    private void UpdateCircleState()
    {
        switch (circleState)
        {
            case CircleState.BaseColor:
                break;
            case CircleState.ShiftingTo:
                //update color with lerp
                circleSprite.color = Color.Lerp(startingColor, targetColor, ((currentTotalShiftTime / 2f) - shiftTimeLeft) / (currentTotalShiftTime / 2f));
                if (shiftTimeLeft <= 0f)
                {
                    circleState = CircleState.ShiftingBack;
                    shiftTimeLeft = currentTotalShiftTime / 2f;
                }
                break;
            case CircleState.ShiftingBack:
                //update color with lerp
                circleSprite.color = Color.Lerp(targetColor, startingColor, ((currentTotalShiftTime / 2f) - shiftTimeLeft) / (currentTotalShiftTime / 2f));
                if (shiftTimeLeft <= 0f)
                {
                    circleState = CircleState.BaseColor;
                    shiftTimeLeft = 0f;
                    circleSprite.color = startingColor;
                }
                break;
        }
    }

    public void ColorShift(Color shiftedTo, float shiftTime)
    {
        targetColor = shiftedTo;
        shiftTimeLeft = shiftTime / 2f;
        currentTotalShiftTime = shiftTime;
        circleState = CircleState.ShiftingTo;
    }
    
    

    private enum CircleState
    {
        ShiftingTo,
        ShiftingBack,
        BaseColor
    }
}
