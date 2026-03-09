using System;
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
    public int power;
    public int maxPower;

    public Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    public event Action<List<string>> OnResourceChanged;
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
        power = maxPower;
    }

    public void AddResource(ResourceType resource, int quantity)
    {
        resources[resource] += quantity;
        OnResourceChanged?.Invoke(ResourceChanged());

    }
    public bool SpendResource(ResourceType resource, int quantity)
    {
        if (resources[resource] >= quantity)
        {
            resources[resource] -= quantity;
            OnResourceChanged?.Invoke(ResourceChanged());
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

    public List<string> ResourceChanged()
    {
        List<string> resourceStrings = new List<string>();
        foreach (var resource in resources)
        {
            resourceStrings.Add($"{resource.Key}: {resource.Value}");
        }
        return resourceStrings;
    }

    public void AddPowery(int plusPower)
    {
        if (power + plusPower > maxPower)
        {
            power = maxPower;
        }
        else
        {
            power += plusPower;
        }
    }
    public bool SpendPower(int spentPower)
    {
        if (power-spentPower>=0)
        {
            power -= spentPower;
            return true;
        }
        return false;
    }


}
