using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameKeeper : MonoBehaviour
{
    private ScoreKeeper scoreKeeper;
    private BallObject ballObject;
    private PlayerMovement playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        ballObject = FindObjectOfType<BallObject>();
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetGame()
    {
        // Reset Score
        var currScore = scoreKeeper.ResetScore();
        
        // Reset Ball Position / Variables
        ballObject.ResetBall();
        
        // Reset Player Position / Variables
        playerMovement.ResetPlayer();
    }
}
