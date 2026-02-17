using UnityEngine;

public class PlayerMining : MonoBehaviour
{
    [SerializeField] private PlayerAim playerAim;
    [SerializeField] private float miningRate = 1.0f;
    [SerializeField] private int miningAmount = 1;
    [SerializeField] private float range = 4.0f;
    [SerializeField] private LayerMask mineableLayer;

    private float nextMineTime;
    private Camera cam;
    private InputSystemActions.PlayerActions playerInput;

    void Start()
    {
        playerAim = GetComponent<PlayerAim>();
        cam = Camera.main;
        playerInput = InputManager.instance.input.Player;
    }

    void Update()
    {
        if (playerInput.Mine.IsPressed())
        {
            if (Time.time >= nextMineTime)
            {
                TryMine();
                nextMineTime = Time.time + (1f / miningRate);
            }
        }
    }

    private void TryMine()
    {
        Vector3 rayDirection = playerAim.aimTarget.position - cam.transform.position;
        if (Physics.Raycast(cam.transform.position, rayDirection, out RaycastHit hit, Mathf.Infinity, mineableLayer))
        {
            if (Vector3.Distance(transform.position,hit.transform.position)<=range)
            {
                if (hit.collider.TryGetComponent(out Mineable mineable))
                {
                    mineable.Mine(miningAmount);
                }
            }
        }
    }
}