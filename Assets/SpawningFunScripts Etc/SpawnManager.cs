using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public Obstacle prefab;
    public CubeFactory cubeFactory;
    /*
    public List<CubeShape> cubeShapes;
    */

    public int spawnCount = 0;

    Obstacle[] obstacleData;

    




    //This is spawndata and instance data stuff is dated delete when other stuff is finished
    public struct SpawnData {
        public int obstacleType;
        //can be changed to a range later for random quantity
        public int spawnQuantity;
        public Obstacle obstacle;
    }

    SpawnData instanceData;
    


    public int spawnQuantity = 50;

    //for getting cell and spawn area size -- could be any of 3 terrain tiles since they all should be the same
    public int terrainXYSize = 1500;


    private void Start()
    {
        
    }



    public void Spawn(Vector3 tileXYZ, Obstacle instance) 
    {

        int tileSize = terrainXYSize;
        if (instance.spawnType == 0) 
        {
            RandomSpawn(tileXYZ, tileSize, instance);
        }
        else
        {
            Debug.Log("there is no integer that matches up to that spawn location type yet, make sure new spawnlocation type information is everywhere it should be");
        }
    }


    public void RandomSpawn(Vector3 tileXYZ, int tileSize, Obstacle instance) 
    {
        //generating random position based on tile size and position
        //  NEED TO CHANGE Y ALGORITHM
        Vector3 position = new Vector3(Random.Range(tileXYZ.x, tileXYZ.x + tileSize), Random.Range(tileXYZ.y, tileXYZ.y + tileSize),
                        Random.Range(tileXYZ.z, tileXYZ.z + tileSize));
        //setting position to random position
        instance.position = position;
        

        //standard random rotation
        instance.rotation = Random.rotation;

        //random scale based on scaleRange field from obstacle type
        float scale = Random.Range(instance.scaleRange.x, instance.scaleRange.y);
        instance.scale = new Vector3(scale, scale, scale);


        instance.UpdatePosition();

    }




    //not used yet
    void BeginNewGame() {
        //this code is if we are actually destroying the game objects -- were not were pooling them and reusing 
        /*
        for (int i = 0; i < itemsSpawned.Count; i++)
        {
            Destroy(itemsSpawned[i].gameObject);
        }
        */


    }


}
