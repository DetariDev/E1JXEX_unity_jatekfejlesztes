using System.Collections;
using System.Resources;
using System.Security.AccessControl;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyCombatIntegrationTests
{
    private GameObject enemy;
    private GameObject wall;
    private GameObject turret;

    [SetUp]
    public void Setup()
    {
        
    }

    [TearDown]
    public void Teardown()
    {
        // A teszt végén eltakarítunk magunk után, hogy a következõ teszt tiszta lappal induljon
        if (enemy != null) Object.DestroyImmediate(enemy);
        if (wall != null) Object.DestroyImmediate(wall);
        if (turret != null) Object.DestroyImmediate(turret);
    }

    [UnityTest]
    public IEnumerator EnemyAttacksWall_ThenTurretKillsEnemy()
    {
        
    }
}