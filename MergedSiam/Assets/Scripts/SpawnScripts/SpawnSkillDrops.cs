using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnSkillDrops : NetworkBehaviour {
    public GameObject skillCrate;
    private Vector3 boundaries;
    public float spawnWait, spawnMostWait, spawnLeastWait;
    public int startWait;
    public bool stop;
	// Use this for initialization
	void Start () {
        StartCoroutine(waitSpawner());
        boundaries = GetComponent<Collider>().bounds.size;
	}
	
	// Update is called once per frame
	void Update () {
		spawnWait = Random.Range(spawnLeastWait, spawnMostWait);
	}


    IEnumerator waitSpawner()
    {
		
        yield return new WaitForSeconds(startWait);
        while (!stop)
        {
			if (!isServer) {
				yield break;
			}
			Vector3 spawnPos = new Vector3(Random.Range(-boundaries.x / 2, boundaries.x / 2), transform.position.y, Random.Range(-boundaries.z / 2, boundaries.z / 2));
			var spawnedCrate = Instantiate(skillCrate, spawnPos + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);
			Debug.Log("Spawned a crate at "+spawnPos.ToString()+" after waiting for "+spawnWait+" seconds.");
			NetworkServer.Spawn (spawnedCrate);
			Debug.Log ("Network Server spawn crate");
			yield return new WaitForSeconds(spawnWait);
            
         }
    }
}
