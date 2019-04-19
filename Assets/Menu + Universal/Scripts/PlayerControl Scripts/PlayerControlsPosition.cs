using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsPosition : MonoBehaviour {

	//camera reference
	public GameObject mainCamera;

	void Update () 

	{
		//sets position to same as camera hierarchy
		transform.position = mainCamera.transform.position;

		//getting rotation data for y axis to create a vector3 to base rotation of this hiearchy off of
		float x = 0f;
		float y = mainCamera.transform.rotation.eulerAngles.y;
		float z = 0f;

		Vector3 rotation = new Vector3 (x, y, z);

		transform.rotation = Quaternion.Euler (rotation);

		/* constrained rotation in x and z
		 * child objects will not move up and down no matter direction facing
		 * their rotation is in terms of this parent so..
		 * their rotation in those directions 
		*/

			
		


		
	}
}
