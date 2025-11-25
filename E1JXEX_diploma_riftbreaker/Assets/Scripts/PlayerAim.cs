using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    public Transform aimTarget;
    [SerializeField] private LayerMask groundMask;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        MoveTargetToMouse();
        RotatePlayerTowardsTarget();
    }
    private void MoveTargetToMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
        {
            aimTarget.position = hit.point;
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