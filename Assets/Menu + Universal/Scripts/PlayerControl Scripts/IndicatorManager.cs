using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour {

	public Transform pointer;
	public float smoothTime = .5f;
	Vector3 velocity = Vector3.zero;
	public Transform cont1;
	public Transform cont2;

	bool moving = false;
	//used to store local world space positions of controllers (under another parent for rotation reasons)
	Vector3 cont1Pos;
	Transform cont2Pos;

	Vector3 contScale = new Vector3 (.28f, .32f, .28f);
	Vector3 pointerScale = new Vector3 (.12f, .15f, .12f);
	float cutoff = .00001f;

	public MoveControlManager moveControls;
	public PlayerMove playerMoveScript;
	public Material indMat;
	public bool stopAttached;
	public bool moveAttached;

	Vector3 stopContPosition;
	int stopControl;
	Vector3 stopPosition;

	//useless variables??
	public GameObject disablecont1;
	public GameObject disablecont2;
	enum Position{Move1, Stop1, Move2, Stop2, Attached};
	Position indPosition = Position.Attached;
	public Transform parent;

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);	
		stopAttached = true;
	}

	//-----------------------------------------------------------------------------------------------------------------------------

	//FUNCTIONS CALLED BY EVENT SECTION OF MOVECONTROLMANAGER
	public void Move1(){
		
		//so far sets position of ind to relevant controller, makes ind active, and makes ind have cont scale
		gameObject.SetActive (true);
		transform.position = cont1.transform.position;
		transform.localScale = contScale;
		StartCoroutine (moveSnap ());
	}

	public void Move2(){
		//so far sets position of ind to relevant controller, makes ind active, and makes ind have cont scale
		gameObject.SetActive (true);
		transform.position = cont2.transform.position;
		transform.localScale = contScale;
		StartCoroutine (moveSnap ());
	}

	public void Stop1(){
		stopPosition = pointer.transform.position;
		transform.position = stopPosition;
		stopControl = 1;
		stopContPosition = cont1.transform.position;
		StartCoroutine (stopSnap ());
	}

	public void Stop2(){
		stopPosition = pointer.transform.position;
		transform.position = stopPosition;
		stopControl = 2;
		stopContPosition = cont2.transform.position;
		StartCoroutine (stopSnap ());
	}

	//-----------------------------------------------------------------------------------------------------------------------------

	//FUCKING BULLSHIT
	//move animation returns moveAttached true when complete attached and moveSnapComplete when scale damp is finished
	IEnumerator moveSnap(){
		moveAttached = false;
		stopAttached = false;
		bool moveSnapComplete = false;
		float timer = 0;
		float timerCount = 0;
		float stimer = 0;
		float stimerCount = 0;
		while (moveSnapComplete == false) {
			//can get rid of timers when fully decide on damp equation
			//position lerp
			/*
			transform.position = Vector3.Lerp (transform.position, pointer.transform.position, smoothTime);
			*/
			transform.position = Damp (transform.position, pointer.transform.position, cutoff, Time.deltaTime);

			//shrink lerp

			transform.localScale = Damp (transform.localScale, pointerScale, (cutoff * 5f), Time.deltaTime);


			//color lerp
			Color cont1color;
			cont1color = indMat.color;
			float currentAlpha = indMat.color.a;
			float coloralpha = Mathf.Lerp (currentAlpha, .2f, smoothTime);
			cont1color.a = coloralpha;
			indMat.color = cont1color;
			if (Mathf.Pow ((transform.position.y - pointer.transform.position.y), 2) > cutoff) {
				timer += Time.deltaTime;
				timerCount = timerCount + 1;
			} else {
				moveAttached = true;
				transform.position = pointer.position;

			}

			if (Mathf.Pow ((transform.localScale.x - pointerScale.x), 2) > cutoff) {
				stimer += Time.deltaTime;
				stimerCount = stimerCount + 1;
			} else {
				moveSnapComplete = true;
				transform.localScale = pointerScale;

			}
			//Timer Debugs for difference between position and scale
			/*
			Debug.Log ("position" + timer + (timer / timerCount));
			Debug.Log ("scale" + stimer + (stimer / stimerCount));
			*/
			yield return null;
		}
	}


	//stop animation returns stopAttached true when complete
    //this section is so when ring(s) "attach" or "detach" physics effect does not occur until "animation" has completed so that it feels niiiice  
	IEnumerator stopSnap(){
		stopAttached = false;
		moveAttached = false;
		while (stopAttached == false) {
			//because we need updated controller positions for lerp
			switch (stopControl) {
			case 1:
				stopContPosition = cont1.transform.position;
				break;
			case 2: 
				stopContPosition = cont2.transform.position;
				break;
			}

			stopAttached = false;

			transform.position = Damp (transform.position, stopContPosition, (cutoff * 5f), Time.deltaTime);

			transform.localScale = Damp (transform.localScale, contScale, (cutoff * 5f), Time.deltaTime);


			Color cont1color;
			cont1color = indMat.color;
			float currentAlpha = indMat.color.a;
			float coloralpha = Mathf.Lerp (currentAlpha, 1f, smoothTime);
			cont1color.a = coloralpha;
			indMat.color = cont1color;

			if (Mathf.Pow ((transform.position.y - stopContPosition.y), 2) < cutoff) {
				stopAttached = true;
				transform.position = stopContPosition;
				gameObject.SetActive (false);
			}
			yield return null;
		}
			
	}

	void AttachPointer (){
		indPosition = Position.Attached;
		transform.localPosition = pointer.transform.localPosition;
	}

	void AttachCont (){
		indPosition = Position.Attached;
		gameObject.SetActive (false);
		disablecont1.SetActive (false);
	}

	public Vector3 Damp(Vector3 source, Vector3 target, float cutoffSmoothing, float dt)
	{
		return Vector3.Lerp (source, target, 1 - Mathf.Pow (cutoffSmoothing, (dt/smoothTime)));
	}
	
}
