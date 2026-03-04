using UnityEngine;

public class ResourceChunk : MonoBehaviour
{
    public ResourceType resourceType;
    public int amount = 1;
    public void PickUp(Transform slot)
    {
        if (TryGetComponent(out Collider col)) col.enabled = false;
        if (TryGetComponent(out Rigidbody rb)) rb.isKinematic = true;

        transform.SetParent(slot);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}