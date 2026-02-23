using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool TryCraft(RecipeBase recipe)
    {
        foreach (var resource in recipe.resourceCost)
        {
            if (ResourceManager.Instance.HasEnoughResource(resource.resourceType, resource.amount))
            {
                continue;
            }
            else
            {
                Debug.Log("Not enough resources to craft!");
                return false;
            }
        }
        foreach (var resource in recipe.resourceCost)
        {
            ResourceManager.Instance.SpendResource(resource.resourceType, resource.amount);
        }
        return true;

    }


}
