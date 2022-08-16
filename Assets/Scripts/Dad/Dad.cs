﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dad : MonoBehaviour
{
    [SerializeField] private BallObject ballObject;
    [SerializeField] private float moveSpeed = 12f;

    private Vector3 moveTarget;
    private float timeAtDestination;
    void Start()
    {
        moveTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
        UpdateMoveTarget();
        UpdateTimeAtDestination();
    }
    
    private void MoveToTarget()
    {
        var moveDirection = (moveTarget - transform.position);
        moveDirection.y = 0f;
        transform.Translate(moveDirection.normalized * (moveSpeed * Time.deltaTime));
    }

    private void UpdateTimeAtDestination()
    {
        if (CheckClose(transform.position.x, moveTarget.x) && CheckClose(transform.position.z, moveTarget.z))
        {
            timeAtDestination += Time.deltaTime;
        }
        else
        {
            timeAtDestination = 0f;
        }
    }

    private bool CheckClose(float a, float b)
    {
        if (a - 1f <= b && a + 1f >= b)
        {
            return true;
        }

        return false;
    }
    
    private void UpdateMoveTarget()
    {
        if (ballObject.GetPlayerLastHit())
        {
            moveTarget = ballObject.GetLandingSpot();
        }
    }

    public float GetTimeAtDestination()
    {
        return timeAtDestination;
    }
}