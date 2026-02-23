using TMPro;
using UnityEngine;

public class PlayerInterface : MonoBehaviour
{
    public TMP_Text healtText;
    public TMP_Text staminaText;
    PlayerManager playerManager;
    private void Start()
    {
        playerManager = PlayerManager.Instance;
    }

    private void FixedUpdate()
    {
        if (playerManager != null) 
        {
            healtText.text = "Health: " + playerManager.currentHealth.ToString();
            staminaText.text = "Stamina: " + Mathf.Round(playerManager.currentStamina).ToString();
        }
    }
}
