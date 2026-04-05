using System;
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
        playerManager.OnHealthChanged += UpdateHealthBar;
        playerManager.OnStaminaChanged += UpdateStaminaBar;
    }

    private void UpdateStaminaBar(float obj)
    {
        staminaBar.fillAmount = (playerManager.CurrentStamina) / playerManager.maxStamina;
    }

    private void UpdateHealthBar(int obj)
    {
        healthBar.fillAmount = (playerManager.CurrentHealth).ToFloat() / playerManager.maxHealth;
    }

}
