using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPosition : MonoBehaviour {

	public Transform controllerTarget;

	// Update is called once per frame
	void Update () 
	{
		
		//this uses quaternion.lookrotation to set the forward of the button to be angled towards the player at all times 
		Vector3 direction = controllerTarget.localPosition - transform.localPosition;
		Quaternion rotation = Quaternion.LookRotation (direction);
		transform.localRotation = rotation;

		//then modified by 90 on x as the natural forward of cylinder is on y rotation axis

		float x = transform.rotation.eulerAngles.x + 90;
		float y = transform.rotation.eulerAngles.y;
		float z = transform.rotation.eulerAngles.z;

		transform.rotation = Quaternion.Euler (new Vector3 (x, y, z));

	}
}
