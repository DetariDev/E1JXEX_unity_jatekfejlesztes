using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

public class Systemtests
{
    [UnityTest]
    public IEnumerator ResourceManager_PowerDoesNotExceedMaxPower()
    {
        GameObject resourceObject = new GameObject("ResourceManager");
        ResourceManager rm = resourceObject.AddComponent<ResourceManager>();
        rm.maxPower = 100;
        yield return null;
        rm.AddPower(500);
        Assert.AreEqual(100, rm.power, "Hibás logika: Az áram túllépte a maximális értéket!");
        Object.DestroyImmediate(resourceObject);
    }
    [UnityTest]
    public IEnumerator ResourceManager_HasEnoughResource()
    {
        GameObject resourceObject = new GameObject("ResourceManager");
        ResourceManager rm = resourceObject.AddComponent<ResourceManager>();
        yield return null;
        Assert.IsFalse(rm.HasEnoughResource(ResourceType.Wood, 100), "Hibás logika: Nem volt elég fa, de a rendszer mégis azt jelezte, hogy van!");
        rm.AddResource(ResourceType.Wood, 150);
        yield return null;
        Assert.IsTrue(rm.HasEnoughResource(ResourceType.Wood, 100), "Hibás logika: Bolt elegendõ fa, de azt jelezte, hogy nincs!");
        Assert.IsTrue(rm.SpendResource(ResourceType.Wood, 100), "Hibás logika: Volt elég fa, de a rendszer nem engedte meg a költést!");
        Assert.IsFalse(rm.HasEnoughResource(ResourceType.Wood, 100), "Hibás logika: Nem volt elég fa a költés után, de a rendszer mégis azt jelezte, hogy van!");
        yield return null;
        Object.DestroyImmediate(resourceObject);
    }

    [UnityTest]
    public IEnumerator ResourceManager_SpendResource()
    {
        GameObject resourceObject = new GameObject("ResourceManager");
        ResourceManager rm = resourceObject.AddComponent<ResourceManager>();
        yield return null;
        rm.AddResource(ResourceType.Wood, 150);
        yield return null;
        Assert.IsTrue(rm.SpendResource(ResourceType.Wood, 100), "Hibás logika: Volt elég fa, de a rendszer nem engedte meg a költést!");
        Assert.IsFalse(rm.SpendResource(ResourceType.Wood, 100), "Hibás logika: Nem volt elég fa a költés után, de a rendszer mégis engedte!");
        yield return null;
        Object.DestroyImmediate(resourceObject);
    }

    [UnityTest]
    public IEnumerator EnemyManager_CorrectlyAddsAndRemovesEnemies()
    {
        GameObject enemyManagerObject = new GameObject("EnemyManager");
        EnemyManager enemyManager = enemyManagerObject.AddComponent<EnemyManager>();
        yield return null;
        GameObject enemyObj = new GameObject("TestEnemy");
        enemyObj.AddComponent<NavMeshAgent>();
        EnemyBase enemy = enemyObj.AddComponent<EnemyBase>();
        yield return null;
        Assert.IsTrue(enemyManager.enemies.Contains(enemy), "Hiba: Az új ellenség nem adta hozzá magát az EnemyManager listájához!");
        Object.Destroy(enemyObj);
        yield return null;
        Assert.IsFalse(enemyManager.enemies.Contains(enemy), "Hiba: A halott ellenség benne maradt a menedzser listájában (memóriaszivárgás)!");
        Object.DestroyImmediate(enemyManagerObject);
    }
}
