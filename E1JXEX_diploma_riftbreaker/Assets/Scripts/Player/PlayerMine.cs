using System.Linq;
using UnityEngine;

public class PlayerMining : MonoBehaviour
{
    [SerializeField] private PlayerAim playerAim;
    [SerializeField] private int miningAmount = 1;
    [SerializeField] private float range = 4.0f;
    [SerializeField] public LayerMask mineableLayer;
    [SerializeField] private ResourceType[] mineableResources = new ResourceType[0];

    private float nextMineTime;
    private Camera cam;
    private InputSystemActions.PlayerActions playerInput;

    void Start()
    {
        playerAim = GetComponent<PlayerAim>();
        cam = Camera.main;
        playerInput = InputManager.instance.input.Player;
        miningAmount = PlayerManager.Instance.currentDrillHead.miningAmount;
        range = PlayerManager.Instance.currentDrillHead.range;
        mineableResources = PlayerManager.Instance.currentDrillHead.mineableResources;

    }

    void Update()
    {
        if (playerInput.Mine.IsPressed())
        {
            if (Time.time >= nextMineTime)
            {
                TryMine(); 
                float totalMiningRate = PlayerManager.Instance.currentDrillHead.miningRate + PlayerManager.Instance.drillspeedmodification;
                nextMineTime = Time.time + (1f / totalMiningRate);
            }
        }
    }

    private void TryMine()
    {
        Vector3 rayDirection = playerAim.aimTarget.position - cam.transform.position;
        if (Physics.Raycast(cam.transform.position, rayDirection, out RaycastHit hit, Mathf.Infinity, mineableLayer))
        {
            if (Vector3.Distance(transform.position, hit.transform.position) <= PlayerManager.Instance.currentDrillHead.range)
            {
                if (hit.collider.CompareTag("Building"))
                {
                    Destroy(hit.collider.gameObject);
                    return;
                }
                if (hit.collider.TryGetComponent(out Mineable mineable))
                {
                    if (PlayerManager.Instance.currentDrillHead.mineableResources.Contains(mineable.resourceType))
                    {
                        mineable.Mine(PlayerManager.Instance.currentDrillHead.miningAmount);
                    }
                }
            }
        }
    }
}