using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class contlocation : MonoBehaviour {

	public GameObject controllerr1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = controllerr1.transform.position;

	}
}
