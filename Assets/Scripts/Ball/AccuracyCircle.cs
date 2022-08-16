using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyCircle : MonoBehaviour
{
    [SerializeField] private Transform ballTransform;
    [SerializeField] private float scaleFactor = 1f;

    private float hitHeight;
    private Vector3 initScale;
    
    void Start()
    {
        initScale = transform.localScale;
        hitHeight = FindObjectOfType<BallObject>().GetHitHeight();
    }
    
    void Update()
    {
        TrackBallXZ();
        ScaleCircle();
    }
    
    private void TrackBallXZ()
    {
        var height = transform.position.y;
        var ballPosition = ballTransform.position;
        var newPosition = new Vector3(ballPosition.x, height, ballPosition.z);
        transform.position = newPosition;
    }

    private void ScaleCircle()
    {
        var heightDifference = ballTransform.position.y - hitHeight;
        var newScale = initScale.z;
        newScale *= 1f + ((heightDifference / 10f) * scaleFactor) ;
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
