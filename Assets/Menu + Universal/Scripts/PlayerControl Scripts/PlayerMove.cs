using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	public float moveSpeed = 10f;
	public bool move = false;
	public MoveControlManager moveControls = new MoveControlManager();


	float accelerate = 0;
	float decelerate = 0;

	// Update is called once per frame
	void Update () 
	{
		if (move == true && moveControls.indicator.moveAttached == true) {
			transform.position = transform.position + Camera.main.transform.forward * moveSpeed * Time.deltaTime;
		} else if (move == false && moveControls.indicator.stopAttached == false){
			transform.position = transform.position + Camera.main.transform.forward * moveSpeed * Time.deltaTime;
		}
	}
	public void MovePlayer ()
	{
		move = !move;
	}
		

}
