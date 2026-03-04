using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private PlayerAim playerAim;
    [SerializeField] private float maxDistance = 6f;
    [SerializeField] private LayerMask interactionLayer;

    private InputSystemActions.PlayerActions playerInput;
    private Camera cam;
    private void Start()
    {
        if (!playerAim) playerAim = GetComponent<PlayerAim>();
        playerInput = InputManager.instance.input.Player;
        InputManager.instance.input.Player.Interact.performed += _ => TryInteract();
        cam = Camera.main;
    }
    private void TryInteract()
    {
        if (!playerAim.aimTarget) return;

        Vector3 rayDirection = playerAim.aimTarget.position - cam.transform.position;

        if (Physics.Raycast(cam.transform.position, rayDirection, out RaycastHit hit, Mathf.Infinity, interactionLayer))
        {
            if ((transform.position - hit.transform.position).sqrMagnitude <= maxDistance * maxDistance)
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact();
                }
            }
        }
    }
}