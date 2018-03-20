using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    public float speed;

    // Update is called once per frame
    void Update()
    {
        // arrow flies forever now
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }
   /*WIP
    * // Setting arrow bounce
        Ray ray = new Ray(transform.position, transform.right);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Time.deltaTime * speed + .1f))
        {
            Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
            float rot = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, rot, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }*/
}
