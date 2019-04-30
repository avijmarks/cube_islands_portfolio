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
    Dictionary<string, GameTextStyle> gameTextStyles = new Dictionary<string, GameTextStyle>();

    //used to set trigger method on button click
    public delegate void GameTextButtonTrigger();


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //GAMESTYLE CLASS AND CONSTRUCTOR
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    //ADD CUSTOM TEXT STYLES HERE
    void MyTextStyles()
    {
        NewGameTextStyle("LabelText", .9f, 1, false, false, false);
        NewGameTextStyle("Button", .6f, 1, true, false, false);
        NewGameTextStyle("HUDMenuLabel", .1f, 1, false, false, true);
        NewGameTextStyle("HUDButton", .07f, 1, true,false,true);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    public class GameTextStyle : MonoBehaviour
    {
        public float textSize;
        public int material;
        public string styleName;
        public bool isButton;
        public bool lookAtPlayer;
        public bool isParented;
    }

    //basic constructor for new text styles
    public void NewGameTextStyle(string styleName, float textSize, int textMaterial, bool isButton, bool lookAtPlayer, bool isParented)
    {
        GameTextStyle newStyle = new GameTextStyle();
        newStyle.styleName = styleName;
        newStyle.textSize = textSize;
        newStyle.material = textMaterial;
        newStyle.isButton = isButton;
        newStyle.lookAtPlayer = lookAtPlayer;
        newStyle.isParented = isParented;

        gameTextStyles.Add(styleName, newStyle);
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
        MyTextStyles();
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
    public TextNode CreateTextNode(string message, string style, Transform objectTextIsOn, Vector3 positionOffset, Vector3 rotationOffset, Transform parentTransform = null, GameTextButtonTrigger trigger = null)
    {
       
        //instantiate textNode ( will be parent of characters)
        TextNode nodeInstance = Instantiate(textNodePrefab);
        
        //rotation offset used here because characters position and rotation will always be same relative to textNode, it is textNode that controls overal rotation
        nodeInstance.transform.Rotate(objectTextIsOn.rotation.eulerAngles + rotationOffset);

        if (gameTextStyles[style].isParented == true)
        {
            nodeInstance.transform.SetParent(parentTransform);
            positionOffset = objectTextIsOn.transform.TransformPoint(positionOffset);
            positionOffset = parentTransform.transform.InverseTransformPoint(positionOffset);
            nodeInstance.transform.localPosition = positionOffset;
            
        } else {
            nodeInstance.transform.position = objectTextIsOn.transform.TransformPoint(positionOffset);
            
        }

        nodeInstance = ChangeNodeMessage(nodeInstance, message, style, trigger);
        return nodeInstance;
    }
    public TextNode CreateTextNode(string message, string style, Transform parentTransform, Vector3 positionOffset)
    {


        TextNode instance = CreateTextNode(message, style, parentTransform, positionOffset, Vector3.zero);
        return instance;
    }


    //Changes message on textnode -- also used when initially creating node
    public TextNode ChangeNodeMessage(TextNode nodeInstance, string message, string style, GameTextButtonTrigger trigger)
    {
        //getting rid of old text/button (if any)
        if (nodeInstance.GetComponentsInChildren<GameTextCharacter>().Length > 0)
        {
            GameTextCharacter[] previousCharacters = nodeInstance.GetComponentsInChildren<GameTextCharacter>();
            for (int i = 0; i <= previousCharacters.Length; i++)
            {
                Destroy(previousCharacters[i]);
            }
            if (nodeInstance.GetComponentsInChildren<ButtonPad>().Length > 0)
            {
                ButtonPad[] oldButtons = nodeInstance.GetComponentsInChildren<ButtonPad>();
                Destroy(oldButtons[0]);
            }
        }

        nodeInstance.textStyle = style;

        char[] messageArray = message.ToCharArray();
        Vector3[] characterLocalPositions = GetCharacterPositions(nodeInstance.transform, gameTextStyles[style].textSize, messageArray);

        for (int i = 0; i < messageArray.Length; i++)
        {
            GameTextCharacter instance = Instantiate(characters[messageArray[i]], nodeInstance.transform);
            instance.transform.localPosition = characterLocalPositions[i];

            instance.transform.Rotate(0, 90, 90);
            instance.GetComponent<MeshRenderer>().material = materials[gameTextStyles[style].material];
            instance.transform.localScale = new Vector3(gameTextStyles[style].textSize, gameTextStyles[style].textSize, gameTextStyles[style].textSize);
        }


        if (gameTextStyles[style].isButton)
        {
            float buttonScale = .3f;
            nodeInstance.buttonPad = Instantiate(buttonPadPrefab, nodeInstance.transform);
            nodeInstance.buttonPad.trigger = trigger;
            nodeInstance.buttonPad.textStyle = style;
            nodeInstance.buttonPad.transform.localPosition = new Vector3(0, 0, -(.05f * gameTextStyles[style].textSize));

            float charactarSpace = gameTextStyles[style].textSize * .7f;
            float X = (charactarSpace * messageArray.Length) + (buttonScale * charactarSpace);
            float Y = charactarSpace + (buttonScale * charactarSpace);
            float Z = (.05f * charactarSpace);
            nodeInstance.buttonPad.transform.localScale = new Vector3(X, Y, Z);

            //setting properties on TextNode instance
            nodeInstance.trigger = trigger;

        }
        //setting style property on TextNode instance



        return nodeInstance;
    }


    //uses textSize, messageArray.Length, and textNode position to calculate the localpositions (to textNode) where chars go (returns a vector3[] of positions) 
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
        float startZPos = buttonInstance.transform.parent.transform.localPosition.z;
        float targetPos = startZPos + ((gameTextStyles[buttonInstance.textStyle].textSize * textSizeScaling) * .3f);

        while (!Mathf.Approximately(buttonInstance.transform.parent.transform.position.z, targetPos))
        {
            Color color = buttonInstance.GetComponent<MeshRenderer>().material.color;
            color.a = Mathf.SmoothDamp(color.a, .55f, ref vel, .02f);
            buttonInstance.GetComponent<MeshRenderer>().material.color = color;

            float newPosZ = buttonInstance.transform.parent.transform.localPosition.z;
            newPosZ = Mathf.SmoothDamp(newPosZ, targetPos, ref moveVel, moveSpeed * Time.deltaTime);
            Vector3 newPos = new Vector3(buttonInstance.transform.parent.transform.localPosition.x, buttonInstance.transform.parent.transform.localPosition.y, newPosZ);
            buttonInstance.transform.parent.transform.localPosition = newPos;
            
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
        float startZPos = buttonInstance.transform.parent.transform.localPosition.z;
        float targetPos = startZPos - ((gameTextStyles[buttonInstance.textStyle].textSize * textSizeScaling) * .3f);
        while (!Mathf.Approximately(buttonInstance.transform.parent.transform.position.z, targetPos))
        {
            Color color = buttonInstance.GetComponent<MeshRenderer>().material.color;
            color.a = Mathf.SmoothDamp(color.a, .17f, ref vel, .02f);
            buttonInstance.GetComponent<MeshRenderer>().material.color = color;

            float newPosZ = buttonInstance.transform.parent.transform.localPosition.z;
            newPosZ = Mathf.SmoothDamp(newPosZ, targetPos, ref moveVel, moveSpeed * Time.deltaTime);
            Vector3 newPos = new Vector3(buttonInstance.transform.parent.transform.localPosition.x, buttonInstance.transform.parent.transform.localPosition.y, newPosZ);
            buttonInstance.transform.parent.transform.localPosition = newPos;

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
