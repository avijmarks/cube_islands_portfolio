using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class TextNode : EventTrigger {

    public GameTextCharacter[] prefabArray;
    public GameTextCharacter[] PrefabArray {
        get {return prefabArray;}
        set {prefabArray = value;}
    }

    string message;
    public string Message {
        get {return message;}
        set
        {
            message = value;
            GameText.generator.ChangeMessage(this);
        }
    }

    GameText.GameTextStyle textStyle;
    public GameText.GameTextStyle TextStyle { 
        get {return textStyle;}
        set{
            textStyle = value;
            InitializeNodeStyle(textStyle);
            GameText.generator.SetNodeTransform(this);
            GameText.generator.ChangeMessage(this);
            GameText.generator.ChangeIsButton(this);
            GameText.generator.ChangeTextSize(this);
            GameText.generator.ChangeTextMaterial(this);
        }
    }

    float textSize;
    public float TextSize { 
        get{return textSize;}
        set{
            textSize = value;
            GameText.generator.ChangeTextSize(this);
        }
    }

    int textMaterialNumber;
    public int TextMaterialNumber {
        get{return textMaterialNumber;}
        set {
            textMaterialNumber = value;
            GameText.generator.ChangeTextMaterial(this);
        }
    }

    bool isButton;
    public bool IsButton {
        get{return isButton;}
        
    }

    bool lookAtPlayer;
    public bool LookAtPlayer {
        get{return lookAtPlayer;}
        
    }

    bool isParented;
    public bool IsParented {
        get {return isParented;}
    }

    bool isHUD;
    public bool IsHUDE {
        get {return isHUD;}     
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    Transform parentTransform;
    public Transform ParentTransform {get {return parentTransform;}}
    

    Vector3 rotationOffset;
    public Vector3 RotationOffset {
        get {return rotationOffset;}
        set 
        {
            transform.Rotate(ParentTransform.rotation.eulerAngles + value);
            rotationOffset = value;
        }
    }

    Vector3 positionOffset;
    public Vector3 PositionOffset {
        get {return positionOffset;}
        set {positionOffset = value;}
    }

    
    public ButtonPad buttonPad;
    //should go on buttonpad
    public GameText.GameTextButtonTrigger trigger;
    public GameText.GameTextButtonTrigger Trigger {
        get {return trigger;}
    }

    

    public void SetPositionOffset (Vector3 posOffset)
    {
        transform.position = ParentTransform.transform.TransformPoint(posOffset);
        
    }

    public void InitializeNode (string _message, GameText.GameTextStyle _style, Transform _parentTransform, Vector3 _positionOffset, Vector3 _rotationOffset, 
                                                                                                                GameText.GameTextButtonTrigger _trigger = null)
    {
        message = _message;
        textStyle = _style;
        parentTransform = _parentTransform;
        positionOffset = _positionOffset;
        rotationOffset = _rotationOffset;
        trigger = _trigger; 
        InitializeNodeStyle (_style);
    }

    public void InitializeNodeStyle (GameText.GameTextStyle _style)
    {
        textSize = _style.textSize;
        textMaterialNumber = _style.materialNumber;
        isButton = _style.isButton;
        lookAtPlayer = _style.lookAtPlayer;
        isParented = _style.isParented;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
