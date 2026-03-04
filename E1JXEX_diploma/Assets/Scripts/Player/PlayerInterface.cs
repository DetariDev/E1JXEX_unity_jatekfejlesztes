using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector.Libs;

public class PlayerInterface : MonoBehaviour
{
    public Image healthBar;
    public Image staminaBar;
    PlayerManager playerManager;
    private void Start()
    {
        playerManager = PlayerManager.Instance;
    }

    private void FixedUpdate()
    {
        if (playerManager != null) 
        {
            healthBar.fillAmount = (playerManager.currentHealth).ToFloat() / playerManager.maxHealth;
            staminaBar.fillAmount = (playerManager.currentStamina) / playerManager.maxStamina;
        }
    }
}
