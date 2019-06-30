    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

        
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public bool active = false;

    public int TypeIndex;
    
    

    

    private void Awake()
    {
        position = gameObject.transform.position;
        rotation = gameObject.transform.rotation;
        scale = gameObject.transform.localScale;

    }

    public void UpdatePosition() {
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
        gameObject.transform.localScale = scale;
    }

    
      

}
