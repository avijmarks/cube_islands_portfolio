using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMove : MonoBehaviour {

	public padPointer padControl;
	public rHUDRaycaster rayInfo;
	Vector3 rotationAmount;
	public float rotSpeed;
	public float moveSpeed = 55f;
	public bool move = true;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.rotation = Quaternion.RotateTowards (transform.rotation, rayInfo.camRotation, (1 * rayInfo.turnRatio));

		Vector3 freeze = new Vector3 (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
		transform.rotation = Quaternion.Euler (freeze);

		if (move) {
			transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
		}

		/*
		transform.position = transform.position + Camera.main.transform.forward * moveSpeed * Time.deltaTime;
		*/

		
		
	}
}
