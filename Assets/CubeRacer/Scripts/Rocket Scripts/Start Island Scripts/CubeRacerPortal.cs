using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeRacerPortal : MonoBehaviour {

	void OnTriggerEnter (){
		SceneManager.LoadScene ("CubeRacer");
	}

}
