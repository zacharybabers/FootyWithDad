using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // enforce singleton
            return;
        }

        Instance = this;
    }
    
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Scenes/GameScene");
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("Scenes/StartMenu");
    }
    
    
    public void LoadEndMenu()
    {
        SceneManager.LoadScene("Scenes/EndMenu");
    }
}
