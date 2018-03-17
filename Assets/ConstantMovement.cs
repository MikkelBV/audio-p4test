using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMovement : MonoBehaviour {
	public Vector3 movement = new Vector3(0.2f, 0, 0);

	void Update () {
		transform.position += movement * Time.deltaTime;
	}
}
