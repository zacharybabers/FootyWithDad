using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    private BallObject ballObject;
    void Start()
    {
        ballObject = FindObjectOfType<BallObject>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(ballObject.GetRotationAxis(), rotationSpeed * Time.deltaTime * ballObject.GetHorizontalVelocity());
    }
}
