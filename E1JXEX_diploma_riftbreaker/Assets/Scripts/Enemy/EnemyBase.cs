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
    public GameObject target;

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

    void Update()
    {
        if (currentState == EnemyState.Chase && target == null)
        {
            currentState = EnemyState.Idle;
        }

        FindTarget();

        if (currentState == EnemyState.Chase && target != null)
        {
            agent.SetDestination(target.transform.position);
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= attackRange)
            {
                //currentState = EnemyState.Attack;
            }
        }
    }

    public void FindTarget()
    {
        if (currentState == EnemyState.Idle)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectRange);
            GameObject closestTarget = null;
            float closestDistance = Mathf.Infinity;

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player") || hit.CompareTag("Building"))
                {
                    float currentDistance = Vector3.Distance(transform.position, hit.transform.position);
                    if (currentDistance < closestDistance)
                    {
                        closestDistance = currentDistance;
                        closestTarget = hit.gameObject;
                    }
                }
            }

            if (closestTarget != null)
            {
                OnTargetFound(closestTarget);
            }
        }
    }

    public void SetTarget(GameObject foundTarget)
    {
        target = foundTarget;
        currentState = EnemyState.Chase;
        agent.SetDestination(target.transform.position);
    }

    public void OnTargetFound(GameObject target)
    {
        if (currentState != EnemyState.Chase)
        {
            SetTarget(target);
            EnemyManager.instance.AlertNearbyEnemies(this, target);
        }
    }

    IEnumerator RandomPosition()
    {
        while (true)
        {
            if (currentState == EnemyState.Idle)
            {
                Vector3 randomDirection = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
                randomDirection += transform.position;
                NavMeshHit navHit;
                if (NavMesh.SamplePosition(randomDirection, out navHit, 10f, NavMesh.AllAreas))
                {
                    agent.SetDestination(navHit.position);
                }
                yield return new WaitForSeconds(Random.Range(10, 20));
            }
            else
            {
                yield return new WaitForSeconds(5f);
            }
        }
    }
}