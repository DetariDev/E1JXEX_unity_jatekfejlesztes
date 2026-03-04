using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class BaseUpgradeTier
{
    public string tierName = "Level 2 Upgrade";
    public List<ResourceCost> upgradeCosts;
    public GameObject upgradeVisuals;

}

public class MainBase : MonoBehaviour
{
    public static MainBase Instance { get; private set; }
    public GameObject baseVisual;
    public TMP_Text neededResourcesUIText;
    public int currentLevel = 1;
    public event Action OnBaseLeveledUp;
    public List<BaseUpgradeTier> baseTiers = new List<BaseUpgradeTier>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ResourceTextUpdate();
    } 

    public BaseUpgradeTier GetNextTier()
    {
        int nextTierIndex = currentLevel - 1;

        if (nextTierIndex < baseTiers.Count)
        {
            return baseTiers[nextTierIndex];
        }
        return null;
    }
    public BaseUpgradeTier GetCurrentTier()
    {
        int currentTierIndex = currentLevel - 2;
        if (currentTierIndex >= 0 && currentTierIndex < baseTiers.Count)
        {
            return baseTiers[currentTierIndex];
        }
        return null;
    }
    public void TryUpgradeBase()
    {
        BaseUpgradeTier nextTier = GetNextTier();

        if (nextTier == null)
        {
            return;
        }
        foreach (var cost in nextTier.upgradeCosts)
        {
            if (!ResourceManager.Instance.HasEnoughResource(cost.resourceType, cost.amount))
            {
                return;
            }
        }
        foreach (var cost in nextTier.upgradeCosts)
        {
            ResourceManager.Instance.SpendResource(cost.resourceType, cost.amount);
        }
        currentLevel++;
        OnBaseLeveledUp?.Invoke();
        ResourceTextUpdate();
        if (nextTier.upgradeVisuals != null)
        {
            baseVisual.SetActive(false);
            if (GetCurrentTier() != null)
            {
                GetCurrentTier().upgradeVisuals.SetActive(false);
            }
            nextTier.upgradeVisuals.SetActive(true);
        }
    }
    void ResourceTextUpdate()
    {
        BaseUpgradeTier nextTier = GetNextTier();
        if (nextTier == null)
        {
            neededResourcesUIText.text = "Max Level!";
            return;
        }
        string resourceText = $"Base {nextTier.tierName}\n";
        foreach (var cost in nextTier.upgradeCosts)
        {
            resourceText += $"{cost.resourceType}: {cost.amount}\n";
        }
        neededResourcesUIText.text = resourceText;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerCarrier carrier))
        {
            carrier.DeliverResources();
        }
    }
}