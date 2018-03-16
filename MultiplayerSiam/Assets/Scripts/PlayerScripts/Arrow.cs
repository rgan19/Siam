using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    public float speed;
    private float maxDistance;
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
        maxDistance += 1 * Time.deltaTime;
        if (maxDistance >= 5)//set distance travelled by object
            Destroy(this.gameObject);
	}
}
