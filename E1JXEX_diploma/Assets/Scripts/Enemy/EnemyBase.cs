using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using VInspector;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

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
    private Collider targetCollider;
    public EnemyState currentState = EnemyState.Idle;
    NavMeshAgent agent;
    private GameObject target;
    

    public GameObject Target
    {
        get { return target; }
        set 
        {
            if (target != value)
            {
                target = value;
                OnTargetChanged?.Invoke(target);
            }
            
        }
    }
    public event Action<GameObject> OnTargetChanged;

    private IDamageable targetDamageable;

    private GameObject ClosestWall;

    private float distancee;

    public float Distance
    {
        get { return distancee; }
        set
        {
            distancee = value;
            if (Distance <= attackRange)
            {
                currentState = EnemyState.Attack;
                if (agent.isOnNavMesh) agent.isStopped = true;
                AttackTarget();
            }
            else
            {
                currentState = EnemyState.Chase;
                if (agent.isOnNavMesh) agent.isStopped = false;

                if (agent.isOnNavMesh && agent.isActiveAndEnabled)
                {
                    agent.SetDestination(Target.transform.position);

                    if (!agent.pathPending && agent.pathStatus == NavMeshPathStatus.PathPartial)
                    {
                        if (ClosestWall != null && Target != ClosestWall)
                        {
                            SetTarget(ClosestWall);
                            EnemyManager.instance.AttackWall(transform.position, ClosestWall, 25f);
                        }
                    }
                }
            }

        }
    }

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
    private void OnEnable()
    {
        OnTargetChanged += HandleTargetChanged;
    }


    private void OnDisable()
    {
        OnTargetChanged -= HandleTargetChanged;
    }

    void Start()
    {
        StartCoroutine(RandomPosition());
        StartCoroutine(TryFindTarget());
        StartCoroutine(CalculateDistance());


    }

    private void HandleTargetChanged(GameObject newTarget)
    {
        if (newTarget== null)
        {
            currentState = EnemyState.Idle;
            if (agent.isOnNavMesh) agent.isStopped = false;
            targetDamageable = null;
            targetCollider = null;
        }
        else
        {
            currentState = EnemyState.Chase;
            targetDamageable = newTarget.GetComponent<IDamageable>();
            Collider[] colliders = newTarget.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                if (!col.isTrigger)
                {
                    targetCollider = col;
                    break;
                }
            }

            if (agent.isOnNavMesh && agent.isActiveAndEnabled)
            {
                agent.SetDestination(newTarget.transform.position);
            }
        }
    }
    IEnumerator CalculateDistance()
    {
        while (true)
        {

            if (targetCollider != null)
            {
                Distance = Vector3.Distance(transform.position, targetCollider.ClosestPointOnBounds(transform.position));
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRange, Physics.AllLayers, QueryTriggerInteraction.Ignore);
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

        if (closestTarget != null && closestTarget != Target)
        {
            OnTargetFound(closestTarget);
        }
    }

    public void SetTarget(GameObject foundTarget)
    {
        Target = foundTarget;
    }

    public void OnTargetFound(GameObject foundtarget)
    {
        SetTarget(foundtarget);
        EnemyManager.instance.AlertNearbyEnemies(this, foundtarget);
    }

    private IEnumerator TryFindTarget()
    {
        while (true)
        {
            if (Target != null && !Target)
            {
                Target = null;
            }
            FindTarget();
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator RandomPosition()
    {
        while (true)
        {
            if (currentState == EnemyState.Idle)
            {
                Vector3 randomDirection = new Vector3(UnityEngine.Random.Range(-10f, 10f), 0, UnityEngine.Random.Range(-10f, 10f));
                randomDirection += transform.position;
                NavMeshHit navHit;
                if (NavMesh.SamplePosition(randomDirection, out navHit, 10f, NavMesh.AllAreas))
                {
                    agent.SetDestination(navHit.position);
                }
                yield return new WaitForSeconds(UnityEngine.Random.Range(10, 20));
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

        if(other.CompareTag("Wall"))
        {
            ClosestWall = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            if (other.gameObject == ClosestWall)
            {
                ClosestWall = null;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health-= damage;
        if (health<=0)
        {
            ResourceManager.Instance.AddResource(ResourceType.Biomass, 10);
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
            else if (Target != null)
            {
                targetDamageable = Target.GetComponent<IDamageable>();
                if (targetDamageable == null) currentState = EnemyState.Idle;
            }
        }
    }
}