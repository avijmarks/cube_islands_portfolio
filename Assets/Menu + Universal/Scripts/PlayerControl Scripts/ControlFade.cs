using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlFade : MonoBehaviour {

	public MoveControlManager moveControls;
	public PlayerMove controlState;
	public Material cont1Mat;
	public Material cont2Mat;
	
	void Start () {
		StartCoroutine (ActiveFade ());
	}
	
	//manages opaqueness of controls based on camera angle
	IEnumerator ActiveFade (){
		float ratio;
		while (true) {
			if (moveControls.pointerPosition == Position.Deadzone) {
				//sets both to low alpha in dead zone 
				Color color2 = cont2Mat.color;
				color2.a = .39f;
				cont2Mat.color = color2;
				Color color1 = cont1Mat.color;
				color1.a = .39f;
				cont1Mat.color = color1;
			} else if (moveControls.pointerPosition == Position.Top) {
				ratio = (33.2f - (360f - moveControls.CamAngle - moveControls.deadZone)) / 33.2f;
				float coloralpha = Mathf.Lerp (1f, .39f, ratio);
				Color color1 = cont1Mat.color;
				color1.a = coloralpha;
				cont1Mat.color = color1;
				//Other Controller
				Color color2 = cont2Mat.color;
				color2.a = .39f;
				cont2Mat.color = color2;
			} else if (moveControls.pointerPosition == Position.Bottom){
				ratio = (33.2f - (moveControls.CamAngle - moveControls.deadZone)) / 33.2f;
				float coloralpha = Mathf.Lerp (1f, .39f, ratio);
				Color color2 = cont2Mat.color;
				color2.a = coloralpha;
				cont2Mat.color = color2;
				//Other Controller
				Color color1 = cont1Mat.color;
				color1.a = .39f;
				cont1Mat.color = color1;
			}

			yield return null;
		}
	}



}
