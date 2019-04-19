using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestControllerDisable : MonoBehaviour {

	//general variables
	public float camPos;
	public GameObject cont1;
	public GameObject cont2;

	//general timers for vision regions and deadzone size control
	float hemiTimer1;
	float hemiTimer2;
	public float deadTimer;
	public float deadZone = 5f;

	//variables for calculating camera speed per frame
	float oldAngle;
	float frameSpeed;
	float oldSpeed;

	//variables for if camera speed vertically per frame is beneath certain amount disables controllers
	public int bufferCountCut;

	//variables used in determining whether the camera movement has decelerated to the point controllers should be disabled
	int bufferCountDec;
	int decFrameCount;
	bool decelerated;

	//enumerator for what region of vision a controller enable/disable occurred in
	enum Position {Deadzone, Top, Bottom};
	Position myPosition;



	void Update ()
	{
		camPos = transform.rotation.eulerAngles.x;

		bool disableCont = false;
			//BOOL TRIGGER FOR DISABLE: false by default; must be set to true within frame to keep controller active

		frameSpeed = (camPos - oldAngle) / Time.deltaTime	;
		//provides DisableCont boolean as true provided there has been speed decrease in [bufferamount] preceding frames

		//CUTOFF buffer count control
		if ((frameSpeed * frameSpeed) < 49) {
			bufferCountCut++;
		} else {
			//needed to reset count if not continually occurring
			bufferCountCut = 0;
		}
		//DECELERATION buffer count control -- count reset in invoke
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
			
		//USES PREVIOUS TWO CONDITIONALS TO SEE IF THE CAMERA SHOULD BE ENABLED
		if (bufferCountCut >= 5 || decelerated == true) {
			disableCont = true;
			bufferCountCut = 0;
			//resetting decelerated
			decelerated = false;
		}
			

		//DEADZONE
		if (camPos > (360 - deadZone) || camPos < deadZone) {
			bufferCountCut = 0;
			myPosition = Position.Deadzone;
			hemiTimer1 = 0f;
			hemiTimer2 = 0f;
			deadTimer += Time.deltaTime;
			if (deadTimer > .2f) {
				cont1.SetActive (true);
				cont2.SetActive (true);
				disableCont = false;
			}
		}
		//CONT1 ENABLE/DISABLE
		else if (camPos > 200 && (360 - camPos) > deadZone) {
			hemiTimer1 += Time.deltaTime;
			deadTimer = 0f;
			cont2.SetActive (true);

			if (hemiTimer1 < 2 && disableCont == false && myPosition != Position.Top) {
				//keeps button active given time in hemisphere, lookSpeed cutoff, and lack of deceleration
				cont1.SetActive (true);
			} else {
				myPosition = Position.Top;
				cont1.SetActive (false);
			}
		}
		//CONT2 ENABLE/DISABLE
		else if (camPos > deadZone) {
			hemiTimer2 += Time.deltaTime;
			deadTimer = 0f;
			cont1.SetActive (true);
			if (hemiTimer2 < 2 && disableCont == false && myPosition != Position.Bottom) {
				//keeps button active given time in hemisphere, lookSpeed cutoff, and lack of deceleration
				cont2.SetActive (true);
			} else {
				myPosition = Position.Bottom;
				cont2.SetActive (false);
			}

		}

		//passing along data to "old" camera position and frame to frame speed variables for comparison in next frame
		oldAngle = camPos;
		oldSpeed = frameSpeed;
	}


	void DecChecker () {
		if (bufferCountDec > (decFrameCount/2)) {
			//if half or more the frames during invoke exceed deceleration--disable = true
			decelerated = true;
		}
		bufferCountDec = 0;
		decFrameCount = 0;
	}
}
