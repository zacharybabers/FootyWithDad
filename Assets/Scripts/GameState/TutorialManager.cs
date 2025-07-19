using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup kickTutorial;
    [SerializeField] private CanvasGroup moveTutorial;

    [SerializeField] private float fadeDuration = 0.2f;
    
    public static TutorialManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        kickTutorial.alpha = 0f;
        moveTutorial.alpha = 0f;
    }

    public void FadeInKickTutorial()
    {
        UIFader.Instance.FadeIn(kickTutorial, fadeDuration);
    }

    public void FadeOutKickTutorial()
    {
        UIFader.Instance.FadeOut(kickTutorial, fadeDuration);
    }
    
    public void FadeInMoveTutorial()
    {
        UIFader.Instance.FadeIn(moveTutorial, fadeDuration);
    }

    public void FadeOutMoveTutorial()
    {
        UIFader.Instance.FadeOut(moveTutorial, fadeDuration * 3f);
    }
}
