using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

	private float speed;
	public GameObject projectile;
	public Transform projectileTrans;
	private float nextFire = 0.0f;
	private float fireRate;
	private float time = 0.0f;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		fireRate = 3.0f;
		speed = 30.0f;
	}
	
	// Update is called once per frame
	void Update () {
		//time from start
		time += Time.deltaTime;
		Debug.Log (time);

		if (time > 5.0f) {
			speed = 60.0f;
			fireRate = 1.5f;
		}

		//boss rotation
		transform.Rotate (Vector3.forward * Time.deltaTime * speed);

		//boss projectile
		if (Time.time > nextFire) {
			nextFire = Time.time + fireRate;

			GameObject obj = (GameObject)Instantiate (projectile, projectileTrans.position, projectileTrans.rotation);// as GameObject;

		}




	}

}
