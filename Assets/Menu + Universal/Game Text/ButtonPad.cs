using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPad : EventTrigger {

    public string textStyle;
    public GameText.GameTextButtonTrigger trigger;


    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        GameText.generator.PointerOnButton(this);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        GameText.generator.PointerExitButton(this);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        GameText.generator.GameTextButtonClick(this);
    }   
    

}
