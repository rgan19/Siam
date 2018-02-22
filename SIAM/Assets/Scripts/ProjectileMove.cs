using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour {

	private float speed = 10.0f;
	private float delay = 8.0f;
	private float strength = 5f;
	public Rigidbody rb;


	// Use this for initialization
	void Start () {



		rb.GetComponent<Rigidbody>();
		//rb.AddForce (0, 1, 1);
		rb.AddForce(rb.transform.up* strength, ForceMode.VelocityChange);

	}

	// Update is called once per frame
	void Update () {
		if (gameObject.transform.position.y > 1)
			Destroy (gameObject);
		Destroy (gameObject, delay);
	}
}
