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
    private Camera mainCamera;
    private Vector2 virtualCursorPos;
    private bool isGamepadMode = false;
    private Vector3 lastGamepadPos;
    private void Awake()
    {
        playerInput = InputManager.instance.input.Player;
        mainCamera = Camera.main;
        lastPosition = new Vector2();
    }
    private void Update()
    {
        HandleInputSwitch();
        if (isGamepadMode)
        {
            MoveTargetToGamepad();
        }
        else
        {
            MoveTargetToMouse();
        }
        RotatePlayerTowardsTarget();
    }

    private void HandleInputSwitch()
    {
        Vector2 gamepadInput = playerInput.Look.ReadValue<Vector2>();
        if (gamepadInput.magnitude >0 && isGamepadMode == false)
        {
            isGamepadMode = true;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isGamepadMode = false;
        }
    }

    private void MoveTargetToMouse()
    {
        CastRayFromScreenPoint(playerInput.MouseLook.ReadValue<Vector2>());
    }
    private void MoveTargetToGamepad()
    {

        Vector2 lookInput = playerInput.Look.ReadValue<Vector2>();
        if (playerInput.GamepadAim.IsPressed())
        {
            Vector3 direction = new Vector3(lookInput.x, 0, lookInput.y);
            if (direction.magnitude > 0.1f)
            {
                lastGamepadPos = direction.normalized;

            }
            aimTarget.position = transform.position + lastGamepadPos * 10f;
        }
        else
        {
            virtualCursorPos += lookInput * gamepadCursorSpeed * Time.deltaTime;
            virtualCursorPos.x = Mathf.Clamp(virtualCursorPos.x,Screen.width*0.2f, Screen.width*0.8f);
            virtualCursorPos.y = Mathf.Clamp(virtualCursorPos.y,Screen.height * 0.2f, Screen.height* 0.8f);
            CastRayFromScreenPoint(virtualCursorPos);
        }
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