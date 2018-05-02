using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailSystem : MonoBehaviour {
    public Transform[] nodes;
	public GameObject target;
    public float movementSpeed, rotationSpeed;
	private float speedScale = 1;
    private int current;
	public bool playOnAwake = true;
	
    void Start() {
		current = 0;
		nodes = GetNodes();

		if (!playOnAwake) this.enabled = false;
    }

  	void Update () {
        if (target.transform.position != nodes[current].position) {
            Vector3 newPosition = Vector3.MoveTowards(
				target.transform.position,
				nodes[current].position,
				movementSpeed * Time.deltaTime * speedScale
			);

			Vector3 newRotation = Vector3.RotateTowards(
				target.transform.forward,
				nodes[current].position - target.transform.position,
				rotationSpeed * Time.deltaTime,
				1.0f
			);

			target.transform.rotation = Quaternion.LookRotation(newRotation);
            target.transform.position = newPosition;
        }
        else {
            current++;

			if (current == nodes.Length) {
				Debug.Log("Rail finished!");
				this.enabled = false;
				return;
			}

			try {
				speedScale = nodes[current].GetComponent<RailNode>().scaleSpeedFactor;
			} catch {
				speedScale = 1;
			}			
        }
	}

	private Transform[] GetNodes() {
		// add one entry as we start at the players position
		Transform[] children = new Transform[transform.childCount + 1];
		children[0] = target.transform;

		for (int i = 0; i < transform.childCount; i++) {
			// insert at i+1 as we are starting at at index 0 for children, but index 1 in the array
			children[i + 1] = transform.GetChild(i);
		}

		return children;
	}	

	void OnTriggerEnter(Collider other) {
		if (!playOnAwake) {
			Debug.Log("RailSystem activated");
			this.enabled = true;
		}
	}
}