using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private int baseMaxHealth = 100;
    private float baseBaseSpeed = 5f;
    private float baseMaxStamina = 10f;

    public int maxHealth;
    public float baseSpeed;
    public float maxStamina;
    public float staminaRegenRate = 5f;
    public float staminaDrainRate = 2f;

    public int currentHealth;
    public float currentSpeed;
    public float currentStamina;

    public bool isRunning = false;
    public bool staminaDrained;
    public bool sprintToggle;
    public bool inBuildState = false;

    public DrillHead currentDrillHead;
    [SerializeField] public DrillHead defaultDrillHead;

    public List<MechUpgrade> availableUpgrades = new List<MechUpgrade>();
    public MechUpgrade headUpgrade;
    public MechUpgrade bodyUpgrade;
    public MechUpgrade armUpgrade;
    public MechUpgrade legUpgrade;
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
        currentHealth = maxHealth;
        currentSpeed = baseSpeed;
        currentStamina = maxStamina;
        currentDrillHead = defaultDrillHead;
        InputManager.instance.input.Player.BuildToggle.performed += HandleBuilding;
    }

    public void HandleStamina(bool isMoving)
    {
        if (sprintToggle && isMoving && currentStamina > 0 && !staminaDrained)
        {
            isRunning = true;
            currentStamina -= staminaDrainRate * Time.deltaTime;
            if (currentStamina < 0.1f)
            {
                staminaDrained = true;
                isRunning = false;
                sprintToggle = false;
            }
        }
        else
        {
            isRunning = false;
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                if (currentStamina > maxStamina / 2)
                {
                    staminaDrained = false;
                }
            }
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    public void HandleBuilding(InputAction.CallbackContext context)
    {
        inBuildState = !inBuildState;
    }

    public void EquipUpgrade(MechUpgrade upgrade)
    {
        switch (upgrade.place)
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

        ApplyModifiers(headUpgrade);
        ApplyModifiers(bodyUpgrade);
        ApplyModifiers(armUpgrade);
        ApplyModifiers(legUpgrade);

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        currentSpeed = baseSpeed;
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
                case UpgradeType.WaterSpeed:
                    break;
                case UpgradeType.MiningYield:
                    break;
            }
        }
    }
}