using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour {
   
    public float distance;


	void Start () { 
		
	}
	
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.UpArrow))
            transform.localPosition = transform.forward * distance;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            transform.localPosition = new Vector3(0f, 0f, 2.22f);

	}
}
