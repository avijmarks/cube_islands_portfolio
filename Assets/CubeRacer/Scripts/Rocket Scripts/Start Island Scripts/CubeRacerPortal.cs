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
            textNodes.Add(GameText.generator.CreateTextNode("TELEPORT TO CUBERACER", "HUDMenuLabel", mainCamera.transform, new Vector3(0f,.3f,1.5f), new Vector3(0f, 180f, 0f), playerParent.transform));
            textNodes.Add(GameText.generator.CreateTextNode("NO", "HUDButton", mainCamera.transform,  new Vector3(.3f, -.3f, 1.5f), new Vector3(0f, 180f, 0f), playerParent.transform, DestroyAllActiveNodes));
            textNodes.Add(GameText.generator.CreateTextNode("YES", "HUDButton", mainCamera.transform, new Vector3(-.3f, -.3f, 1.5f), new Vector3(0f, 180f, 0f), playerParent.transform, GoToCubeRacer));
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
        Destroy(this.gameObject);
    }
    


}
