using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JoyStickPosition {Active, Inactive}

public class RocketJoystick : MonoBehaviour {

	/// <gets values from joystick>
	/// provides info to actual move (fly) mechanism
	/// uses raycasting to establish values which are then established between 0-1 as though virtual x y axis
	/// send xy values and joystick state to fly script
	/// </summary>

	//for raycast
	public Transform mainCam;
	float maxRayDist = 5f;
	public LayerMask rayLayerHit;

	//for reticle positioning
	public Vector3 reticlePos;
	public GameObject reticle;

	//Joystick pad position
	public Vector3 padPos;

	public Vector2 joystickXY;

	//check if collider is hitting deadzone (remove if not using middle snap)
	public Collider deadzone;


	public GameObject hierarchy;
	public float rotationSpeed = 1000f;
	public float rollFactor = .3f;
	//the exponent here controls how quickly turnspeed reaches max turnspeed on control pad -- higher = slower
	public float rotExpon = 1;

	public GameObject indRing;

	public bool move = true;
	public float moveSpeed = 55;



	public JoyStickPosition myPosition = JoyStickPosition.Inactive;

	void Start () {
		
		StartCoroutine (JoystickManager ());
	}
	void Update (){
		
	}

	IEnumerator JoystickManager (){
		while (true) {
			//makes ya move bish
			if (move) {
				hierarchy.transform.position = hierarchy.transform.position + hierarchy.transform.forward * moveSpeed * Time.deltaTime;
			}


			//pad position for calculating variables
			padPos = transform.TransformPoint ((transform.localPosition + new Vector3 (0f, 0f, 1.5f)));

			//boolean for whether camera is on the joystick at all or not, defaulted to false every frame/loop
			bool camOnPad = false;

			//draws ray every frame
			RaycastHit hit;
			Vector3 hitCoords = padPos;
			float hitDistance;

			Ray ray = new Ray (mainCam.transform.position, mainCam.transform.forward);
			Debug.DrawRay (mainCam.transform.position, mainCam.transform.forward * maxRayDist, Color.magenta);

			//Hitcoords is set to raycast position only if raycast is on pad and not in deadzone
			if (Physics.Raycast (ray, out hit, maxRayDist, rayLayerHit)) {
				Collider colliderHit = hit.collider;
				if (colliderHit != deadzone) {
					camOnPad = true;
					hitCoords = hit.point;
				}
			} else {
				camOnPad = false;
			}


			//apply reticle position to HitCoords position via lerp sauce (make framerate sensitive function later and delete this)
			reticle.transform.position = Vector3.Lerp(reticle.transform.position, hitCoords, .3f);

			//getting XY and restting if reticle is centered or close to it -- turning/movement is controlled by reticle position not camera position
			joystickXY = new Vector2 (reticle.transform.localPosition.x, reticle.transform.localPosition.y);

			//Making shit turn
			//
			//

			if (camOnPad == true) {
				float horizontalSpeed = Mathf.Pow((joystickXY.x / .46f) * (rotationSpeed * Time.deltaTime), rotExpon);
				float verticalSpeed = - Mathf.Pow((joystickXY.y / .46f) * (rotationSpeed * Time.deltaTime), rotExpon);



			

				/*
				Quaternion rotatorZ = Quaternion.AngleAxis (-horizontalSpeed, hierarchy.transform.forward);
				hierarchy.transform.rotation = rotatorZ * hierarchy.transform.rotation;
				*/

				Quaternion rotatorY = Quaternion.AngleAxis (verticalSpeed, hierarchy.transform.right);
				hierarchy.transform.rotation = rotatorY * hierarchy.transform.rotation;

				Quaternion rotatorX = Quaternion.AngleAxis (horizontalSpeed, hierarchy.transform.up);
				hierarchy.transform.rotation = rotatorX * hierarchy.transform.rotation;

				float zRot = -joystickXY.x * 135f;
				Vector3 rotatorZ = new Vector3 (hierarchy.transform.rotation.eulerAngles.x, hierarchy.transform.rotation.eulerAngles.y, zRot);
				hierarchy.transform.rotation = Quaternion.Euler (rotatorZ);

				Vector3 indRotator = new Vector3 (zRot + 45f, 90f, 90f);
				indRing.transform.localRotation = Quaternion.Euler (indRotator);
				/*
				float zRot = -joystickXY.x * 135f;
				Vector3 rotatorZ = new Vector3 (hierarchy.transform.rotation.eulerAngles.x, hierarchy.transform.rotation.eulerAngles.y, zRot);
				hierarchy.transform.rotation = Quaternion.Euler (rotatorZ);

				/*
				Vector3 rotNoZ = new Vector3 (hierarchy.transform.rotation.eulerAngles.x, hierarchy.transform.rotation.eulerAngles.y, 0);
				Quaternion rotatorZ = Quaternion.Euler (rotNoZ);

				rotatorZ = Quaternion.AngleAxis (-horizontalSpeed, hierarchy.transform.rotation);
				hierarchy.transform.rotation = rotatorZ * hierarchy.transform.rotation;
				/////
				Vector3 rightNoY = Vector3.Cross(transform.up, transform.forward);
				rightNoY.y = 0f;
				Quaternion rotatorZ = Quaternion.Euler (rightNoY);
				rotatorZ = Quaternion.AngleAxis (-horizontalSpeed, hierarchy.transform.forward) * rotatorZ;

				hierarchy.transform.rotation = rotatorZ * hierarchy.transform.rotation;
				////
				Vector3 rotNoZ = new Vector3 (hierarchy.transform.rotation.eulerAngles.x, hierarchy.transform.rotation.eulerAngles.y, 0);
				hierarchy.transform.rotation = Quaternion.Euler (rotNoZ);
				*/

			} else {
				//reset roll
				// Calculate rotation
				Vector3 rightNoY = Vector3.Cross(Vector3.up, transform.forward);
				rightNoY.y = 0f;
				Quaternion rotator = Quaternion.FromToRotation(transform.right, rightNoY);

				// Apply rotation
				hierarchy.transform.rotation = Quaternion.Slerp(hierarchy.transform.rotation, rotator * hierarchy.transform.rotation, 25 * Time.deltaTime);
			}



			yield return null;
		}
	}
}
