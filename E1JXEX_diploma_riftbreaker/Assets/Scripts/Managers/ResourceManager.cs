using System.Collections.Generic;
using UnityEngine;
using VInspector;
public enum ResourceType
{
    Wood,
    Stone,
    Metal
}
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    
    public Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
        {
            resources.Add(type, 0);
        }
    }

    public void AddResource(ResourceType resource, int quantity)
    {
        resources[resource] += quantity;
    }
    public bool SpendResource(ResourceType resource, int quantity)
    {
        if (resources[resource] >= quantity)
        {
            resources[resource] -= quantity;
            return true;
        }
        return false;
    }
    public bool HasEnoughResource(ResourceType resource, int quantity)
    {
        if (resources[resource]>= quantity)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
