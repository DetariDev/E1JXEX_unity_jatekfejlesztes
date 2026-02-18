using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public int maxHealth = 100;
    public float baseSpeed = 5f;
    public float maxStamina = 10f;
    public float staminaRegenRate = 5f;
    public float staminaDrainRate = 2f;

    public int currentHealth;
    public float currentSpeed;
    public float currentStamina;

    public bool isRunning = false;
    public bool staminaDrained;
    public bool sprintToggle;

    public DrillHead currentDrillHead;
    [SerializeField] public DrillHead defaultDrillHead;
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
                if (currentStamina>maxStamina/2)
                {
                    staminaDrained = false;
                }
            }
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }
}