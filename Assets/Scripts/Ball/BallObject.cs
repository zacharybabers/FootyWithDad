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
    [SerializeField] private Collider playerCollider;
    [SerializeField] private float yAcceleration = -9.8f;
    [SerializeField] private float inaccuracyMultiplier = 1f;
    [SerializeField] private float dadInaccuracyMultiplier = 1f;
    [SerializeField] private float dadKickPrepTime = 1.5f;
    [SerializeField] private float kickPower = 40f;
    
    
   
    private Vector3 movementDirection;
    private float verticalVelocity = 0f;
    private float horizontalVelocity = 0f;
    private Transform playerTransform;
    
    private bool inPlayerRange = false;
    private bool grounded = false;
    private bool playerLastHit = false;
    
    void Start()
    {
        playerTransform = playerCollider.transform;
    }

    // Update is called once per frame
    void Update()
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
        if(!Input.GetButtonUp("Kick") || !inPlayerRange || grounded)
        {
            return;
        }

        movementDirection = CalculateKickDirection();
        horizontalVelocity = kickPower;
        verticalVelocity = kickPower;
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
        
        var dadKickPower = Mathf.Sqrt(4.9f * initDistance);

        return dadKickPower;
    }

    private void CheckDadKick()
    {
        if (!playerLastHit || transform.position.y >= hitHeight + .1f || transform.position.y <= hitHeight - .1f || dad.GetAtDestination() == false)
        {
            return;
        }
        
        Debug.Log("kicky kicky");
        movementDirection = CalculateDadKickDirection();
        var dadKickPower = CalculateDadKickPower();
        verticalVelocity = dadKickPower;
        horizontalVelocity = dadKickPower;
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

        return timeToLand * horizontalVelocity * movementDirection + new Vector3(transform.position.x, 0f, transform.position.z);
    }

    public bool GetPlayerLastHit()
    {
        return playerLastHit;
    }
}
