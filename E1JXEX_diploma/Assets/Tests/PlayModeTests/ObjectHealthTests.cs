using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ObjectHealthTests
{
    private GameObject testObject;
    private ObjectHealth objectHealth;
    [Test]
    public void ObjectHealthTestsSimplePasses()
    {
        testObject = new GameObject();
        objectHealth = testObject.AddComponent<ObjectHealth>();
        objectHealth.maxHealth = 100;
        objectHealth.combatDelay = 15f;
    }

    [UnityTest]
    public IEnumerator TakeDamage_ReducesHealthCorrectly()
    {
        yield return null;
        testObject = new GameObject();
        objectHealth = testObject.AddComponent<ObjectHealth>();
        objectHealth.maxHealth = 100;
        objectHealth.combatDelay = 15f;
        objectHealth.TakeDamage(30);

        yield return null;

        Assert.AreEqual(70, objectHealth.currentHealth);
    }

    [UnityTest]
    public IEnumerator TakeDamage_BelowZero_DestroysObject()
    {
        yield return null;
        testObject = new GameObject();
        objectHealth = testObject.AddComponent<ObjectHealth>();
        objectHealth.maxHealth = 100;
        objectHealth.combatDelay = 15f;
        objectHealth.TakeDamage(999);

        yield return null;

        Assert.IsTrue(testObject == null || !testObject.activeInHierarchy);
    }
}
