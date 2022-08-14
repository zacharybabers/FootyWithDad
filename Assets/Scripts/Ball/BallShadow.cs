using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShadow : MonoBehaviour
{
    [SerializeField] private Transform ballTransform;
    private float initBallHeight;
    private Vector3 initScale;

    void Start()
    {
        initBallHeight = ballTransform.position.y;
        initScale = transform.localScale;
    }
    void Update()
    {
        TrackBallXZ();
        UpdateScale();
    }

    private void TrackBallXZ()
    {
        var height = transform.position.y;
        var ballPosition = ballTransform.position;
        var newPosition = new Vector3(ballPosition.x, height, ballPosition.z);
        transform.position = newPosition;
    }

    private void UpdateScale()
    {
        var normalizedHeight = ballTransform.position.y - initBallHeight;
        var newMult = ((2f / (1f + Mathf.Exp(normalizedHeight / 5f))) + 1f) / 2f;
        newMult *= initScale.z;
        transform.localScale = new Vector3(newMult, newMult, newMult);
    }
}
