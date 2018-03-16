using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    //Variables
    public Transform player;
    public float smooth = 0.3f;
    public float height;
    public Vector3 velocity = Vector3.zero;

    //Methods
    private void Update()
    {
        if (player != null)
        {
            Vector3 pos = new Vector3();
            pos.x = player.position.x;
            pos.z = player.position.z - 7f;//dont minus for directly above
            pos.y = player.position.y + height;
            transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smooth);
        }
        else
        {
            //player dead
        }
    }
}
