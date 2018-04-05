using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour {
	public int movementSpeed = 3;
	public static float sensitivity = 2f;
	void Update () {
		if (Input.GetKey(KeyCode.W)) {
			transform.position += transform.forward * movementSpeed * Time.deltaTime;
		}	
		if (Input.GetKey(KeyCode.S)) {
			transform.position += transform.forward * -movementSpeed / 2 * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.A)) {
			transform.position += transform.right * -movementSpeed / 2 * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.D)) {
			transform.position += transform.right * movementSpeed / 2 * Time.deltaTime;
		}
		transform.Rotate(0, Input.GetAxis("Horizontal") * Time.deltaTime * sensitivity, 0);
        //transform.Rotate(Input.GetAxis("Vertical") * Time.deltaTime * sensitivity, 0, 0);
	}
}
