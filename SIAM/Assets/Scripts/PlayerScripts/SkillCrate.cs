using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCrate : MonoBehaviour {

    // User Inputs
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.25f;
    public float frequency = 1f;
    public float duration = 15f;
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
    void Update()
    {
        duration -= 1 * Time.deltaTime;
        //allow crate to exist for a duration only
        if (duration <= 0)
        {
            Destroy(this.gameObject);
            Debug.Log("Crate destroyed after expiring");
        }
        else
        {

            if (posOffset.y <= 1)
            {
                // Spin object around Y-Axis
                transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

                // Float up/down with a Sin()
                tempPos = posOffset;
                tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

                transform.position = tempPos;
            }
            else
            {
                posOffset = transform.position;
            }
        }
    }


}
