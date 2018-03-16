using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taichi : MonoBehaviour {
    private float duration;
    private float cooldown;
    // Update is called once per frame
    void Update () {
       
        duration += 1 * Time.deltaTime;
        if (duration >= 2)
            Destroy(this.gameObject);
    }
    // changing the way it works

}

