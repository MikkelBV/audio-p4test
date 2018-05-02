using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour {
    public float Distance;
    public Vector3 Normal;
	
	void Update ()  {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            transform.localPosition = transform.forward * Distance;
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            transform.localPosition = Normal;
        }
	}
}
