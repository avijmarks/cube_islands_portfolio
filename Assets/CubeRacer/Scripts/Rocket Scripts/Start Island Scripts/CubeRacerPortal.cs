using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeRacerPortal : MonoBehaviour {


    private void Start()
    {
        GameTextController.gameText.CreateTextNode("CUBE~RACER", "LabelText", gameObject.transform.parent, new Vector3(0f, 2f, 0f), new Vector3(-90f, 0f, -47f));     
    }

    void OnTriggerEnter (){
		SceneManager.LoadScene ("CubeRacer");

        /*
        GameTextController.CreateTextNode("Cube~Racer", 0, 1, new Vector3(90f, 0f, 0f), false, gameObject.transform);

        */
    }
    


}
