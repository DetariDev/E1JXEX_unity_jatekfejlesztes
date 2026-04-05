using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInterface : MonoBehaviour
{
    public Image healthBar;
    public Image staminaBar;
    PlayerManager playerManager;
    private void Start()
    {
        playerManager = PlayerManager.Instance;
        playerManager.OnHealthChanged += UpdateHealthBar;
        playerManager.OnStaminaChanged += UpdateStaminaBar;
    }

    private void UpdateStaminaBar(float obj)
    {
        staminaBar.fillAmount = (playerManager.CurrentStamina) / playerManager.maxStamina;
    }

    private void UpdateHealthBar(int obj)
    {
        healthBar.fillAmount = (float)playerManager.CurrentHealth / playerManager.maxHealth;
    }

}
