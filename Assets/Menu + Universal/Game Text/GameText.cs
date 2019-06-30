using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameText: MonoBehaviour {

    [SerializeField]
    GameTextCharacter[] listOfCharacters_Initializer;

    
    Dictionary<char, GameTextCharacter> characters = new Dictionary<char, GameTextCharacter>();

    //scales text size because characters dont align with scaling
    public float textSizeScaling = .7f;
    //textnode prefab for instantiation
    public TextNode textNodePrefab; 
    //buttonPad prefab for instantiation
    public ButtonPad buttonPadPrefab;

    [SerializeField]
    Material[] materials;

    //empty character prefabs are nested under for initialization
    public GameObject gameTextPrefabsParent;

    //SINGLETON
    public static GameText generator = null;

    //used to store styles created with CreateGameTextStyleFunction
    public Dictionary<string, GameTextStyle> gameTextStyles = new Dictionary<string, GameTextStyle>();

    //used to set trigger method on button click
    public delegate void GameTextButtonTrigger();


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //GAMESTYLE CLASS AND CONSTRUCTOR
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    //ADD CUSTOM TEXT STYLES HERE -- MAKE SURE TO INITIALIZE IN AWAKE
    [SerializeField]
    public GameTextStyle style_LabelText;
    public GameTextStyle style_Button;
    public GameTextStyle style_HUDMenuLabel;
    public GameTextStyle style_HUDButton;
    
    

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    public class GameTextStyle : MonoBehaviour
    {
        public float textSize;
        public int materialNumber;
        public string styleName;
        public bool isButton;
        public bool lookAtPlayer;
        public bool isParented;

        public GameTextStyle (string _styleName, float _textSize, int _textMaterialNumber, bool _isButton, bool _lookAtPlayer, bool _isParented)
        {
            //constructor for new text styles
            styleName = _styleName;
            textSize = _textSize;
            materialNumber = _textMaterialNumber;
            isButton = _isButton;
            lookAtPlayer = _lookAtPlayer;
            isParented = _isParented;
        }
    }
    

    
    
   




    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //INITIALIZATION
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    /// <summary>
    /// Initializes dictionary used by gametext with a key of type char and an index item of the prefab of the character
    /// ----> methods will need to be called by external source (e.g. game manager)
    ///
    /// This method of initializing gameText autopopulates based on an INITIALARRAY copy of all the prefabs in the environment parented under an object.
    /// This is because it makes it really easy for us to add characters as we fit during development
    /// ---->will eventually want to switch to having them in a preset array so we don't need copies of them in the scene 
    /// </summary>
    /// <returns>GameText Character Dictionary</returns>


    //creates a dictionary for us of alphabet prefabs and characters by accessing each index of prefab array and finding out its string character variable
    void Awake()
    {
        
        if (generator == null)
        {
            generator = this; 
        } else if (generator != null)
        {
            //destroy this instance text here.
        }

        GameTextCharacter[] initializer = InitialArray();
        for (int i = 0; i < initializer.Length; i++)
        {
            characters.Add(initializer[i].characterID, initializer[i]);
        }

        //MAKE SURE TO ADD CUSTOM BASE STYLES HERE
        style_LabelText = new GameTextStyle("LabelText", .9f, 1, false, false, false);
        style_Button = new GameTextStyle("Button", .6f, 1, true, false, false);
        style_HUDMenuLabel = new GameTextStyle("HUDMenuLabel", .1f, 1, false, false, true);
        style_HUDButton =  new GameTextStyle("HUDButton", .07f, 1, true,false,true);
    }

    GameTextCharacter[] InitialArray()
    {
        //can remove this line and array when no longer want to see autopopulate characters in inspector
        listOfCharacters_Initializer = gameTextPrefabsParent.GetComponentsInChildren<GameTextCharacter>();

        GameTextCharacter[] dictionaryPrefabInitializer = gameTextPrefabsParent.GetComponentsInChildren<GameTextCharacter>();
        return dictionaryPrefabInitializer;
    }



    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //CREATING GAMETEXT
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    //message = self explanatory; textSize & material = choose from array in inspector; rotOffset in case instantiating object has weird rot; 
    //************************************************************ */
    public TextNode CreateNode(string message, GameTextStyle style, Transform parentTransform, Vector3 positionOffset, Vector3 rotationOffset, 
                                                                                                                GameTextButtonTrigger trigger = null)
    {
        
        TextNode nodeInstance = Instantiate(textNodePrefab);
        nodeInstance.InitializeNode(message, style, parentTransform, positionOffset, rotationOffset, trigger);
        //sets transform hierarchy position, position, and rotation
        SetNodeTransform(nodeInstance);
        //changes message and sets prefab array on textnode
        ChangeMessage(nodeInstance);
        //changes material (integer index for array)
        ChangeTextMaterial(nodeInstance);
        //changes textsize (based on number not array index)
        ChangeTextSize(nodeInstance);
        //self explanatory -- creates button size depends on message array size already being correct
        ChangeIsButton(nodeInstance);
        return nodeInstance;
    }
    public void SetNodeTransform(TextNode node)
    {
        ResetNode(node);
        node.transform.Rotate(node.ParentTransform.rotation.eulerAngles + node.RotationOffset);
        //unparenting so position can be set regardless of hierarchy position.
        node.transform.SetParent(node.transform);
        node.transform.position = node.ParentTransform.transform.TransformPoint(node.PositionOffset);
        if (node.IsParented == true)
        {
            node.transform.SetParent(node.ParentTransform);
        }
    }

    public void ChangeMessage(TextNode node)
    {
        if (node.GetComponentsInChildren<GameTextCharacter>().Length > 0)
        {
            GameTextCharacter[] previousCharacters = node.GetComponentsInChildren<GameTextCharacter>();
            for (int i = 0; i < previousCharacters.Length; i++)
            {
                Destroy(previousCharacters[i].gameObject);
            }
            ButtonPad previousPad = node.GetComponentInChildren<ButtonPad>();
            if (previousPad != null) 
            {
                Destroy(previousPad.gameObject);
            }
        
        }

        char[] messageArray = node.Message.ToCharArray();
        Vector3[] characterLocalPositions = GetCharacterPositions(node.transform, node.TextSize, messageArray);
        GameTextCharacter[] prefabArray = new GameTextCharacter[messageArray.Length];
         for (int i = 0; i < messageArray.Length; i++)
        {
            GameTextCharacter instance = Instantiate(characters[messageArray[i]], node.transform);
            instance.transform.localPosition = characterLocalPositions[i];

            instance.transform.Rotate(0, 90, 90);
            
            prefabArray[i] = instance;
        }
        node.PrefabArray = prefabArray;
    }
    public void ChangeTextMaterial (TextNode node)
    {
        int prefabMat = node.TextMaterialNumber;
        for (int i = 0; i < node.PrefabArray.Length; i++)
        {
            node.PrefabArray[i].GetComponent<MeshRenderer>().material = materials[prefabMat];
        }
    }

    public void ChangeTextSize (TextNode node)
    {
        for (int i = 0; i < node.PrefabArray.Length; i++)
        {
            GameTextCharacter character = node.PrefabArray[i];
            character.transform.localScale = new Vector3(node.TextSize, node.TextSize, node.TextSize);
        }
    }

    public void ChangeIsButton(TextNode node)
    {
        //creates button pad prefab instance and sets size based on TextSize
        if (node.GetComponentsInChildren<ButtonPad>().Length > 0)
        {
            ButtonPad[] oldButtons = node.GetComponentsInChildren<ButtonPad>();
            Destroy(oldButtons[0]);
        }
        char[] messageArray = node.Message.ToCharArray();
        if (node.IsButton)
        {
            float buttonScale = .3f;
            node.buttonPad = Instantiate(buttonPadPrefab, node.transform);
            node.buttonPad.trigger = node.trigger;
            node.buttonPad.textStyle = node.TextStyle;
            node.buttonPad.node = node;
            node.buttonPad.transform.localPosition = new Vector3(0, 0, -(.05f * node.TextSize));

            float charactarSpace = node.TextStyle.textSize * .7f;
            float X = (charactarSpace * messageArray.Length) + (buttonScale * charactarSpace);
            float Y = charactarSpace + (buttonScale * charactarSpace);
            float Z = (.05f * charactarSpace);
            node.buttonPad.transform.localScale = new Vector3(X, Y, Z); 
        }

    }

//uses textSize, messageArray.Length, and textNode position to calculate the localpositions (to textNode) where chars go 
//(returns a vector3[] of positions) 
      Vector3[] GetCharacterPositions(Transform nodeTransform, float textSize, char[] messageArray)
    {
        Vector3[] characterPositions = new Vector3[messageArray.Length];


        float characterSpace = textSize * textSizeScaling;
        float messageWorldSize = characterSpace * characterPositions.Length;


        float currentPosition;
        //initializing at first characterPosition x value
        currentPosition = (.5f * messageWorldSize) - (.5f * characterSpace);
      
        for (int i = 0; i < messageArray.Length; i++)
        {
            Vector3 position = new Vector3(currentPosition, 0f, 0f);

            //adding to array
            characterPositions[i] = position;
            //incrementing position
            currentPosition = currentPosition - characterSpace;
        }
        return characterPositions;
    }

    void ResetNode(TextNode node)
    {
        node.transform.localScale = new Vector3(1f, 1f, 1f);
        Quaternion originRot = node.transform.rotation;
        originRot.eulerAngles = Vector3.zero;
        node.transform.rotation = originRot;
        //stop node look at coroutine HERE
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //BUTTON EVENTS
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public void PointerOnButton(ButtonPad buttonInstance)
    {
        StartCoroutine(ButtonHover(buttonInstance)); 
    }
    IEnumerator ButtonHover (ButtonPad buttonInstance)
    {
        //for node size
        float vel = 0.0f;

        //for node movement
        float moveVel = 0.0f;
        float moveSpeed = 4.0f;
        float current = 0f;
        float target = .25f * buttonInstance.textStyle.textSize;
        
        Vector3 newPos;

        //changes color and moves button vector3.forward on "mouseover" (uses SmoothDamp)
        while (current < target-.01f)
        {
            Color color = buttonInstance.GetComponent<MeshRenderer>().material.color;
            color.a = Mathf.SmoothDamp(color.a, .55f, ref vel, .02f);
            buttonInstance.GetComponent<MeshRenderer>().material.color = color;
            
        
            current = Mathf.SmoothDamp(current, target, ref moveVel, moveSpeed * Time.deltaTime);
            
            
            
            
            buttonInstance.node.transform.Translate(0f, 0f, current);
            

           
            
            yield return null;

        }
    }

    public void PointerExitButton(ButtonPad buttonInstance) 
    {
        StartCoroutine(ButtonHoverExit(buttonInstance));
    }
    IEnumerator ButtonHoverExit (ButtonPad buttonInstance)
    {
        //for node size
        float vel = 0f;

        //for node movement
        float moveVel = 0.0f;
        float moveSpeed = 4.0f;
        float current = 0f;
        float target = .25f * buttonInstance.textStyle.textSize;
        while (current < target-.01f)
        {
            Color color = buttonInstance.GetComponent<MeshRenderer>().material.color;
            color.a = Mathf.SmoothDamp(color.a, .17f, ref vel, .02f);
            buttonInstance.GetComponent<MeshRenderer>().material.color = color;

            current = Mathf.SmoothDamp(current, target, ref moveVel, moveSpeed * Time.deltaTime);
            
            
            
            
            buttonInstance.node.transform.Translate(0f, 0f, -current);

            yield return null;
        }
    }

    public void GameTextButtonClick (ButtonPad buttonInstance)
    {
        StartCoroutine(TextButtonClick(buttonInstance));
    }
    IEnumerator TextButtonClick (ButtonPad buttonInstance) 
    {
        float speed = 2.0f;
        float targetScale = .09f;
        float vel = 0f;
        while (buttonInstance.transform.parent.transform.localScale.x >.1)
        {
            Vector3 newScale = buttonInstance.transform.parent.transform.localScale;
            newScale.x = Mathf.SmoothDamp(newScale.x, targetScale, ref vel, speed * Time.deltaTime);
            newScale.y = Mathf.SmoothDamp(newScale.y, targetScale, ref vel, speed * Time.deltaTime);
            buttonInstance.transform.parent.transform.localScale = newScale;
            yield return null;
        }
        //stopping all coroutines to prevent errors in case hover or exit animation still running (for now)
        StopAllCoroutines();
        buttonInstance.trigger();
    }

}
