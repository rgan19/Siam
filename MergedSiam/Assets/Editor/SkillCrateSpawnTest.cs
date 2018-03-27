using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SkillCrateSpawnTest {

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator SkillCrateInstantiatationTestWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        var cratePrefab = Resources.Load("Prefab/SkillCrate");
        yield return null;
        var spawnedCrate = GameObject.FindWithTag("SkillDrop");
        var prefabCrate = PrefabUtility.GetPrefabParent(spawnedCrate);
        Assert.AreEqual(cratePrefab,prefabCrate);

    }

  /**  [UnityTest]
 //   public IEnumerator SkillCrateSpawnTestWithEnumeratorPasses()
    {
        var cratePrefab = Resources.Load("Prefab/SkillCrate");
        var crateSpawner = new GameObject().AddComponent < SpawnSkillDrops >();
        crateSpawner.skillCrate =(GameObject) cratePrefab;
        crateSpawner.spawnWait = 1;
        crateSpawner.spawnMostWait = 1;
        crateSpawner.spawnLeastWait = 1;
        crateSpawner.startWait = 1;
        crateSpawner.stop = false;

        yield return new WaitForSeconds(10);
     //   yield return null;

        var spawnedCrates = GameObject.FindGameObjectsWithTag("SkillDrop").Length;
        
        Assert.AreEqual(spawnedCrates, 9);

    }**/
}
