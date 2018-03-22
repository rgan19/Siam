using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    public float initSpeed;
    public float maxVelocity;
    private Rigidbody rb;
    private Collider arrow_collider;
    private Vector3 oldVel;
    private float multiplier1, multiplier2;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        arrow_collider = GetComponent<Collider>();
        GameObject[] allArrows = GameObject.FindGameObjectsWithTag("Arrow");
        for(int i = 0; i< allArrows.Length; i++)
        {
            Physics.IgnoreCollision(allArrows[i].GetComponent<Collider>(), arrow_collider);
        }        
        // arrow flies forever now
        Vector3 direction = transform.forward;
        direction.y = 0f;
        rb.AddForce(direction*initSpeed, ForceMode.VelocityChange);
        multiplier1 = 1.1f;
        multiplier2 = 1.2f;
        
    }
    // Update is called once per frame
    void Update()
    {
        oldVel = rb.velocity;
        // set max velocity arrow can travel at
        if(oldVel.magnitude >= maxVelocity)
        {
            multiplier2 = 1f;
            multiplier1 = 1f;
        }
        else
        {
            multiplier1 = 1.1f;
            multiplier2 = 1.2f;
        }
        // set arrow to look at direction it is going
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        // Make arrows bounce on collision with environment or taichi sphere. 
        ContactPoint cp = collision.contacts[0];
        
        if (collision.gameObject.CompareTag("TaichiSphere"))
        {
            // causes the arrow to speed up with multiplier when reflected by taichi sphere
            rb.velocity = Vector3.Reflect(oldVel * multiplier2, cp.normal);
        }
        else
        {
            // causes the arrow to speed up with multiplier when reflected by wall
            rb.velocity = Vector3.Reflect(oldVel * multiplier1, cp.normal);
        }
   
    }
}
