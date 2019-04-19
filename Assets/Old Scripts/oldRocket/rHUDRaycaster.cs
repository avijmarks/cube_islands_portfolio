using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rHUDRaycaster : MonoBehaviour {

	public float maxRayDistance = 5;
	public Vector3 padPointerPos;
	public LayerMask controlLayers;
	public Quaternion camRotation;

	//turn ratio calc variables
	public float distance;
	public float turnRatio;
	public Transform pad;
	public GameObject reticle;

	void Update(){
		RaycastHit hit;
		Ray ray = new Ray (transform.position, transform.forward);
		Debug.DrawRay (transform.position, transform.forward * maxRayDistance, Color.magenta);
		if (Physics.Raycast(ray, out hit, maxRayDistance, controlLayers)){
			padPointerPos = hit.point;
			camRotation = transform.rotation;
		}

		//lerp this later (delete when done)
		reticle.transform.position = Vector3.Lerp(reticle.transform.position, padPointerPos, .3f);

		distance = Vector3.Distance (padPointerPos, pad.transform.position);
		Debug.Log (distance);

		turnRatio = distance / .46f;

		//creates deadzone -- approximate values should be from .05 -.1 
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
