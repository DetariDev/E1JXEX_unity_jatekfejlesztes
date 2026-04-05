using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private bool isDead = false;
    public int healAmount = 5;

    public float combatDelay = 15f; 
    private float lastDamageTime;

    private void Start()
    {
        lastDamageTime = -combatDelay;
        StartCoroutine(HealRoutine());
    }

    public void TakeDamage(int damage)
    {
        if (PlayerManager.Instance.CurrentHealth <= 0) return;

        PlayerManager.Instance.CurrentHealth -= damage;
        lastDamageTime = Time.time; 

        Debug.Log("Sérültem! Élet: " + PlayerManager.Instance.CurrentHealth);

        if (PlayerManager.Instance.CurrentHealth <= 0)
        {
            PlayerManager.Instance.playerModel.transform.position = MainBase.Instance.transform.position;
            PlayerManager.Instance.CurrentHealth = PlayerManager.Instance.maxHealth;
        }
    }

    public IEnumerator HealRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(1f);

            if (PlayerManager.Instance.CurrentHealth < PlayerManager.Instance.maxHealth && Time.time >= lastDamageTime + combatDelay)
            {
                PlayerManager.Instance.CurrentHealth += healAmount;
                PlayerManager.Instance.CurrentHealth = Mathf.Clamp(PlayerManager.Instance.CurrentHealth, 0, PlayerManager.Instance.maxHealth);
            }
        }
    }
}