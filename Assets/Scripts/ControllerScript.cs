using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour {
   
    public float distance;
    private bool hasMoved = false;

	void Start () { 
		
	}
	
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!hasMoved)
            {
                transform.Translate(new Vector3(0f, 0f, distance) * Time.deltaTime, Space.Self);
                hasMoved = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.localPosition = new Vector3(0.38f, 0.15f, 2.06f);
            hasMoved = false;
        }

	}
}
