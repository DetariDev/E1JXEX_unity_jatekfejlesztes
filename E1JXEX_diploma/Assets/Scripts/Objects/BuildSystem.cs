using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildSystem : MonoBehaviour
{
    public PlayerAim playerAim;
    public BuildingRecipe currentBuildingRecipe;
    public event Action<BuildingRecipe> OnCurrentBuildingChanged;

    PlayerManager playerManager;

    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();



    private void Start()
    {
        playerManager = PlayerManager.Instance;
        if (playerManager.unlockedBuildings.Count > 0)
        {
            currentBuildingRecipe = PlayerManager.Instance.unlockedBuildings[0];
            OnCurrentBuildingChanged?.Invoke(currentBuildingRecipe);

        }
        InputManager.instance.input.Player.ChangeWeapon.performed += ChangeBuildingRecipe;
    }

    private int currentBuildingIndex = 0;
    public void ChangeBuildingRecipe(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        List<BuildingRecipe> availableBuildings = playerManager.unlockedBuildings.FindAll(building => building.minBaseLevel <= MainBase.Instance.currentLevel);

        if (availableBuildings.Count <= 1) return;

        if (!InputManager.instance.isGamepadMode)
        {
            float scrollValue = context.ReadValue<Vector2>().y;

            if (scrollValue > 0)
            {
                currentBuildingIndex = (currentBuildingIndex + 1) % availableBuildings.Count;
            }
            else if (scrollValue < 0)
            {
                currentBuildingIndex = (currentBuildingIndex - 1 + availableBuildings.Count) % availableBuildings.Count;
            }
            currentBuildingRecipe = availableBuildings[currentBuildingIndex];
        }
        else
        {
            // gamepades epitesvaltast majd implementalni
        }

        OnCurrentBuildingChanged?.Invoke(currentBuildingRecipe);
    }

    private void Update()
    {
        if (!playerManager.inBuildState || playerAim.aimTarget == null || currentBuildingRecipe == null) return;

        if (InputManager.instance.input.Player.Attack.IsPressed())
        {
            TryBuild();
        }

        
    }

    private void TryBuild()
    {
        Vector3 targetPos = playerAim.aimTarget.position;
        Vector3 snapPos = new Vector3(Mathf.Round(targetPos.x), 0.5f, Mathf.Round(targetPos.z));
        float distanceFromPlayer = Vector3.Distance(playerManager.playerModel.transform.position, targetPos);
        if ( distanceFromPlayer <= 2 || distanceFromPlayer >= 10 || occupiedPositions.Contains(snapPos) || playerManager.inMenu) return;

        Collider[] hitColliders = Physics.OverlapBox(snapPos, new Vector3(0.45f, 0.45f, 0.45f), Quaternion.identity, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Building") || hit.CompareTag("Player"))
            {
                Debug.Log("Foglalt a hely!");
                return;
            }
        }

        foreach (var resource in currentBuildingRecipe.resourceCost)
        {
            if (!ResourceManager.Instance.HasEnoughResource(resource.resourceType, resource.amount)) return;
        }

        foreach (var resource in currentBuildingRecipe.resourceCost)
        {
            ResourceManager.Instance.SpendResource(resource.resourceType, resource.amount);
        }

        GameObject newBuilding = Instantiate(currentBuildingRecipe.buildingPrefab, snapPos, Quaternion.identity);
        newBuilding.tag = "Building";
        occupiedPositions.Add(snapPos);

    }
}