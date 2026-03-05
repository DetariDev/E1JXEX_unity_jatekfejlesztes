using System;
using UnityEngine;

[Serializable]
public struct ResourceCost
{
    public ResourceType resourceType;
    public int amount;
}

public enum UpgradeType
{
    MaxHealth,
    MaxStamina,
    BaseSpeed,
    MiningYield,
    carryPenalty
}

[Serializable]
public struct UpgradeModifier
{
    public UpgradeType type;
    public float value;
}

public abstract class RecipeBase : ScriptableObject
{
    public ResourceCost[] resourceCost;
    public int minBaseLevel;
}