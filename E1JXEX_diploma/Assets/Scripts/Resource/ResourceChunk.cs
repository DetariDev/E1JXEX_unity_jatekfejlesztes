using UnityEngine;

public class ResourceChunk : MonoBehaviour
{
    public ResourceType resourceType;
    public int amount = 1;
    private void Start()
    {
        if (TutorialManager.instance.currentStage== TutorialStage.Mining) { 
            TutorialManager.instance.NextStage();
        }
    }
    public void PickUp(Transform slot)
    {
        if (TryGetComponent(out Collider col)) col.enabled = false;
        if (TryGetComponent(out Rigidbody rb)) rb.isKinematic = true;

        transform.SetParent(slot);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        if (TutorialManager.instance.currentStage == TutorialStage.Pickup)
        {
            TutorialManager.instance.NextStage();
        }
    }
}