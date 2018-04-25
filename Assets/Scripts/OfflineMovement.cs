using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineMovement : MonoBehaviour {

    Rigidbody o_RigidBody;
    public float distance = 100.0f;

	void Start () {
        o_RigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.UpArrow)){
            o_RigidBody.velocity = transform.up * distance;
            Debug.Log("FORWARD");
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            o_RigidBody.velocity = -transform.forward * distance;
            Debug.Log("BACK");
        }
        
	}
}

