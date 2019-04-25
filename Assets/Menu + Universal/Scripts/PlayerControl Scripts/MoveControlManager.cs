using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the central decision making 

public enum Position {Deadzone, Top, Bottom}
public enum Activated {Deadzone, Move1, Move2, Stop1, Stop2}

public class MoveControlManager : MonoBehaviour {

	public Transform camTransform;
	public float CamAngle {
		get { 
			return camTransform.eulerAngles.x;
		}
	}

	public float deadZone = 5;

	//Basic pointer position enum variable
	public Position pointerPosition;

	//Activated button enum variable
	public Activated trigger;

	public GameObject cont1;
	public GameObject cont2;

    //times how long vision is in the deadzone in the middle of vision -- used to re-enable controllers on timer so they dont flash on and off when vision is at boundary of DZ
	public float deadTimer = 0;

    //this boolean is change to true if the vision of the user decelerates to a certain point, causing the relevant controller to disable so the user can have full vision
	public bool disableCont = false;

    //controls whether the button in the upper hemisphere of vision is used or not -- probably going to remove button entirely
    public bool enableTopControl = true;


	float tester = 5;

	//variables for deceleration checker -- up here because of invoke
	int bufferCountDec = 0;
	int decFrameCount = 0;
	public bool decelerated = false;

	//reference for control trigger state from playermove script
	public PlayerMove moveScript;

	public IndicatorManager indicator;


	void Update (){
		//only for Debug Purposes
		/*
		Debug.Log(pointerPosition);
		*/
	}
	// Use this for initialization
	void Start () {
		StartCoroutine (PointerManager ());
		
	}

	//-----------------------------------------------------------------------------------------------------------------------------
	//Pointer region/controller state manager -- handles when to disable and enable controllers
	IEnumerator PointerManager (){
		while (true) {



			if (CamAngle > (360 - deadZone) || CamAngle < deadZone) {
				pointerPosition = Position.Deadzone;
				deadTimer += Time.deltaTime;

                //checking if we need top controller enabled
				if (deadTimer > .25f) {
                    if (enableTopControl != false) {
                        cont1.SetActive(true);
                    }
                   
					cont2.SetActive (true);
				}

			} else if (CamAngle > 200 && (360 - CamAngle) > deadZone && enableTopControl != false){
				if (pointerPosition != Position.Top) {
					//only executes first frame in position.top
					pointerPosition = Position.Top;
					StartCoroutine (ControlDeactive ());
				}

                /*
				//Checks for trigger activation enum for click disable
				if (trigger != Activated.Deadzone) {
					disableCont = true;
				}
				*/

                //actually manages controller enable based on bool generated
                if (disableCont == true){
                    cont1.SetActive(false);
                }


                else if (disableCont == false)
                {
                    cont1.SetActive(true);
                }
					
			} else if (CamAngle > deadZone){
				if (pointerPosition != Position.Bottom) {
					//only executes first frame in position.bottom
					pointerPosition = Position.Bottom;
					StartCoroutine (ControlDeactive ());
				}

				//makes sure disabled after click
				if (trigger != Activated.Deadzone) {
					disableCont = true;
				}

				//actually manages controller enable based on bool generated
				if (disableCont == true) {
					cont2.SetActive (false);
				} 

				else if (disableCont == false) {
					cont2.SetActive (true);
				}
			}
				
			yield return null;
		}
	}
	//-----------------------------------------------------------------------------------------------------------------------------

	//this coroutine literally just checks for pointer speed conditions under which we want to have relevant controller disabled
	//returns a boolean used by the pointermanager
	IEnumerator ControlDeactive (){
		//prior to loop -- this coroutine resets all values it uses
		//this is because this coroutine is called only when pointer is outside the deadzone and the controller has not been disabled/clicked
		float positionTimer = 0;

		float frameSpeed = 0;
		float oldAngle = 0;
		float oldSpeed = 0;

		int bufferCountCut = 0;

		disableCont = false;
		deadTimer = 0;
		while (disableCont == false) {
			//condition calculations

			frameSpeed = (CamAngle - oldAngle) / Time.deltaTime;

			//CUTOFF
			if ((frameSpeed * frameSpeed) < 49) {
				bufferCountCut++;
			} else {
				//needed to reset count if not continually occurring
				bufferCountCut = 0;
			}


			//DECELERATION CHECK
			if ((frameSpeed / oldSpeed) < .65 && bufferCountDec == 0) {
				bufferCountDec++;
				decFrameCount++;
				Invoke ("DecChecker", .2f);
			} else if ((frameSpeed / oldSpeed) < .65){
				bufferCountDec++;
				decFrameCount++;
			} else if (bufferCountDec != 0){
				decFrameCount++;
			}



			positionTimer += Time.deltaTime;

			//actual conditions for disableCont bool
			if (bufferCountCut >= 5 || decelerated == true) {
				disableCont = true;
				bufferCountCut = 0;
				//resetting decelerated
				decelerated = false;
			}


			//passing on values for next frame calc
			oldAngle = CamAngle;
			oldSpeed = frameSpeed;

			yield return null;
		}


	}

	//inovoked in ControlDeactive to check frame count -- get rid of later
	void DecChecker () {
		if (bufferCountDec > (decFrameCount/2)) {
			//if half or more the frames during invoke exceed deceleration--disable = true
			decelerated = true;
		}
		bufferCountDec = 0;
		decFrameCount = 0;
	}
	//-----------------------------------------------------------------------------------------------------------------------------


	//these triggers allow for enumerator definition of what most recent button press was if deadzone not returned to (for control movement etc)
	//they are directly ran by eventriggers on controllers
	//also responsible for disablecont = true upon button click
	public void ControlTrigger1 () {
		if (moveScript.move == false) {
			trigger = Activated.Move1;
			moveScript.MovePlayer ();
			indicator.Move1 ();
			disableCont = true;
			StartCoroutine (resetTrigger ());

		} else if (moveScript.move == true) {
			
			trigger = Activated.Stop1;
			moveScript.MovePlayer ();
			indicator.Stop1 ();
			StartCoroutine (resetTrigger ());
		}
	}
	public void ControlTrigger2 () {
		if (moveScript.move == false) {
			trigger = Activated.Move2;
			moveScript.MovePlayer ();
			indicator.Move2 ();
			StartCoroutine (resetTrigger ());
		} else if (moveScript.move == true) {
			trigger = Activated.Stop2;
			moveScript.MovePlayer ();
			indicator.Stop2 ();
			StartCoroutine (resetTrigger ());
		}
	}

	//resets trigger enum after button activation
	IEnumerator resetTrigger(){
		while (trigger != Activated.Deadzone) {
			if (deadTimer != 0f) {
				trigger = Activated.Deadzone;
			}
			if (indicator.stopAttached == true) {
				disableCont = true;
			}
			yield return null;
		}
	}

}
