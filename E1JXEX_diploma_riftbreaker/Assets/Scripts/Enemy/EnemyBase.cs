using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Chase,
    Attack
}

public class EnemyBase : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public float speed = 3.5f;
    public float attackRange = 2.0f;
    public float detectRange = 15.0f;
    public EnemyState currentState = EnemyState.Idle;
    NavMeshAgent agent;
    public Transform target;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        EnemyManager.instance.enemies.Add(this);
    }
    private void OnDestroy()
    {
        EnemyManager.instance.enemies.Remove(this);
    }
    void Start()
    {
        StartCoroutine(RandomPosition());
    }


    public void SetTarget(Transform foundTarget)
    {
        target = foundTarget;
        currentState = EnemyState.Chase;
        agent.SetDestination(foundTarget.position);
    }

    public void OnTargetFound(Transform target)
    {
        if (currentState != EnemyState.Chase)
        {
            SetTarget(target);
            EnemyManager.instance.AlertNearbyEnemies(this, target);
        }
    }

    IEnumerator RandomPosition() 
    {
        while (currentState == EnemyState.Idle)
        {
            Vector3 randomDirection = new Vector3(Random.Range(-10f,10f),0,Random.Range(-10f,10f));
            randomDirection += transform.position;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, 10f, NavMesh.AllAreas);
            agent.SetDestination(navHit.position);
            yield return new WaitForSeconds(Random.Range(10,20));
        }
    }
}


