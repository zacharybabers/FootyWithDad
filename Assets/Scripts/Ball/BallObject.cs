using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallObject : MonoBehaviour
{
    [SerializeField] private float hitHeight;
    [SerializeField] private Transform ballMeshTransform;
    [SerializeField] private Dad dad;
    [SerializeField] private PlayerCircle playerCircle;
    [SerializeField] private Collider playerCollider;
    [SerializeField] private float yAcceleration = -9.8f;
    [SerializeField] private float inaccuracyMultiplier = 1f;
    [SerializeField] private float dadInaccuracyMultiplier = 1f;
    [SerializeField] private float dadKickPrepTime = 1.5f;
    [SerializeField] private float kickPower = 40f;
    [SerializeField] private float playerCircleShiftTime = 1f;
    [SerializeField] private float goodKickDissonance = 0.3f;
    [SerializeField] private float deadTime = 1.5f;
    [SerializeField] private Color playerCircleGoodColor;
    [SerializeField] private Color playerCircleBadColor;
    [SerializeField] private Color playerCircleMissColor;
    [SerializeField] private bool debugMode;



    private Vector3 movementDirection;
    private Vector3 startPosition;
    private float verticalVelocity = 0f;
    private float horizontalVelocity = 0f;
    private float activeDeadTime = 0f;
    private Transform playerTransform;
    private ScoreKeeper scoreKeeper;
    private GameKeeper gameKeeper;
    private SoundPlayer soundPlayer;
    
    private bool inPlayerRange = false;
    private bool grounded = false;
    private bool playerLastHit = false;
    
    void Start()
    {
        playerTransform = playerCollider.transform;
        startPosition = transform.position;
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        soundPlayer = FindObjectOfType<SoundPlayer>();
        gameKeeper = FindObjectOfType<GameKeeper>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CheckKick();
        CheckDadKick();
        UpdateVelocity();
        var movementVector = new Vector3(0f, verticalVelocity, 0f) + (movementDirection.normalized * horizontalVelocity);
        transform.Translate(movementVector * Time.deltaTime);
        if (transform.position.y <= 0.5)
        {
            var pos = transform.position;
            pos.y = 0.5f;
            transform.position = pos;
            horizontalVelocity = 0f;
            grounded = true;
        }

        if (grounded)
        {
            activeDeadTime += Time.deltaTime;
            if (activeDeadTime >= deadTime)
            {
                gameKeeper.ResetGame();
            }
        }
    }

    private void UpdateVelocity()
    {
        verticalVelocity += yAcceleration * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider)
        {
            inPlayerRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == playerCollider)
        {
            inPlayerRange = false;
        }
    }

    private void CheckKick()
    {
        if (!Input.GetButtonDown("Kick"))
        {
            return;
        }
        if(!inPlayerRange || grounded)
        {
            playerCircle.ColorShift(playerCircleMissColor, playerCircleShiftTime);
            return;
        }

        movementDirection = CalculateKickDirection();
        horizontalVelocity = kickPower;
        verticalVelocity = kickPower;
        scoreKeeper.AddScore(1);
        soundPlayer.PlayRandomKick();
        playerLastHit = true;
    }

    private Vector3 CalculateKickDirection()
    {
        var heightDissonance = transform.position.y - hitHeight;
        var initMovement = playerTransform.forward;
        var movementChange = playerTransform.right;

        int inaccuracyRandomizer = 1;
        if (Random.Range(0f, 1f) > 0.5)
        {
            inaccuracyRandomizer = -1;
        }

        if (heightDissonance <= goodKickDissonance)
        {
            playerCircle.ColorShift(playerCircleGoodColor, playerCircleShiftTime);
        }
        else
        {
            playerCircle.ColorShift(playerCircleBadColor, playerCircleShiftTime);
        }

        initMovement = initMovement + (movementChange * (heightDissonance * inaccuracyMultiplier * inaccuracyRandomizer));
        

        return initMovement;
    }

    private Vector3 CalculateDadKickDirection()
    {
        var dissonance = dad.GetTimeAtDestination();
        if (dissonance > dadKickPrepTime)
        {
            dissonance = dadKickPrepTime;
        }
        dissonance = dadKickPrepTime - dissonance;
        var initMovement = playerTransform.position - dad.transform.position;
        var movementChange = Quaternion.Euler(0, 90, 0) * initMovement;

        int inaccuracyRandomizer = 1;
        if (Random.Range(0f, 1f) > 0.5)
        {
            inaccuracyRandomizer = -1;
        }

        initMovement = initMovement + (movementChange * (dissonance * dadInaccuracyMultiplier * inaccuracyRandomizer));
        

        return initMovement;
    }

    private float CalculateDadKickPower()
    {
        var dadPosition = dad.transform.position;
        var playerPosition = playerTransform.position;
        float initDistance = Vector3.Distance(new Vector3(playerPosition.x, 0f, playerPosition.z), new Vector3(dadPosition.x, 0f, dadPosition.z));
        
        //change distance depending on dissonance
        
        var dadKickPower = Mathf.Sqrt(-1f * (yAcceleration / 2f) * initDistance);

        return dadKickPower;
    }

    private void CheckDadKick()
    {
        if (!playerLastHit || transform.position.y >= hitHeight + .1f || transform.position.y <= hitHeight - .1f || dad.GetAtDestination() == false)
        {
            return;
        }
        
        movementDirection = CalculateDadKickDirection();
        var dadKickPower = CalculateDadKickPower();
        verticalVelocity = dadKickPower;
        horizontalVelocity = dadKickPower;
        scoreKeeper.AddScore(1);
        soundPlayer.PlayRandomKick();
        if (debugMode)
        {
            Debug.Log("dad kicked");
        }
        playerLastHit = false;
    }

    public float GetHitHeight()
    {
        return hitHeight;
    }
    
    public Vector3 GetLandingSpot()
    {
        if (transform.position.y == 0f)
        {
            return transform.position;
        }

        var a = (yAcceleration / 2f);
        var b = verticalVelocity;
        var c = transform.position.y;

        var timeToLand = ((b * -1f) + Mathf.Sqrt(Mathf.Pow(b, 2) - (4 * a * c))) / (2f * a);
        if (timeToLand <= 0f)
        {
            timeToLand = ((b * -1f) - Mathf.Sqrt(Mathf.Pow(b, 2) - (4 * a * c))) / (2f * a);
        }
        
        return timeToLand * horizontalVelocity * movementDirection.normalized + new Vector3(transform.position.x, 0f, transform.position.z);
    }

    public bool GetPlayerLastHit()
    {
        return playerLastHit;
    }

    public Vector3 GetRotationAxis()
    {
        var newDirection = Quaternion.Euler(0, 90, 0) * movementDirection;
        return newDirection;
    }

    public float GetHorizontalVelocity()
    {
        return horizontalVelocity;
    }

    public void ResetBall()
    {
        activeDeadTime = 0f;
        verticalVelocity = 0f;
        horizontalVelocity = 0f;
        inPlayerRange = false;
        grounded = false;
        playerLastHit = false;
        transform.position = startPosition;
    }
}
