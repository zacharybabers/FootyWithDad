using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivot : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    private Vector3 target;

    private BallObject ballObject;

    private float yVelocity = 0f;

    void Awake()
    {
        ballObject = FindObjectOfType<BallObject>();
    }
    
    /*void Update()
    {
        target = ballObject.GetLandingSpot();
        Debug.Log(target);

        // Direction vector from pivot to target
        Vector3 direction = target - transform.position;
        direction *= -1f;

        if (direction.sqrMagnitude < 0.001f) return;

        // Desired angle around Y
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // Current Y angle
        float currentAngle = transform.eulerAngles.y;

        // Smoothly interpolate
        float newY = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        // Explicitly set only the Y rotation
        transform.rotation = Quaternion.Euler(0f, newY, 0f);
    }*/
    
    void Update()
    {
        if (!ballObject.GetPlayerLastHit())
        {
            SmoothRotation(); 
        }
    }

    void SmoothRotation()
    {
        target = ballObject.GetLandingSpot();
        

        Vector3 direction = target - transform.position;
        direction *= -1f;
        direction.y = 0f; // make sure we're on XZ plane

        if (direction.sqrMagnitude < 0.001f) return;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float currentAngle = transform.eulerAngles.y;

        // SmoothDampAngle parameters:
        // current, target, ref velocity, smoothTime
        float smoothTime = 1f / rotationSpeed; // adjust smoothTime based on your speed
        float newY = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref yVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0f, newY, 0f);
    }
}
