using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taichi : MonoBehaviour {
	public float duration;
	private Collider shield_collider;
	// Update is called once per frame
	private void Start()
	{
		//set ignore collision with player?

		shield_collider = GetComponent<Collider>();
		GameObject[] allArrows = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < allArrows.Length; i++)
		{
			Physics.IgnoreCollision(allArrows[i].GetComponent<Collider>(), shield_collider);
		}
	}
	void Update () {

		duration -= 1 * Time.deltaTime;

		if (duration <= 0)
		{
			Destroy(this.gameObject);
		}

	}
}

