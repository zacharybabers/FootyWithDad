using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float movementSpeed = 10f;
    private CharacterController characterController;
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(GetLookPosition());
        DoMovement();
    }

    private void DoMovement()
    {
        var inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        var camTransform = mainCamera.transform;
        var flatForward = camTransform.forward;
        flatForward.y = 0;
        var flatRight = camTransform.right;
        flatRight.y = 0;
        var movementVector = flatRight.normalized + flatForward.normalized;
        movementVector *= movementSpeed * Time.deltaTime;
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
}
