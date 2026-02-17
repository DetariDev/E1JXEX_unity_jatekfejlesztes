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
        foreach (EnemyBase enemy in enemies)
        {
            if(Vector3.Distance(enemy.transform.position,spotter.transform.position)< enemy.detectRange * 3)
            {
                enemy.SetTarget(target);
            }
        }
    }
}
