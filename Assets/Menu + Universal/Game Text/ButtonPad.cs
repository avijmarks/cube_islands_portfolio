using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPad : EventTrigger {

    public string textStyle;
    public GameTextController.GameTextButtonTrigger trigger;


    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        GameTextController.gameText.PointerOnButton(this);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        GameTextController.gameText.PointerExitButton(this);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        GameTextController.gameText.GameTextButtonClick(this);
    }   
    

}
