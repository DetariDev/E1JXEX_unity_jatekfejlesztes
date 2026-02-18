using UnityEngine;

public class Mineable : MonoBehaviour
{
    [SerializeField] public ResourceType resourceType;
    [SerializeField] private int resourceAmount = 100;

    public void Mine(int amount)
    {
        if (resourceAmount <= 0) return;

        resourceAmount -= amount;
        ResourceManager.Instance.AddResource(resourceType, amount);

        if (resourceAmount <= 0)
        {
            Destroy(gameObject);
        }
    }
}