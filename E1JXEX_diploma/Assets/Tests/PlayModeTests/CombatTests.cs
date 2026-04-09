using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CombatTests
{
    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("CombatTestScene");
        yield return null;
    }

    [UnityTest]
    public IEnumerator EnemyWanders_DestroysWall_ThenKilledByTurret()
    {
        GameObject enemy = GameObject.Find("EnemyPrefab");
        GameObject wall = GameObject.Find("StoneWall");
        GameObject turret = GameObject.Find("SmallTurret");
        Assert.IsNotNull(enemy, "Nincs ellenség a pályán");
        Assert.IsNotNull(wall, "nincs fal a pályán");
        Assert.IsNotNull(turret, "nincs lõtorony a pályán");

        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.power = 1000;
        }
        wall.GetComponent<ObjectHealth>().currentHealth = 1;
        float timeout = 15f;
        while (wall != null && timeout > 0)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }
        Assert.IsTrue(wall == null, "Hiba: Az ellenség nem találta meg, vagy nem tudta elpusztítani a falat 15 másodperc alatt!");
        timeout = 10f;
        while (enemy != null && timeout > 0)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }
        Assert.IsTrue(enemy == null, "Hiba: A lõtorony nem tudta megölni az ellenséget 10 másodperc alatt!");
    }
}
