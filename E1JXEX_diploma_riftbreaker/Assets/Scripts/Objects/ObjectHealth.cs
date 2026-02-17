using UnityEngine;

public class ObjectHealth : MonoBehaviour, IDamageable
{
    int currentHealth = 0;
    public int maxHealth = 100;
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }
}