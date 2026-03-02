using UnityEngine;

public class MainBase : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out PlayerCarrier carrier))
            {
                carrier.DeliverResources();
            }
        }
    }
}