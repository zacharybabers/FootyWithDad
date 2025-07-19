using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float slowedSpeed = 4f;
    private CharacterController characterController;
    private Vector3 startPosition;

    private CamTransform camTransform;
    private BallObject ballObject;

    private bool slowed = false;
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        startPosition = transform.position;
        camTransform = new CamTransform();
        UpdateCamTransform();
        ballObject = FindObjectOfType<BallObject>();
    }

    
    void Update()
    {
        if (!ballObject.GetTutorialDone())
        {
            transform.LookAt(GetLookPosition());
            return;
        }
        transform.LookAt(GetLookPosition());
        UpdateSlowedInput();
        DoMovement();
    }

    private void UpdateSlowedInput()
    {
        if (Input.GetButton("Slow"))
        {
            slowed = true;
        }
        else
        {
            slowed = false;
        }
    }

    private void DoMovement()
    {
        var finalMoveSpeed = movementSpeed;
        if (slowed)
        {
            finalMoveSpeed = slowedSpeed;
        }
        var inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        var flatForward = camTransform.camFlatForward;
        var flatRight = camTransform.camFlatRight;
        var movementVector = flatRight.normalized + flatForward.normalized;
        movementVector *= finalMoveSpeed * Time.deltaTime;
        movementVector.x *= inputVector.x;
        movementVector.z *= inputVector.y;
        characterController.Move(movementVector);
    }

    private Vector3 GetLookPosition()
    {
        var result = Vector3.zero;
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            result = hit.point;
        }

        result.y = transform.position.y;
        return result;
    }

    public void ResetPlayer()
    {
        transform.position = startPosition;
    }

    public void UpdateCamTransform()
    {
        // Update cam transform and flatten
        camTransform.camFlatForward = mainCamera.transform.forward;
        camTransform.camFlatForward.y = 0;
        
        camTransform.camFlatRight = mainCamera.transform.right;
        camTransform.camFlatForward.y = 0;
    }
    
    private struct CamTransform
    {
        public Vector3 camFlatForward;
        public Vector3 camFlatRight;
    }
}
