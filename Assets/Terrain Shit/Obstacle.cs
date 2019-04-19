    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

        
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public Vector2 scaleRange;
    public int spawnType;
    public int spawnQuantity;
    public int activeQuantity; 
    public int type;
    public bool active = false;
     
    

    

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

    public void SetObstacleData(int newSpawnType, int newObstacleType, Vector2 newScaleRange, int newSpawnQuantity, int newActiveQuantity) {
        spawnType = newSpawnType;
        type = newObstacleType;
        scaleRange = newScaleRange;
        spawnQuantity = newSpawnQuantity;
        activeQuantity = newActiveQuantity;
    }
      

}
