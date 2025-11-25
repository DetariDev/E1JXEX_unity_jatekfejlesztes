using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerManager playerManager;
    private Rigidbody rb;
    private InputSystemActions input;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = new InputSystemActions();
    }
    private void OnEnable()
    {
        input.Player.Enable();
    }
    private void OnDisable()
    {
        input.Player.Disable();
    }
    void Update()
    {
        moveInput = input.Player.Move.ReadValue<Vector2>();
        playerManager.HandleStamina(input.Player.Sprint.IsPressed(), moveInput.magnitude > 0.1f);
    }

    void FixedUpdate()
    {
        float finalSpeed = playerManager.currentSpeed * (playerManager.isRunning ? 2f : 1f);
        Vector3 targetVelocity = new Vector3(moveInput.x * finalSpeed, rb.linearVelocity.y, moveInput.y * finalSpeed);
        rb.linearVelocity = targetVelocity;
    }
}