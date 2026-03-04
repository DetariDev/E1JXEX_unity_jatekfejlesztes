using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
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

public class UpgradeRecipe : RecipeBase
{
    public MechUpgrade itemToCraft;
}
