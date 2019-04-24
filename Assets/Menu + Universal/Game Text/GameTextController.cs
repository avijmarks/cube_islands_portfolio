using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTextController: MonoBehaviour {

    [SerializeField]
    gameTextCharacter[] listOfCharacters_Initializer;


    Dictionary<char, gameTextCharacter> characters = new Dictionary<char, gameTextCharacter>();

    //textnode prefab for instantiation
    public GameObject textNode;




    //x value is scale, y value is spacing
    [SerializeField] 
    Vector2[] gameTextSizes;

    [SerializeField]
    Material[] materials;

    //empty character prefabs are nested under for initialization
    public GameObject gameTextPrefabsParent;

    public static GameTextController gameText = null;


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
    public GameObject CreateTextNode(string message, int textSize, int textMaterial,Vector3 rotationOffset, bool lookAtPlayer, Transform parentTransform, Vector3 positionOffset)


    {
       
        //instantiate textNode ( will be parent of characters)
        GameObject nodeInstance = Instantiate(textNode);
        nodeInstance.transform.position = parentTransform.position + positionOffset;



        //ROTATION CONDITIONAL, REENABLE IF WE GET CHARARRAY

        if (lookAtPlayer == false || lookAtPlayer == true) 
        {
            //rotation offset used here because characters position and rotation will always be same relative to textNode, it is textNode that controls overal rotation
            nodeInstance.transform.Rotate(parentTransform.rotation.eulerAngles + rotationOffset);

            Debug.Log(parentTransform.rotation.eulerAngles);
        }
        else
        {
            //*******call lookatplayer script here and remove or statement from first part of conditional
        }


        char[] messageArray = message.ToCharArray();

        Vector3[] characterLocalPositions = GetCharacterPositions(nodeInstance.transform, textSize, messageArray);


        for (int i = 0; i < messageArray.Length; i++) 
        {
            gameTextCharacter instance = Instantiate(characters[messageArray[i]], nodeInstance.transform);
            instance.transform.localPosition = characterLocalPositions[i];
           
            instance.transform.Rotate(0, 90, 90);
            instance.GetComponent<MeshRenderer>().material = materials[textMaterial];

        }
        
        return nodeInstance;
    }

    //uses textSize, messageArray.Length, and textNode position to calculate the localpositions (to textNode) where chars go (returns a vector3[] of positions) 
    Vector3[] GetCharacterPositions(Transform nodeTransform, int textSize, char[] messageArray)
    {
        Vector3[] characterPositions = new Vector3[messageArray.Length];

        //finds entire size string would be given textScale and inbetween space then divides by total number of characters to find characterTileSize
        float messageWorldSize = (gameTextSizes[textSize].x * messageArray.Length) + (gameTextSizes[textSize].y * (messageArray.Length - 1));
        float characterTileSize = messageWorldSize / messageArray.Length;
        float currentPosition;
        //initializing at first characterPosition x value
        currentPosition = (.5f * messageWorldSize) - (.5f * characterTileSize);
      


        for (int i = 0; i < messageArray.Length; i++)
        {
            Vector3 position = new Vector3(currentPosition, 0f, 0f);

            //adding to array
            characterPositions[i] = position;
            //incrementing position
            currentPosition = currentPosition - characterTileSize;
        }

        return characterPositions;
    }

    /*
    Vector3[] GetCharacterPositions(Vector3 position, float spacing) 
    { 

    }
    */




}
