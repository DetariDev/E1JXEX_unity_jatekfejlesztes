using System;
using UnityEngine;
public enum UpgradeType
{
    MaxHealth,
    MaxStamina,
    BaseSpeed,
    WaterSpeed,
    MiningYield
}
[Serializable]
public struct UpgradeModifier
{
    public UpgradeType type;
    public float value;
}

public class UpgradeRecipe : RecipeBase
{
    public UpgradeModifier[] modifiers;
}
