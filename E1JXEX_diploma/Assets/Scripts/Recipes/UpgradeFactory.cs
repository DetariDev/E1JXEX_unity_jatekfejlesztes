using UnityEngine;

public class UpgradeFactory : ICraftingFactory
{
    public void Craft(RecipeBase recipe)
    {
        if (recipe is MechUpgradeRecipe upgradeRecipe)
        {
            PlayerManager.Instance.availableUpgrades.Add(upgradeRecipe.itemToCraft);
        }
    }
}