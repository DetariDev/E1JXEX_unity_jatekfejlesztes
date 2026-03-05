using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMechUpgradeRecipe", menuName = "Recipes/New Mech Upgrade Recipe")]
public class MechUpgradeRecipe : RecipeBase
{
    public MechUpgrade itemToCraft;
}
