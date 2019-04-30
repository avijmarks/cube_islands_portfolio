using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    TextNode tutorialNodeLabel;
	void Start () 
    {
        tutorialNodeLabel = GameTextController.gameText.CreateTextNode("TUTORIAL", "Button", gameObject.transform, new Vector3(0f, 2f, 0f), Vector3.zero, MoreText);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void MoreText()
    {
        Destroy(tutorialNodeLabel.gameObject);
        TextNode more = GameTextController.gameText.CreateTextNode("MOREMOREMORE~TEXT", "LabelText", gameObject.transform, new Vector3(0f, 2f, 0f));
    }
    
}
