using UnityEngine;

public class WeaponFactory : ICraftingFactory
{
    public void Craft(RecipeBase recipe)
    {
        if (recipe is WeaponRecipe)
        {
            WeaponManager.instance.availableWeapons.Add((recipe as WeaponRecipe).weaponToCraft);
        }
    }
}
