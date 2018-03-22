using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taichi : MonoBehaviour {
    public float duration;
    // Update is called once per frame

    void Update () {

        duration -= 1 * Time.deltaTime;

        if (duration <= 0)
        {
            Destroy(this.gameObject);
        }
            
    }
}

