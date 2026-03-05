using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SuitUpgradeUI : MonoBehaviour
{
    public GameObject[] upgradePurchaseButtons;
    public Canvas upgradeCanvas;
    public TMP_Dropdown headDropdown;
    public TMP_Dropdown bodyDropdown;
    public TMP_Dropdown armDropdown;
    public TMP_Dropdown legDropdown;
    public TMP_Dropdown DrillheadDropDown;

    PlayerManager playerManager;


    private List<MechUpgrade> headUpgrades = new List<MechUpgrade>();
    private List<MechUpgrade> bodyUpgrades = new List<MechUpgrade>();
    private List<MechUpgrade> armUpgrades = new List<MechUpgrade>();
    private List<MechUpgrade> legUpgrades = new List<MechUpgrade>();

    private List<DrillHead> drillHeads = new List<DrillHead>();

    private void Start()
    {
        playerManager = PlayerManager.Instance;

        headDropdown.onValueChanged.AddListener(OnHeadSelected);
        bodyDropdown.onValueChanged.AddListener(OnBodySelected);
        armDropdown.onValueChanged.AddListener(OnArmSelected);
        legDropdown.onValueChanged.AddListener(OnLegSelected);
        DrillheadDropDown.onValueChanged.AddListener(OnDrillheadSelected);
        UpdateMechUpgradeRecipes();
        UpdateAvailableUpgrades();

    }



    void UpdateAvailableUpgrades()
    {
        headDropdown.ClearOptions();
        bodyDropdown.ClearOptions();
        armDropdown.ClearOptions();
        legDropdown.ClearOptions();
        DrillheadDropDown.ClearOptions();

        headUpgrades.Clear();
        bodyUpgrades.Clear();
        armUpgrades.Clear();
        legUpgrades.Clear();
        drillHeads.Clear();

        headDropdown.options.Add(new TMP_Dropdown.OptionData("None"));
        bodyDropdown.options.Add(new TMP_Dropdown.OptionData("None"));
        armDropdown.options.Add(new TMP_Dropdown.OptionData("None"));
        legDropdown.options.Add(new TMP_Dropdown.OptionData("None"));
        DrillheadDropDown.AddOptions(playerManager.availableDrillHeads.ConvertAll(drillhead => new TMP_Dropdown.OptionData(drillhead.name)));
        drillHeads.AddRange(playerManager.availableDrillHeads);

        headUpgrades.Add(null);
        bodyUpgrades.Add(null);
        armUpgrades.Add(null);
        legUpgrades.Add(null);

        foreach (MechUpgrade upgrade in playerManager.availableUpgrades)
        {
            switch (upgrade.place)
            {
                case UpgradePlace.Head:
                    headDropdown.options.Add(new TMP_Dropdown.OptionData(upgrade.name));
                    headUpgrades.Add(upgrade);
                    break;
                case UpgradePlace.Body:
                    bodyDropdown.options.Add(new TMP_Dropdown.OptionData(upgrade.name));
                    bodyUpgrades.Add(upgrade);
                    break;
                case UpgradePlace.Arm:
                    armDropdown.options.Add(new TMP_Dropdown.OptionData(upgrade.name));
                    armUpgrades.Add(upgrade);
                    break;
                case UpgradePlace.Leg:
                    legDropdown.options.Add(new TMP_Dropdown.OptionData(upgrade.name));
                    legUpgrades.Add(upgrade);
                    break;
            }
        }

        headDropdown.SetValueWithoutNotify(Mathf.Max(0, headUpgrades.IndexOf(playerManager.headUpgrade)));
        bodyDropdown.SetValueWithoutNotify(Mathf.Max(0, bodyUpgrades.IndexOf(playerManager.bodyUpgrade)));
        armDropdown.SetValueWithoutNotify(Mathf.Max(0, armUpgrades.IndexOf(playerManager.armUpgrade)));
        legDropdown.SetValueWithoutNotify(Mathf.Max(0, legUpgrades.IndexOf(playerManager.legUpgrade)));
        DrillheadDropDown.SetValueWithoutNotify(Mathf.Max(0, drillHeads.IndexOf(playerManager.currentDrillHead)));

        headDropdown.RefreshShownValue();
        bodyDropdown.RefreshShownValue();
        armDropdown.RefreshShownValue();
        legDropdown.RefreshShownValue();
        DrillheadDropDown.RefreshShownValue();
    }

    private void OnHeadSelected(int index) { playerManager.EquipUpgrade(UpgradePlace.Head, headUpgrades[index]); }
    private void OnBodySelected(int index) { playerManager.EquipUpgrade(UpgradePlace.Body, bodyUpgrades[index]); }
    private void OnArmSelected(int index) { playerManager.EquipUpgrade(UpgradePlace.Arm, armUpgrades[index]); }
    private void OnLegSelected(int index) { playerManager.EquipUpgrade(UpgradePlace.Leg, legUpgrades[index]); }

    private void OnDrillheadSelected(int index) { playerManager.currentDrillHead = drillHeads[index]; }


    private void UpdateMechUpgradeRecipes()
    {
        for (int i = 0; i < upgradePurchaseButtons.Length; i++)
        {
            if (playerManager.availableUpgradeRecipes.Count > i)
            {
                upgradePurchaseButtons[i].SetActive(true);
                MechUpgradeRecipe currentRecipe = playerManager.availableUpgradeRecipes[i];
                string recipecost = "";
                for (int j = 0; j < playerManager.availableUpgradeRecipes[i].resourceCost.Length; j++)
                {
                    recipecost += $"{playerManager.availableUpgradeRecipes[i].resourceCost[j].resourceType}:{playerManager.availableUpgradeRecipes[i].resourceCost[j].amount}\n";
                }
                upgradePurchaseButtons[i].GetComponentInChildren<TMP_Text>().text = $"{playerManager.availableUpgradeRecipes[i].name}\n{recipecost}\n level:{playerManager.availableUpgradeRecipes[i].minBaseLevel}";
                recipecost = "";
                if (MainBase.Instance.currentLevel < playerManager.availableUpgradeRecipes[i].minBaseLevel)
                {
                    upgradePurchaseButtons[i].GetComponent<Image>().color = Color.orangeRed;
                }
                else
                {
                    upgradePurchaseButtons[i].GetComponent<Image>().color = Color.darkSeaGreen;
                }

                Button upgradePurchaseButton = upgradePurchaseButtons[i].GetComponent<Button>();
                upgradePurchaseButton.onClick.RemoveAllListeners();
                upgradePurchaseButton.onClick.AddListener(() => CraftMechUpgrade(currentRecipe));
            }
            else
            {
                upgradePurchaseButtons[i].SetActive(false);
            }

        }
    }

    public void CraftMechUpgrade(MechUpgradeRecipe mechUpgradeRecipe)
    {
        bool tryCraft = CraftingManager.Instance.TryCraft(mechUpgradeRecipe);
        if (tryCraft)
        {
            playerManager.availableUpgradeRecipes.Remove(mechUpgradeRecipe);
            UpdateMechUpgradeRecipes();
            UpdateAvailableUpgrades();
        }
    }


}