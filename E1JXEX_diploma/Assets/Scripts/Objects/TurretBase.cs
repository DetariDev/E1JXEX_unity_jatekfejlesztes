using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBase : MonoBehaviour
{
    public int powerCost;
    private List<GameObject> targets = new List<GameObject>();
    public Weapon turretWeapon;
    private Shooting shootingComponent;
    private Coroutine shootingCoroutine;
    private Coroutine spendPowerCoroutine;
    private bool hasPower = false;

    private void Start()
    {
        shootingComponent = GetComponent<Shooting>();
        spendPowerCoroutine = StartCoroutine(SpendPower());
        if (TutorialManager.instance != null && TutorialManager.instance.currentStage == TutorialStage.UseTurret)
        {
            TutorialManager.instance.NextStage();
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !targets.Contains(other.gameObject))
        {
            targets.Add(other.gameObject);
            if (targets.Count>=1 && shootingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(ShootingTarget());
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && !targets.Contains(other.gameObject))
        {
            targets.Add(other.gameObject);
            if (targets.Count >= 1 && shootingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(ShootingTarget());
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && targets.Contains(other.gameObject))
        {
            targets.Remove(other.gameObject);
            if(targets.Count==0 && shootingCoroutine!= null)
            {
                StopCoroutine(shootingCoroutine);
                shootingCoroutine = null;
            }
        }
    }
    private IEnumerator SpendPower()
    {
        while (true)
        {
            hasPower = ResourceManager.Instance.SpendPower(powerCost);
            yield return new WaitForSeconds(10f);
        }
        
    }

    private IEnumerator ShootingTarget()
    {
        while (targets.Count > 0)
        {
            if (hasPower)
            {
                Transform target = FindClosestTarget();
                if (target == null)
                {
                    break;
                }
                shootingComponent.Shoot(gameObject, turretWeapon, FindClosestTarget());
            }
            yield return new WaitForSeconds(turretWeapon.fireRate);
        }
        shootingCoroutine = null;
    }

    private Transform FindClosestTarget()
    {
        targets.RemoveAll(target => target == null);

        if (targets.Count == 0)
        {
            return null;
        }
        Transform closestTarget = targets[0].transform;
        float closestDistance = Vector3.Distance(closestTarget.position, gameObject.transform.position);
        foreach (GameObject target in targets)
        {
            float tempdistance = Vector3.Distance(target.transform.position, gameObject.transform.position);
            if (tempdistance < closestDistance)
            {
                closestDistance = tempdistance;
                closestTarget = target.transform;
            }
        }
        return closestTarget;
    }




}
