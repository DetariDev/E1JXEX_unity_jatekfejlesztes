using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    public PlayerManager playerManager;
    public Canvas buildCanvas;
    public TMP_Text currentBuildingText;
    public TMP_Text resourcesText;
    public BuildSystem buildSystem;

    

    private void Start()
    {
        playerManager = PlayerManager.Instance;
        playerManager.OnBuildStateToggle += HandleBuildState;
        buildSystem.OnCurrentBuildingChanged += UpdateCurrentBuildingText;
        ResourceManager.Instance.OnResourceChanged += UpdateResourcesText;
    }

    private void UpdateResourcesText(List<string> list)
    {
        resourcesText.text = string.Join("\n", list);
    }

    private void UpdateCurrentBuildingText(BuildingRecipe recipe)
    {
        currentBuildingText.text = $"{recipe.buildingName}";
    }

    private void HandleBuildState(bool obj)
    {
        buildCanvas.enabled = obj;
    }
}
