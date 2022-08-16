using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingTester : MonoBehaviour
{
    [SerializeField] private BallObject ballObject;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ballObject.GetLandingSpot();
    }
}
