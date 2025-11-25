using UnityEngine;
using VInspector;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Foldout("Base Statok")]
    public int maxHealth = 100;
    public float baseSpeed = 5f;
    public float maxStamina = 10f;
    public float staminaRegenRate = 5f;
    public float staminaDrainRate = 2f;

    [Foldout("Current Statok")]
    public int currentHealth;
    public float currentSpeed;
    public float currentStamina;

    [Foldout("Állapotjelzõk")]
    public bool isRunning = false;
    public bool staminaDrained;

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
    }

    public void HandleStamina(bool sprintInput, bool isMoving)
    {
        if (sprintInput && isMoving && currentStamina > 0 && !staminaDrained)
        {
            isRunning = true;
            currentStamina -= staminaDrainRate * Time.deltaTime;
            if (currentStamina < 0.1f)
            {
                staminaDrained = true;
                isRunning = false;
            }
        }
        else
        {
            isRunning = false;
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                if (currentStamina>maxStamina/2)
                {
                    staminaDrained = false;
                }
            }
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }
}