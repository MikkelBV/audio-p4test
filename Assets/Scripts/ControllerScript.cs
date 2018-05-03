using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour {
    public float Distance;
    public Vector3 Normal;
	
	void Update ()  {
        if (Input.GetMouseButtonDown(0)) {
            transform.localPosition = new Vector3(0, 0, Distance);
        } else if (Input.GetMouseButtonDown(1)) {
            transform.localPosition = Normal;
        }
	}
}
