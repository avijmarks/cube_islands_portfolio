using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    TextNode mainNode;
	void Start () 
    {
        mainNode = GameText.generator.CreateNode("TUTORIAL", GameText.generator.style_Button, gameObject.transform, new Vector3(0f, 2f, 0f), Vector3.zero, MoreText);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void MoreText()
    {
        mainNode.TextStyle = GameText.generator.style_LabelText;
        mainNode.Message = "NEW MESSAGE";
    }
    
}
