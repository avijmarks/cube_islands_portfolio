using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class padPointer : MonoBehaviour {

	//getting raycast point from raycast script on cam
	public rHUDRaycaster rayPoint;

	public float distance;
	public float turnRatio;

	//Lerped pointerposition, pointer object
	Vector2 pointerPosition;
	public GameObject pointer;


	//max distance of pointer is about .51 
	// Update is called once per frame
	void Update () {
		pointerPosition = new Vector2(rayPoint.padPointerPos.x - transform.position.x, rayPoint.padPointerPos.y - transform.position.y);
		distance = Vector2.Distance (pointerPosition, new Vector2(0f, 0f));
		Debug.Log (distance);
		turnRatio = distance / .46f;

		//creates deadzone -- approximate values should be from .05 -.1 stopped, .5-1 moving (speed dependent) (fix later)
		if (distance < .05f) {
			turnRatio = 0f;
		}

		if (turnRatio > 1f) {
			turnRatio = 1f;
		}

		//the exponent here controls how quickly turnspeed reaches max turnspeed on control pad -- higher = slower -- approx 1-2
		turnRatio = Mathf.Pow (turnRatio, 1);

	}
}
