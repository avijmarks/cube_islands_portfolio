using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class TextNode : EventTrigger {

    public string textStyle;
    public GameObject buttonPad;
    public delegate void OnButtonTrigger(string textStyle);
    public OnButtonTrigger onButtonTrigger;




    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PointerOnButton()
    {
        GameTextController.gameText.PointerOnButton(buttonPad, textStyle);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        PointerOnButton();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        GameTextController.gameText.PointerExitButton(buttonPad, textStyle);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (onButtonTrigger == null)
        {
            Debug.Log("Did not set OnPointerClick for text button" + gameObject);
        }
        onButtonTrigger(textStyle);
    }
}
