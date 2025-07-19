using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFader : MonoBehaviour
{
    public static UIFader Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public void FadeIn(CanvasGroup canvasGroup, float duration)
    {
        StartCoroutine(Fade(0f, 1f, duration, canvasGroup));
    }

    public void FadeOut(CanvasGroup canvasGroup, float duration)
    {
        StartCoroutine(Fade(1f, 0f, duration, canvasGroup));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float fadeDuration, CanvasGroup canvasGroup)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
