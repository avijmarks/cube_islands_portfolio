using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTesting : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log(transform.TransformPoint(0f, 0f, 2f));
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
