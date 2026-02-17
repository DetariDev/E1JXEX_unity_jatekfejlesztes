using System.Collections.Generic;
using UnityEngine;
public enum ResourceType
{
    Wood,
    Stone,
    Metal
}
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    public Dictionary<string, int> resources = new Dictionary<string, int>();
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
            resources.Add(type.ToString(), 0);
        }
    }

    public void AddResource(ResourceType resource, int quantity)
    {
        resources[resource.ToString()] += quantity;
    }
    public bool SpendResource(ResourceType resource, int quantity)
    {
        if (resources[resource.ToString()] >= quantity)
        {
            resources[resource.ToString()] -= quantity;
            return true;
        }
        return false;
    }


}
