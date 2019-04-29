using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTextController: MonoBehaviour {

    [SerializeField]
    gameTextCharacter[] listOfCharacters_Initializer;


    Dictionary<char, gameTextCharacter> characters = new Dictionary<char, gameTextCharacter>();

    //textnode prefab for instantiation
    public TextNode textNodePrefab; 
    //buttonPad prefab for instantiation
    public GameObject buttonPadPrefab;

    //x value is scale, y value is spacing
    [SerializeField] 
    Vector2[] gameTextSizes;
    

    [SerializeField]
    Material[] materials;

    //empty character prefabs are nested under for initialization
    public GameObject gameTextPrefabsParent;

    public static GameTextController gameText = null;

    //used to store styles created with CreateGameTextStyleFunction
    Dictionary<string, GameTextStyle> gameTextStyles = new Dictionary<string, GameTextStyle>();

    public Color buttonColorPassive;
    public Color buttonColorActive;


    
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
        if (gameText == null)
        {
            gameText = this; 
        } else if (gameText != null)
        {
            //destroy this instance text here.
        }

        gameTextCharacter[] initializer = InitialArray();

        for (int i = 0; i < initializer.Length; i++)
        {
            characters.Add(initializer[i].characterID, initializer[i]);
        }

        CubeIslandTextStyles();
    }

    gameTextCharacter[] InitialArray()
    {
        //can remove this line and array when no longer want to see autopopulate characters in inspector
        listOfCharacters_Initializer = gameTextPrefabsParent.GetComponentsInChildren<gameTextCharacter>();

        

        gameTextCharacter[] dictionaryPrefabInitializer = gameTextPrefabsParent.GetComponentsInChildren<gameTextCharacter>();

        return dictionaryPrefabInitializer;
    }

    

 


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //CREATING GAMETEXT
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


    //message = self explanatory; textSize & material = choose from array in inspector; rotOffset in case instantiating object has weird rot; 
    public TextNode CreateTextNode(string message, string style, Transform parentTransform, Vector3 positionOffset, Vector3 rotationOffset)
    {
       
        //instantiate textNode ( will be parent of characters)
        TextNode nodeInstance = Instantiate(textNodePrefab);
        nodeInstance.transform.position = parentTransform.position + positionOffset;
        //rotation offset used here because characters position and rotation will always be same relative to textNode, it is textNode that controls overal rotation
        nodeInstance.transform.Rotate(parentTransform.rotation.eulerAngles + rotationOffset);

        char[] messageArray = message.ToCharArray();
        Vector3[] characterLocalPositions = GetCharacterPositions(nodeInstance.transform, gameTextStyles[style].textSize, messageArray);

        for (int i = 0; i < messageArray.Length; i++) 
        {
            gameTextCharacter instance = Instantiate(characters[messageArray[i]], nodeInstance.transform);
            instance.transform.localPosition = characterLocalPositions[i];
           
            instance.transform.Rotate(0, 90, 90);
            instance.GetComponent<MeshRenderer>().material = materials[gameTextStyles[style].material];
            instance.transform.localScale = new Vector3(gameTextStyles[style].textSize, gameTextStyles[style].textSize, gameTextStyles[style].textSize);
        }


        if (gameTextStyles[style].isButton)
        {
            nodeInstance.buttonPad = Instantiate(buttonPadPrefab, nodeInstance.transform);
            nodeInstance.buttonPad.transform.localPosition = new Vector3(0, 0, -(.05f * gameTextStyles[style].textSize));

            float charactarSpace = gameTextStyles[style].textSize * .7f;
            float X = (charactarSpace * messageArray.Length) + (.15f * charactarSpace);
            float Y = charactarSpace + (.15f * charactarSpace);
            float Z = (.05f * charactarSpace);

            nodeInstance.buttonPad.transform.localScale = new Vector3(X, Y, Z);
        }

        
        return nodeInstance;
    }
    public TextNode CreateTextNode(string message, string style, Transform parentTransform, Vector3 positionOffset)
    {
       TextNode instance = CreateTextNode(message, style, parentTransform, positionOffset, Vector3.zero);
       return instance;
    }




    //uses textSize, messageArray.Length, and textNode position to calculate the localpositions (to textNode) where chars go (returns a vector3[] of positions) 
    Vector3[] GetCharacterPositions(Transform nodeTransform, float textSize, char[] messageArray)
    {
        Vector3[] characterPositions = new Vector3[messageArray.Length];

        //finds entire size string would be given textScale and inbetween space then divides by total number of characters to find characterTileSize
        float characterSpace = textSize * .7f;
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

    public void PointerOnButton(GameObject buttonPad, string textStyle)
    {
        StartCoroutine(ButtonHover( buttonPad, textStyle));
    }

    IEnumerator ButtonHover (GameObject buttonPad, string textStyle)
    {
        float vel = 0.0f;

        while (buttonPad.GetComponent<MeshRenderer>().material.color.a < .54f)
        {
            Color color = buttonPad.GetComponent<MeshRenderer>().material.color;
            color.a = Mathf.SmoothDamp(color.a, .55f, ref vel, .02f);
            buttonPad.GetComponent<MeshRenderer>().material.color = color;
            yield return null;
        }
    }

    public void PointerExitButton(GameObject buttonPad, string textStyle)
    {
        StartCoroutine(ButtonHoverExit(buttonPad, textStyle));
    }

    IEnumerator ButtonHoverExit (GameObject buttonPad, string textStyle)
    {
        float vel = 0f;

        while (buttonPad.GetComponent<MeshRenderer>().material.color.a > .18)
        {
            Color color = buttonPad.GetComponent<MeshRenderer>().material.color;
            color.a = Mathf.SmoothDamp(color.a, .17f, ref vel, .02f);
            buttonPad.GetComponent<MeshRenderer>().material.color = color;
            yield return null;
        }
    }




    public class GameTextStyle: MonoBehaviour 
    {
        public float textSize;
        public int material; 
        public string styleName;
        public bool isButton; 
        public bool lookAtPlayer;
        public bool isParented;


        //(may be used)only required if parent is true -- currently not used in functions
        public float scaleOffset;

    }

    //basic constructor for new text styles
    public void NewGameTextStyle (string styleName, float textSize, int textMaterial, bool isButton, bool lookAtPlayer, bool isParented)
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

    public void CubeIslandTextStyles ()
    {
        NewGameTextStyle("LabelText", .9f, 1, false, false, false);
        NewGameTextStyle("Button", .6f, 1, true, false, false);
    }
    




}
