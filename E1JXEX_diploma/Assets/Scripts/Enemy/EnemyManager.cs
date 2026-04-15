using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public List<EnemyBase> enemies = new List<EnemyBase>();
    public List<GameObject> nests = new List<GameObject>();
    public List<GameObject> randomPlaces = new List<GameObject>();
    public float randomprobability = 2;
    public int enemySpawnCount;
    public int maxEnemyCount = 50;
    public int spawntime;
    public int radius = 10;
    private Coroutine spawnCoroutine;


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

    void Start()
    {
        spawnCoroutine = StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            nests.RemoveAll(nest => nest == null);
            randomPlaces.RemoveAll(x => x == null);
            if (enemyPrefabs.Count > maxEnemyCount)
            {
                yield return new WaitForSeconds(spawntime);
            }
            else
            {
                foreach (GameObject baseplace in nests)
                {

                    for (int i = 0; i < enemySpawnCount; i++)
                    {
                        Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Count)], new Vector3(baseplace.transform.position.x + UnityEngine.Random.Range(-radius, radius), 0, baseplace.transform.position.z + UnityEngine.Random.Range(-radius, radius)), Quaternion.identity, null);
                    }
                }
                foreach (GameObject randomplace in randomPlaces)
                {
                    if (UnityEngine.Random.Range(0, 100) < randomprobability)
                    {
                        Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Count)], randomplace.transform.position, Quaternion.identity, null);
                    }

                }
                yield return new WaitForSeconds(spawntime);
            }
            
        }
    }

    public void AlertNearbyEnemies(EnemyBase spotter, GameObject target)
    {
        foreach (EnemyBase enemy in enemies)
        {
            if(Vector3.Distance(enemy.transform.position,spotter.transform.position)< enemy.detectRange * 3 && enemy.currentState==EnemyState.Idle)
            {
                enemy.SetTarget(target);
            }
        }
    }

    public void AttackWall(Vector3 callerPosition, GameObject wall, float radius)
    {
        foreach (EnemyBase enemy in enemies)
        {
            if (Vector3.Distance(enemy.transform.position, callerPosition) < radius)
            {
                if (enemy.Target != wall)
                {
                    enemy.SetTarget(wall);
                }
            }
        }
    }
}
