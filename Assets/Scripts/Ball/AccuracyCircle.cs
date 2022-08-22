using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyCircle : MonoBehaviour
{
    public bool followMode = true;
    
    [SerializeField] private Transform ballTransform;
    [SerializeField] private float scaleFactor = 1f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float hitHeight;
    private Vector3 initScale;
    private BallObject ballObject;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initScale = transform.localScale;
        hitHeight = FindObjectOfType<BallObject>().GetHitHeight();
        ballObject = ballTransform.GetComponent<BallObject>();
    }
    
    void Update()
    {
        if (followMode)
        {
            TrackBallXZ();
        }
        else
        {
            UpdateBallAdvanced();
        }
        
        ScaleCircle();
    }
    
    private void TrackBallXZ()
    {
        var height = transform.position.y;
        var ballPosition = ballTransform.position;
        var newPosition = new Vector3(ballPosition.x, height, ballPosition.z);
        transform.position = newPosition;
    }

    private void UpdateBallAdvanced()
    {
        if (!ballObject.GetPlayerLastHit())
        {
            var landingSpot = ballObject.GetLandingSpot();
            transform.position = new Vector3(landingSpot.x, transform.position.y, landingSpot.z);
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    private void ScaleCircle()
    {
        var heightDifference = ballTransform.position.y - hitHeight;
        var newScale = initScale.z;
        newScale *= 1f + ((heightDifference / 10f) * scaleFactor) ;
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
