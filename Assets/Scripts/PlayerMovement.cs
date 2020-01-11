using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float force;

    private Rigidbody playerRigidBody;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start() {
        playerRigidBody = this.GetComponent<Rigidbody>();
        playerTransform = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (Input.GetKey("w")) {
            playerRigidBody.AddForce(0, 0, force * Time.deltaTime);
        }
        if (Input.GetKey("s")) {
            playerRigidBody.AddForce(0, 0, -force * Time.deltaTime);
        }
        if (Input.GetKey("d")) {
            playerRigidBody.AddForce(force * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("a")) {
            playerRigidBody.AddForce(-force * Time.deltaTime, 0, 0);
        }

        if (playerTransform.position.y < 0) {
            // the player fell off
            playerTransform.position = new Vector3(0, 3, 0); // reset player position
        }
    }
}
