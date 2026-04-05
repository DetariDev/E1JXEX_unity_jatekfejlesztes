using System;
using System.Collections;
using UnityEngine;

public class ObjectHealth : MonoBehaviour, IDamageable
{
    public bool isBase;
    public int currentHealth = 0;
    public int maxHealth = 100;
    private bool isDead = false;
    public int amount = 10;

    public float combatDelay = 15f; 
    private float lastDamageTime;

    public event Action OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        lastDamageTime = -combatDelay;
    }

    private void Start()
    {
        StartCoroutine(HealRoutine());
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        lastDamageTime = Time.time;

        OnHealthChanged?.Invoke(); 

        if (currentHealth <= 0 && !isBase)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }


    public IEnumerator HealRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(1f); 

            if (currentHealth < maxHealth && Time.time >= lastDamageTime + combatDelay)
            {
                currentHealth += amount;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
                OnHealthChanged?.Invoke(); 
            }
        }
    }
}