using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInterface : MonoBehaviour
{
    public Image healthBar;
    public TMP_Text stateText;
    public EnemyBase enemy;

    private void Update()
    {
        if (enemy != null)
        {
            healthBar.fillAmount = (float)enemy.health/ enemy.maxHealth;
            stateText.text = enemy.currentState.ToString();
        }
    }
}
