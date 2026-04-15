using System.Collections.Generic;
using UnityEngine;

public class PlayerCarrier : MonoBehaviour
{
    public Transform[] carrySlots;
    public LayerMask chunkLayer;
    public float pickupRadius = 4.0f;

    private List<ResourceChunk> carriedChunks = new List<ResourceChunk>();
    private InputSystemActions.PlayerActions playerInput;

    private void Start()
    {
        playerInput = InputManager.instance.input.Player;
        PlayerManager.Instance.OnStatsUpdated += ApplyWeightPenalty;
    }

    private void Update()
    {
        if (playerInput.Interact.WasPressedThisFrame())
        {
            TryPickupChunks();
        }
        if (playerInput.Drop.WasPressedThisFrame())
        {
            DropTopChunk();
        }
    }

    private void TryPickupChunks()
    {
        if (carriedChunks.Count >= carrySlots.Length) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRadius, chunkLayer);
        foreach (var hit in hits)
        {
            if (Mathf.Max(0f, 1f - ((carriedChunks.Count + 1) * PlayerManager.Instance.speedPenalty)) <= 0.1f) break;
            if (carriedChunks.Count >= carrySlots.Length) break;

            if (hit.TryGetComponent(out ResourceChunk chunk))
            {
                if (carriedChunks.Contains(chunk)) continue;

                Transform availableSlot = carrySlots[carriedChunks.Count];
                chunk.PickUp(availableSlot);
                carriedChunks.Add(chunk);
                ApplyWeightPenalty();
            }
        }
        
    }

    public void DropTopChunk()
    {
        if (carriedChunks.Count == 0) return;
        int lastIndex = carriedChunks.Count - 1;
        ResourceChunk chunkToDrop = carriedChunks[lastIndex];
        carriedChunks.RemoveAt(lastIndex);
        chunkToDrop.transform.SetParent(null);

        if (chunkToDrop.TryGetComponent(out Collider col)) col.enabled = true;
        if (chunkToDrop.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.AddForce(transform.forward * 2f + Vector3.up * 2f, ForceMode.Impulse);
        }

        ApplyWeightPenalty();
    }

    public void ApplyWeightPenalty()
    {
        float penaltyMultiplier = Mathf.Max(0f, 1f - (carriedChunks.Count * PlayerManager.Instance.speedPenalty));
        PlayerManager.Instance.currentSpeed = PlayerManager.Instance.baseSpeed * penaltyMultiplier;
        if (TutorialManager.instance != null)
        {
            TutorialManager.instance.ToggleKeyHint(KeyHint.Pickup, carriedChunks.Count < carrySlots.Length);
            TutorialManager.instance.ToggleKeyHint(KeyHint.Drop, carriedChunks.Count > 0);
        }
    }

    public void DeliverResources()
    {
        if (carriedChunks.Count == 0) return;

        foreach (var chunk in carriedChunks)
        {
            ResourceManager.Instance.AddResource(chunk.resourceType, chunk.amount);
            Destroy(chunk.gameObject);
        }

        carriedChunks.Clear();
        ApplyWeightPenalty();
    }
}