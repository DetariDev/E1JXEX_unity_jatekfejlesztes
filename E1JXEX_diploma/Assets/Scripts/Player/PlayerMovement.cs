using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerManager playerManager;
    private Rigidbody rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (playerManager.currentHealth <= 0) return;

        moveInput = InputManager.instance.input.Player.Move.ReadValue<Vector2>();
        if (InputManager.instance.input.Player.Sprint.triggered)
        {
            playerManager.sprintToggle = !playerManager.sprintToggle;
        }
        playerManager.HandleStamina(moveInput.magnitude > 0.1f);
    }

    void FixedUpdate()
    {
        if (playerManager.currentHealth <= 0)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        float finalSpeed = playerManager.currentSpeed * (playerManager.isRunning ? 2f : 1f);
        Vector3 targetVelocity = new Vector3(moveInput.x * finalSpeed, rb.linearVelocity.y, moveInput.y * finalSpeed);
        rb.linearVelocity = targetVelocity;
    }
}