using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerAim : MonoBehaviour
{
    public Transform aimTarget;
    [SerializeField] private LayerMask groundMask;
    public Vector3 lastPosition;
    [SerializeField] private float gamepadCursorSpeed = 1500f;
    private InputSystemActions.PlayerActions playerInput;
    private InputManager inputManager;
    private Camera mainCamera;
    private Vector2 virtualCursorPos;
    private void Awake()
    {
        inputManager = InputManager.instance;
        playerInput = inputManager.input.Player;
        mainCamera = Camera.main;
        lastPosition = new Vector2();
    }
    private void Update()
    {
        MoveTargetToMouse();
        RotatePlayerTowardsTarget();
    }

    private void MoveTargetToMouse()
    {
        CastRayFromScreenPoint(playerInput.MouseLook.ReadValue<Vector2>());
    }

    private void CastRayFromScreenPoint(Vector2 screenPoint)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
        {
            aimTarget.position = hit.point;
            lastPosition = aimTarget.position;
        }
    }
    private void RotatePlayerTowardsTarget()
    {
        Vector3 direction = (aimTarget.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

}