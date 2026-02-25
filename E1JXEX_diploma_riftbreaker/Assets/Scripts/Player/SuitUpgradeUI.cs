using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SuitUpgradeUI : MonoBehaviour
{
    public Canvas upgradeCanvas;
    public TMP_Dropdown headDropdown;
    public TMP_Dropdown bodyDropdown;
    public TMP_Dropdown armDropdown;
    public TMP_Dropdown legDropdown;

    PlayerManager playerManager;

    private List<MechUpgrade> headUpgrades = new List<MechUpgrade>();
    private List<MechUpgrade> bodyUpgrades = new List<MechUpgrade>();
    private List<MechUpgrade> armUpgrades = new List<MechUpgrade>();
    private List<MechUpgrade> legUpgrades = new List<MechUpgrade>();

    private void Start()
    {
        playerManager = PlayerManager.Instance;
        playerManager.OnUpgradeMenuToggle += ToggleUpgradeMenu;

        headDropdown.onValueChanged.AddListener(OnHeadSelected);
        bodyDropdown.onValueChanged.AddListener(OnBodySelected);
        armDropdown.onValueChanged.AddListener(OnArmSelected);
        legDropdown.onValueChanged.AddListener(OnLegSelected);

        UpdateAvailableUpgrades();
    }

    private void ToggleUpgradeMenu(bool obj)
    {
        upgradeCanvas.enabled = obj;
        if (obj)
        {
            UpdateAvailableUpgrades();
        }
    }

    private void OnDestroy()
    {
        if (playerManager != null)
        {
            playerManager.OnUpgradeMenuToggle -= ToggleUpgradeMenu;
        }
    }

    void UpdateAvailableUpgrades()
    {
        headDropdown.ClearOptions();
        bodyDropdown.ClearOptions();
        armDropdown.ClearOptions();
        legDropdown.ClearOptions();

        headUpgrades.Clear();
        bodyUpgrades.Clear();
        armUpgrades.Clear();
        legUpgrades.Clear();

        headDropdown.options.Add(new TMP_Dropdown.OptionData("None"));
        bodyDropdown.options.Add(new TMP_Dropdown.OptionData("None"));
        armDropdown.options.Add(new TMP_Dropdown.OptionData("None"));
        legDropdown.options.Add(new TMP_Dropdown.OptionData("None"));

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

        headDropdown.RefreshShownValue();
        bodyDropdown.RefreshShownValue();
        armDropdown.RefreshShownValue();
        legDropdown.RefreshShownValue();
    }

    private void OnHeadSelected(int index) { playerManager.EquipUpgrade(UpgradePlace.Head, headUpgrades[index]); }
    private void OnBodySelected(int index) { playerManager.EquipUpgrade(UpgradePlace.Body, bodyUpgrades[index]); }
    private void OnArmSelected(int index) { playerManager.EquipUpgrade(UpgradePlace.Arm, armUpgrades[index]); }
    private void OnLegSelected(int index) { playerManager.EquipUpgrade(UpgradePlace.Leg, legUpgrades[index]); }
}