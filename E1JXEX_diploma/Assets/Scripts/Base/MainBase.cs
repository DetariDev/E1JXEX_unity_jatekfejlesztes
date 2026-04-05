using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class BaseUpgradeTier
{
    public string tierName = "Level 2 Upgrade";
    public List<ResourceCost> upgradeCosts;
    public GameObject upgradeVisuals;
    public int maxHealth;
}

public class MainBase : MonoBehaviour
{
    public static MainBase Instance { get; private set; }
    public GameObject baseVisual;
    public TMP_Text neededResourcesUIText;
    public int currentLevel = 1;
    public event Action OnBaseLeveledUp;
    public List<BaseUpgradeTier> baseTiers = new List<BaseUpgradeTier>();
    private ObjectHealth baseHealth;

    public Image healthBar;
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        baseHealth = GetComponent<ObjectHealth>();
        ResourceTextUpdate();

        baseHealth.OnHealthChanged += UpdateHealthUI;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (baseHealth != null)
        {
            healthBar.fillAmount = (float)baseHealth.currentHealth / baseHealth.maxHealth;

            if (baseHealth.currentHealth <= 0)
            {
                TriggerGameOver();
            }
        }
    }
    private void TriggerGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        if (nextTier == null) return;

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
        baseHealth.maxHealth = nextTier.maxHealth;
        OnBaseLeveledUp?.Invoke();
        ResourceTextUpdate();

        if (TutorialManager.instance.currentStage == TutorialStage.UpgradeBase)
        {
            TutorialManager.instance.NextStage();
        }

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

    public void BaseEnclosed()
    {
        if (TutorialManager.instance == null || TutorialManager.instance.currentStage != TutorialStage.BuildWall)
            return;

        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
        Vector3 distantPoint = transform.position + new Vector3(125f, 0f, 125f);
        if (UnityEngine.AI.NavMesh.SamplePosition(distantPoint, out UnityEngine.AI.NavMeshHit hit, 15f, UnityEngine.AI.NavMesh.AllAreas))
        {
            UnityEngine.AI.NavMesh.CalculatePath(transform.position, hit.position, UnityEngine.AI.NavMesh.AllAreas, path);
            if (path.status != UnityEngine.AI.NavMeshPathStatus.PathComplete)
            {
                TutorialManager.instance.NextStage();
            }
        }
    }
}