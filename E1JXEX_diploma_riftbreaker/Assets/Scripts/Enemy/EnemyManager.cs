using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public List<EnemyBase> enemies;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AlertNearbyEnemies(EnemyBase spotter, GameObject target)
    {
        float rangeSqr = spotter.detectRange * spotter.detectRange;
        Vector3 spotterPos = spotter.transform.position;

        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyBase enemy = enemies[i];
            if (enemy == spotter || enemy.currentState != EnemyState.Idle) continue;

            if ((enemy.transform.position - spotterPos).sqrMagnitude <= rangeSqr)
            {
                enemy.SetTarget(target);
            }
        }
    }
}
