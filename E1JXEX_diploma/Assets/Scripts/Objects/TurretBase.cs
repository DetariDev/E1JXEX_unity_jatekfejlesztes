using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBase : MonoBehaviour
{
    public int powerCost;
    private List<GameObject> targets = new List<GameObject>();
    private Weapon turretWeapon;
    private Shooting shootingComponent;
    private Coroutine shootingCoroutine;
    private Coroutine spendPowerCoroutine;
    private bool hasPower = false;

    private void Awake()
    {
        shootingComponent = GetComponent<Shooting>();
        spendPowerCoroutine = StartCoroutine(SpendPower());
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
        hasPower = ResourceManager.Instance.SpendResource(ResourceType.Wood, powerCost);
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator ShootingTarget()
    {
        if (hasPower)
        {
            shootingComponent.Shoot(gameObject, turretWeapon, FindClosestTarget());
        }
        yield return new WaitForSeconds(turretWeapon.fireRate);
    }

    private Transform FindClosestTarget()
    {
        Transform closestTarget = targets[0].transform;
        float closestDistance = Vector2.Distance(closestTarget.position, gameObject.transform.position);
        foreach (GameObject target in targets)
        {
            float tempdistance = Vector2.Distance(target.transform.position, gameObject.transform.position);
            if (tempdistance<closestDistance)
            {
                closestDistance = tempdistance;
                closestTarget = target.transform;
            }
        }
        return closestTarget;
    }




}
