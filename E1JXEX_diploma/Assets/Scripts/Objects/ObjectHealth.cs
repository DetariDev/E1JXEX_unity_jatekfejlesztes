using UnityEngine;

public class ObjectHealth : MonoBehaviour, IDamageable
{
    public int currentHealth = 0;
    public int maxHealth = 100;
    private bool isDead = false;
    public int healTime = 1;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        healTime = 10;
        if (currentHealth <= 0)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth += amount;
        healTime = 1;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}