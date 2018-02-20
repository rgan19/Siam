using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taichi : MonoBehaviour {
    private float duration;
    // Update is called once per frame
    void Update () {
       
        duration += 1 * Time.deltaTime;
        if (duration >= 2)
            Destroy(this.gameObject);
    }
    //collider currently causes problems. we want a halo like shield.
}

