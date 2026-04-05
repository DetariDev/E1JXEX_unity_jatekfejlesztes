using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public event Action<bool> OnMenuToggle;
    public event Action<bool> OnBuildStateToggle;
    public event Action OnStatsUpdated;

    private int baseMaxHealth = 100;
    private float baseBaseSpeed = 5f;
    private float baseMaxStamina = 10f;

    public int maxHealth;
    public float baseSpeed;
    public float maxStamina;
    public float staminaRegenRate = 5f;
    public float staminaDrainRate = 2f;

    public float speedPenalty;
    public float baseSpeedPenalty = 0.25f;

    private int _currentHealth;

    public int CurrentHealth
    {
        get { return _currentHealth; }
        set 
        {
            _currentHealth = value;
            OnHealthChanged?.Invoke(_currentHealth);
        }
    }

    public float currentSpeed;
    private float _currentStamina;

    public float CurrentStamina
    {
        get { return _currentStamina; }
        set 
        {
            _currentStamina = value;
            OnStaminaChanged?.Invoke(_currentStamina);
        }
    }

    public event Action<int> OnHealthChanged;
    public event Action<float> OnStaminaChanged;
    public bool isRunning = false;
    public bool staminaDrained;
    public bool sprintToggle;
    public bool inBuildState = false;
    public bool inMenu = false;

    public DrillHead currentDrillHead;
    public List<DrillHead> availableDrillHeads;
    public int drillspeedmodification = 0;

    public List<MechUpgrade> availableUpgrades = new List<MechUpgrade>();
    public List<MechUpgradeRecipe> availableUpgradeRecipes = new List<MechUpgradeRecipe>();
    public MechUpgrade headUpgrade;
    public MechUpgrade bodyUpgrade;
    public MechUpgrade armUpgrade;
    public MechUpgrade legUpgrade;
    public GameObject playerModel;

    public List<BuildingRecipe> unlockedBuildings = new List<BuildingRecipe>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CurrentHealth = maxHealth;
        currentSpeed = baseSpeed;
        CurrentStamina = maxStamina;
        currentDrillHead = availableDrillHeads[0];
        speedPenalty = baseSpeedPenalty;
        InputManager.instance.input.Player.BuildToggle.performed += HandleBuilding;
        InputManager.instance.input.Player.UpgradeToggle.performed += ToggleUpgradeMenu;
        if (TutorialManager.instance.currentStage == TutorialStage.None)
        {
            TutorialManager.instance.NextStage();
        }

    }

    private void ToggleUpgradeMenu(InputAction.CallbackContext context)
    {
        inMenu = !inMenu;
        OnMenuToggle?.Invoke(inMenu);
        inBuildState = false;
        OnBuildStateToggle?.Invoke(inBuildState);
    }

    public void HandleBuilding(InputAction.CallbackContext context)
    {
        if (inMenu)
        {
            inBuildState = false;
        }
        else
        {
            inBuildState = !inBuildState;
        }
        OnBuildStateToggle?.Invoke(inBuildState);
    }

    public void HandleStamina(bool isMoving)
    {
        if (sprintToggle && isMoving && CurrentStamina > 0 && !staminaDrained)
        {
            isRunning = true;
            CurrentStamina -= staminaDrainRate * Time.deltaTime;
            if (CurrentStamina < 0.1f)
            {
                staminaDrained = true;
                isRunning = false;
                sprintToggle = false;
            }
        }
        else
        {
            isRunning = false;
            if (CurrentStamina < maxStamina)
            {
                CurrentStamina += staminaRegenRate * Time.deltaTime;
                if (CurrentStamina > maxStamina / 2)
                {
                    staminaDrained = false;
                }
            }
        }

        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, maxStamina);
    }

    public void EquipUpgrade(UpgradePlace place, MechUpgrade upgrade)
    {
        switch (place)
        {
            case UpgradePlace.Head:
                headUpgrade = upgrade;
                break;
            case UpgradePlace.Body:
                bodyUpgrade = upgrade;
                break;
            case UpgradePlace.Arm:
                armUpgrade = upgrade;
                break;
            case UpgradePlace.Leg:
                legUpgrade = upgrade;
                break;
        }
        UpdateStats();
    }

    public void UpdateStats()
    {
        maxHealth = baseMaxHealth;
        baseSpeed = baseBaseSpeed;
        maxStamina = baseMaxStamina;
        speedPenalty = baseSpeedPenalty;
        drillspeedmodification = 0;

        ApplyModifiers(headUpgrade);
        ApplyModifiers(bodyUpgrade);
        ApplyModifiers(armUpgrade);
        ApplyModifiers(legUpgrade);

        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, maxStamina);
        currentSpeed = baseSpeed;
        OnStatsUpdated?.Invoke();
    }


    private void ApplyModifiers(MechUpgrade item)
    {
        if (item == null) return;
        foreach (var mod in item.modifiers)
        {
            switch (mod.type)
            {
                case UpgradeType.MaxHealth:
                    maxHealth += Mathf.RoundToInt(mod.value);
                    break;
                case UpgradeType.MaxStamina:
                    maxStamina += mod.value;
                    break;
                case UpgradeType.BaseSpeed:
                    baseSpeed += mod.value;
                    break;
                case UpgradeType.MiningYield:
                    drillspeedmodification += Mathf.RoundToInt(mod.value);
                    break;
                case UpgradeType.carryPenalty:
                    speedPenalty -= mod.value;
                    break;
            }
        }
    }
}