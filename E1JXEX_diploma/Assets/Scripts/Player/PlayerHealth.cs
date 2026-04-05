using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public void TakeDamage(int damage)
    {
        if (PlayerManager.Instance.CurrentHealth <= 0) return;

        PlayerManager.Instance.CurrentHealth -= damage;
        Debug.Log("Sérültem! Élet: " + PlayerManager.Instance.CurrentHealth);

        if (PlayerManager.Instance.CurrentHealth <= 0)
        {
            PlayerManager.Instance.CurrentHealth = 0;
            PlayerManager.Instance.baseSpeed = 0f;
            PlayerManager.Instance.currentSpeed = 0f;
            PlayerManager.Instance.isRunning = false;
        }
    }
}