using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public void TakeDamage(int damage)
    {
        if (PlayerManager.Instance.currentHealth <= 0) return;

        PlayerManager.Instance.currentHealth -= damage;
        Debug.Log("Sérültem! Élet: " + PlayerManager.Instance.currentHealth);

        if (PlayerManager.Instance.currentHealth <= 0)
        {
            PlayerManager.Instance.currentHealth = 0;
            PlayerManager.Instance.baseSpeed = 0f;
            PlayerManager.Instance.currentSpeed = 0f;
            PlayerManager.Instance.isRunning = false;
        }
    }
}