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
    public float attackRate = 1f;
    private float nextAttackTime = 0f;
    public int damage = 10;
    public float detectRange = 15.0f;
    public EnemyState currentState = EnemyState.Idle;
    NavMeshAgent agent;
    public GameObject target;
    private IDamageable targetDamageable;
    public float findTargetInterval = 0.5f;
    private float findTargetTimer = 0f;

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
        findTargetTimer += Time.deltaTime;
        if (findTargetTimer >= findTargetInterval)
        {
            FindTarget();
            findTargetTimer = 0f;
        }

        if (target == null)
        {
            currentState = EnemyState.Idle;
            return;
        }
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance <= attackRange)
        {
            currentState = EnemyState.Attack;
            agent.isStopped = true;
            AttackTarget();
        }
        else
        {
            currentState = EnemyState.Chase;
            agent.isStopped = false;

            if (agent.isOnNavMesh && agent.isActiveAndEnabled)
            {
                agent.SetDestination(target.transform.position);
            }
        }
    }

    public void TryAttackTarget()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= attackRange)
        {
            agent.isStopped = true;
            AttackTarget();
        }
        else { currentState = EnemyState.Chase; }
    }

    public void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRange);
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player") || hit.CompareTag("Building"))
            {
                
                float currentDistance = Vector3.Distance(transform.position, hit.transform.position);
                if (hit.CompareTag("Player"))
                {
                    currentDistance *= 0.5f;
                }
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestTarget = hit.gameObject;
                }
            }
        }

        if (closestTarget != null && closestTarget != target)
        {
            OnTargetFound(closestTarget);
        }
    }

    public void SetTarget(GameObject foundTarget)
    {
        target = foundTarget;
        targetDamageable = target.GetComponent<IDamageable>();
        currentState = EnemyState.Chase;

        if (agent.isOnNavMesh && agent.isActiveAndEnabled)
        {
            agent.SetDestination(target.transform.position);
        }
    }

    public void OnTargetFound(GameObject target)
    {
        SetTarget(target);
        EnemyManager.instance.AlertNearbyEnemies(this, target);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Projectile bullet = other.GetComponent<Projectile>();
            TakeDamage(bullet.damage);
            Destroy(other.gameObject);
            OnTargetFound(bullet.owner);
        }
    }

    public void TakeDamage(int damage)
    {
        health-= damage;
        if (health<=0)
        {
            Destroy(gameObject);
        }
    }
    public void AttackTarget()
    {
        currentState = EnemyState.Attack;
        if (Time.time >= nextAttackTime)
        {
            if (targetDamageable != null)
            {
                targetDamageable.TakeDamage(damage);
                nextAttackTime = Time.time + 1f / attackRate;
            }
            else if (target != null)
            {
                targetDamageable = target.GetComponent<IDamageable>();
                if (targetDamageable == null) currentState = EnemyState.Idle;
            }
        }
        

    }
}