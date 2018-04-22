using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SkillCrate : NetworkBehaviour
{

    // User Inputs
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.1f;
    public float frequency = 0.5f;
    public float duration = 15f;
    public float dropSpeed;
    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    // Use this for initialization
    void Start()
    {
        // Store the starting position & rotation of the object
        posOffset = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        duration -= 1 * Time.deltaTime;
        //allow crate to exist for a duration only
        if (duration <= 0)
        {
            NetworkServer.Destroy(this.gameObject); //TODO: check whether there's a diff using network serverhere
            Debug.Log("Crate destroyed after expiring");
        }
        else
        {
            // Spin object around Y-Axis
            transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

            if (posOffset.y <= 1)
            {
                // Float up/down with a Sin()
                tempPos = posOffset;
                tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

                transform.position = tempPos;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - dropSpeed, transform.position.z);
                posOffset = transform.position;
            }
            //}
        }


    }
}
