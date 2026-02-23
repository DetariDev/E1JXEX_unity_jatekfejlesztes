using System;
using UnityEngine;

[Serializable]
public struct ResourceCost
{
    public ResourceType resourceType;
    public int amount;
}

public abstract class RecipeBase : ScriptableObject
{
    public ResourceCost[] resourceCost;
}