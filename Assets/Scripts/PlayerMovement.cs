using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public Rigidbody player;
    public float force;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void FixedUpdate() {
        if (Input.GetKey("w")) {
            player.AddForce(0, 0, force * Time.deltaTime);
        }
        if (Input.GetKey("s")) {
            player.AddForce(0, 0, -force * Time.deltaTime);
        }
        if (Input.GetKey("d")) {
            player.AddForce(force * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("a")) {
            player.AddForce(-force * Time.deltaTime, 0, 0);
        }
    }
}
