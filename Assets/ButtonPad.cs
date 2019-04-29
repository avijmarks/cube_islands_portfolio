using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPad : EventTrigger {

    public string textStyle;
    public GameTextController.GameTextButtonTrigger trigger;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void PointerOnButton()
    {
        GameTextController.gameText.PointerOnButton(gameObject, textStyle);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        PointerOnButton();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        GameTextController.gameText.PointerExitButton(gameObject, textStyle);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("clicked");
        base.OnPointerClick(eventData);
        trigger();
    }

    public void Downnnn()
    {
        Debug.Log("downnn");
    }
    

}
