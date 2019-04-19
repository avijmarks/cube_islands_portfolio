using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTextController : MonoBehaviour {
    //need to create a serialized array b/c seriazlized dictionary did not work. can iterate through and add to dictionary using the characterID field in gameTextCharacter class
    [SerializeField]
    gameTextCharacter[] listOfCharacters_Initializer;


    Dictionary<char, gameTextCharacter> characters = new Dictionary<char, gameTextCharacter>();

    //empty prefabs are nested under 
    public GameObject gameTextPrefabsParent;

    //textnode prefab for instantiation
    public GameObject textNode;


    //storage of standard data for reference by instanceinfo struct

    //x value is scale, y value is spacing
    [SerializeField] 
    Vector2[] gameTextSizes;

    [SerializeField]
    Material[] materials;






    
    private void Start()
    {
        //can remove this line and array when no longer want to see autopopulate
        listOfCharacters_Initializer = gameTextPrefabsParent.GetComponentsInChildren<gameTextCharacter>();

        

        gameTextCharacter[] dictionaryPrefabInitializer = gameTextPrefabsParent.GetComponentsInChildren<gameTextCharacter>();
       
        GetAlphabetDictionary(dictionaryPrefabInitializer);


        CreateTextNode("please god work", 0, 0, transform.position, false, gameObject.transform);
    }

    //creates a dictionary for us of alphabet prefabs and characters by accessing each index of prefab array and finding out its string character variable
    public void GetAlphabetDictionary (gameTextCharacter[] initializer) 
    {
        for (int i = 0; i < initializer.Length; i++) 
        {
            Debug.Log(initializer[i].characterID);
            characters.Add(initializer[i].characterID, initializer[i]);
        }

    }

    gameTextCharacter InstantiateCharacter (char id, Vector3 position) 
    {
        //can make this set parent object with second parameter, need to pass node instance into this function so we can use node to destroy
        gameTextCharacter instance = Instantiate(characters[id]);
        return instance;
    }


    public GameObject CreateTextNode(string message, int textSize, int material,Vector3 position, bool lookAtPlayer, Transform ParentTransform)
        //NEED TO RE-ADD ROTATION
    {
        if (message == null) 
        {
            Debug.Log("No string was sent to GameTextController");
        }

        //instantiate node
        //needs to generate matching list of vector3 positions with same indices as string.toarry(or whatever its called)
        //iterate through.toarray
        GameObject nodeInstance = Instantiate(textNode, ParentTransform);


        //ROTATION CONDITIONAL, REENABLE IF WE GET CHARARRAY
        /*
        if (lookAtPlayer == false) 
        {
            Quaternion qRotation = Quaternion.Euler(rotation);
            nodeInstance.transform.rotation = nodeInstance.transform.rotation * qRotation;
        }
        else
        {
            //call lookatplayer script here
        }
        */

        char[] messageArray = message.ToCharArray();

        Debug.Log(messageArray.Length);
        return textNode;
    }

    /*
    Vector3[] GetCharacterPositions(Vector3 position, float spacing) 
    { 

    }
    */




}
