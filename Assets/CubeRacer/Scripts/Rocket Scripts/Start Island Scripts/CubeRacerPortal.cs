using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeRacerPortal : MonoBehaviour {

    bool buffer = false;
    public GameObject mainCamera;
    public GameObject playerParent;

    List<TextNode> textNodes = new List<TextNode>();
    private void Start()
    {
        
        GameText.generator.CreateTextNode("CUBE RACER", "LabelText", gameObject.transform.parent, new Vector3(0f, 3f, 0f), new Vector3(0, -42f, 0)); 
        bool buffer = false; 
         
    }

    void OnTriggerEnter (){
        
        if (buffer == false)
        {
            buffer=true;

            Vector3 rot = new Vector3(mainCamera.transform.rotation.eulerAngles.x, mainCamera.transform.rotation.eulerAngles.y + 180f, 
                        mainCamera.transform.rotation.eulerAngles.z);

            textNodes.Add(GameText.generator.CreateTextNode("TELEPORT TO CUBERACER", "HUDMenuLabel", playerParent.transform, 
                        mainCamera.transform.TransformDirection(0f,.3f,1.5f), rot));
            textNodes.Add(GameText.generator.CreateTextNode("NO", "HUDButton", playerParent.transform,  mainCamera.transform.TransformDirection(.3f, -.3f, 1.5f), 
                        rot, DestroyAllActiveNodes));  
            textNodes.Add(GameText.generator.CreateTextNode("YES", "HUDButton", playerParent.transform, mainCamera.transform.TransformDirection(-.3f, -.3f, 1.5f), 
                        rot, GoToCubeRacer));
        }
    }
    void GoToCubeRacer ()
    {
        SceneManager.LoadScene ("CubeRacer");
    }

    void DestroyAllActiveNodes ()
    {
        for (int i = 0; i < textNodes.Count; i++)
        {
            Destroy(textNodes[i].gameObject);
            textNodes.Remove(textNodes[i]);
        }
    }
    


}
