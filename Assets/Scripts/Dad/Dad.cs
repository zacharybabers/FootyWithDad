using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dad : MonoBehaviour
{
    [SerializeField] private BallObject ballObject;
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float targetTolerance = 1f;

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
        if (GetAtDestination())
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
        if (a - targetTolerance <= b && a + targetTolerance >= b)
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

    public bool GetAtDestination()
    {
        return CheckClose(transform.position.x, moveTarget.x) && CheckClose(transform.position.z, moveTarget.z);
    }

    public float GetTimeAtDestination()
    {
        return timeAtDestination;
    }
}
