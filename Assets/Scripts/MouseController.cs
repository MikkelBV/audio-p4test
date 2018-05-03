using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {
	public float sensitivity = 1;
	public GameObject target;

	void OnEnable() {
		Debug.Log("Mouse controller activated");
	}	
	
	void Update () {
		float rotation = Input.GetAxis("Horizontal") * Time.deltaTime * sensitivity;
		target.transform.Rotate(0, rotation, 0);
	}
}
