using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class WeaponMakerUI : MonoBehaviour
{
    public GameObject[] weaponButtons;
    WeaponManager weaponManager;
    private void Start()
    {
        weaponManager = WeaponManager.instance;
        UpdateWeaponRecipes();
        MainBase.Instance.OnBaseLeveledUp += UpdateWeaponRecipes;
    }
    private void OnDestroy()
    {
        if (MainBase.Instance != null)
        {
            MainBase.Instance.OnBaseLeveledUp -= UpdateWeaponRecipes;
        }
    }

    private void UpdateWeaponRecipes()
    {
        for (int i = 0; i < weaponButtons.Length; i++)
        {
            if (weaponManager.weaponRecipes.Count>i)
            {
                weaponButtons[i].SetActive(true);
                WeaponRecipe currentRecipe = weaponManager.weaponRecipes[i];
                string recipecost = "";
                for (int j = 0; j < weaponManager.weaponRecipes[i].resourceCost.Length; j++)
                {
                    recipecost += $"{weaponManager.weaponRecipes[i].resourceCost[j].resourceType}:{weaponManager.weaponRecipes[i].resourceCost[j].amount}\n";
                }
                weaponButtons[i].GetComponentInChildren<TMP_Text>().text = $"{weaponManager.weaponRecipes[i].name}\n{recipecost}\n level:{weaponManager.weaponRecipes[i].minBaseLevel}";
                recipecost = "";
                if (MainBase.Instance.currentLevel< weaponManager.weaponRecipes[i].minBaseLevel)
                {
                    weaponButtons[i].GetComponent<Image>().color = Color.orangeRed;
                }
                else
                {
                    weaponButtons[i].GetComponent<Image>().color = Color.darkSeaGreen;
                }

                Button weaponButton = weaponButtons[i].GetComponent<Button>();
                weaponButton.onClick.RemoveAllListeners();
                weaponButton.onClick.AddListener(() => CraftWeapon(currentRecipe));
            }
            else
            {
                weaponButtons[i].SetActive(false);
            }
            
        }
    }

    public void CraftWeapon(WeaponRecipe weaponRecipe)
    {
        bool tryCraft = CraftingManager.Instance.TryCraft(weaponRecipe);
        if (tryCraft)
        {
            weaponManager.weaponRecipes.Remove(weaponRecipe);
            UpdateWeaponRecipes();
        }
    }
}
