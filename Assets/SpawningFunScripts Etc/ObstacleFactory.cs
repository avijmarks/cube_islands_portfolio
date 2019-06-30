using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFactory : MonoBehaviour {
    /// <summary>
    ///Uses ObstacleTypeData class and constructor to create ObstacleData list who's indices allow access (for changing difficulty and this code) 
    ///Contains spawning location types and processes used to "spawn" (move from pool location to a type-location) obstacles
    ///To add new obstacle add new constructor in SetTypeData(), add a public reference to prefab (obstacle inherited script on it), and add to
    ///obstacleData.add() the new constructed version of ObstacleTypeData class
    /// </summary>

    public class ObstacleTypeData : MonoBehaviour
    {
        public int index;
        public Vector2 scaleRange;
        public int spawnLocationIndex;
        public int spawnQuantity;
        public int activeQuantity;
        public Obstacle prefab;

        public ObstacleTypeData (int _index, Vector2 _scaleRange, int _spawnLocationIndex, int _spawnQuantity, int _activeQuantity, Obstacle _prefab)
        {
            index = _index;
            scaleRange = _scaleRange;
            spawnLocationIndex = _spawnLocationIndex;
            spawnQuantity = _spawnQuantity;
            activeQuantity = _activeQuantity;
            prefab = _prefab;

        }
    }

    ObstacleTypeData regular_Obstacle;
    ObstacleTypeData medium_Obstacle;
    ObstacleTypeData thick_Obstacle;
    public Obstacle regularPrefab;
    public Obstacle mediumPrefab;
    public Obstacle thickPrefab;

    //array containing class obstacletypedata/prefabs 
    public List<ObstacleTypeData> obstacleData;

    //used to make sure the SetTypeData(); has been called (don't want to mess around with making sure this start function happens before other ones)
    bool typeDataSet = false;

    //for terrain spawn area size(should be same for all tiles)
    public int terrainXYSize = 1000;

    //random seed for perlin spawn pattern
    Vector3 seedXY;
    
    struct SpawnData {
        Vector3 position;
        Quaternion rotation;
        Vector3 scale;
    }

     private void SetTypeData (){
        //CONTRUCTORS FOR OBSTACLE TYPES GO HERE *MAKE SURE TO ADD THEM TO obstacleData
        
        regular_Obstacle = new ObstacleTypeData(0, new Vector2(20f, 120f), 0, 100, 25, regularPrefab);
        medium_Obstacle = new ObstacleTypeData(0, new Vector2(4f, 7f), 1, 75, 20, mediumPrefab);
        thick_Obstacle = new ObstacleTypeData(0, new Vector2(20f, 50f), 0, 75, 15, thickPrefab);
        

        obstacleData = new List<ObstacleTypeData>();
        obstacleData.Add(regular_Obstacle);
        obstacleData.Add(medium_Obstacle);
        obstacleData.Add(thick_Obstacle);

        randomNoise(); //getting a random noise xyz seed for perlin noise spawning
    }


    public Obstacle[][] GetObstacleArray ()
    {
        //making sure we have type data
        if (typeDataSet == false) {
            SetTypeData();
            typeDataSet = true;
        }

        //initializing array
        Obstacle[][] obstaclesArray = new Obstacle[obstacleData.Count][];
        for (int i = 0; i < obstacleData.Count; i++){
            obstaclesArray[i] = new Obstacle[obstacleData[i].spawnQuantity];
        }

        //basically same code from instantiateobstacles below (maybe rename this function that when said and done)
        for (int i = 0; i < obstacleData.Count; i++)
        {
            for (int j = 0; j < obstacleData[i].spawnQuantity; j++)
            {
                //instantiating an obstacle of type i
                Obstacle instance = Instantiate(obstacleData[i].prefab);

                //putting the obstacle instance we just created in the array of obstacles of that type (for access by spawn manager)
                obstaclesArray[i][j] = instance;
                instance.TypeIndex = i;
                ObstacleInPoolLocation(instance);
            }
        }

        return obstaclesArray;
    }

    public void ObstacleInPoolLocation(Obstacle instance) 
    {
        //fix this updateposition BS once all the other spagetti is figured out
        instance.position = new Vector3(transform.position.x, (transform.position.y - 1000f), transform.position.z);
        instance.active = false;
        instance.UpdatePosition();
    }




    public void SetObstaclePositions(Vector3 chunkPosition, Obstacle[][] chunkObstacles) 
    {

        //main iteration through obstaclereference array lists in cube factory -- setting the position of every object we have spawned, whether active or
        for (int i = 0; i < obstacleData.Count; i++)
        {
            //handling case the set active amount of obstacles is greater than base number spawned in pool -- for when perfecting difficulty algorithm
            if (obstacleData[i].spawnQuantity < obstacleData[i].activeQuantity) {
                obstacleData[i].activeQuantity = obstacleData[i].spawnQuantity;
                Debug.Log("active amount of obstacles exceeded base spawn pool");
            }
            for (int j = 0; j < chunkObstacles[i].Length; j++)
            {
                //sets only active amount as obstacles, sending rest to pool
                if (j < obstacleData[i].activeQuantity)
                {
                    Spawn(chunkPosition, chunkObstacles[i][j], i);
                } 
                else 
                {
                    ObstacleInPoolLocation(chunkObstacles[i][j]);
                }
            }
        }

    }



    //decision making for spawn location type -- functions for each type further below
    public void Spawn(Vector3 tileXYZ, Obstacle instance, int obstacleType) 
    {
        
        instance.active = true;
        int tileSize = terrainXYSize;
        if (obstacleData[obstacleType].spawnLocationIndex == 0) 
        {
            RandomSpawn(tileXYZ, tileSize, instance, obstacleType);
        }
        else if (obstacleData[obstacleType].spawnLocationIndex == 1)
        {
            PerlinSpawn(tileXYZ, tileSize, instance, obstacleType);
        }
        else{
            Debug.Log("there is no integer that matches up to that spawn location type, make sure new spawnlocation type information is everywhere it should be");
        }
    }


    public void RandomSpawn(Vector3 tileXYZ, int tileSize, Obstacle instance, int obstacleType) 
    {
        //generating random position based on tile size and position
        //  NEED TO CHANGE Y to reflect curved boundary
        Vector3 position = new Vector3(Random.Range(tileXYZ.x, tileXYZ.x + tileSize), Random.Range(tileXYZ.y, tileXYZ.y + 340f),
                        Random.Range(tileXYZ.z, tileXYZ.z + tileSize));
        //setting position to random position
        instance.position = position;
        
        //standard random rotation
        instance.rotation = Random.rotation;

        //random scale based on scaleRange field from obstacle type
        float scale = Random.Range(obstacleData[obstacleType].scaleRange.x, obstacleData[obstacleType].scaleRange.y);
        instance.scale = new Vector3(scale, scale, scale);

        instance.UpdatePosition();
    }

    public void PerlinSpawn(Vector3 tileXYZ, int tileSize, Obstacle instance, int obstacleType)
    {
        bool testBool = false;
        Vector3 position = Vector3.zero;
        while (testBool == false){
            Vector3 testLocation = new Vector3(Random.Range(tileXYZ.x, tileXYZ.x + tileSize), Random.Range(tileXYZ.y, tileXYZ.y + 340f),
                        Random.Range(tileXYZ.z, tileXYZ.z + tileSize));
            float testValue = Mathf.Pow(Noise(testLocation, .007f), 1.5f);
            if (testValue > Random.Range(.42f, .99f)){
                position = testLocation;
                testBool = true;
            }
        }

        

        
        //setting position to random position
        instance.position = position;
        
        //standard random rotation
        instance.rotation = Random.rotation;

        //random scale based on scaleRange field from obstacle type
        float scale = Random.Range(obstacleData[obstacleType].scaleRange.x, obstacleData[obstacleType].scaleRange.y);
        instance.scale = new Vector3(scale, scale, scale);

        instance.UpdatePosition();
    }

    public float Noise (Vector3 location, float frequency){
        //move later -- for messin around
        bool useOctaves = true;
        int numberOfOctaves = 1;
        float persistance = .37f;
        float lacunarity = 4.5f;

        location = new Vector3(location.x + seedXY.x, location.y + seedXY.y, location.z + seedXY.z);
        float XY = Mathf.PerlinNoise(location.x * frequency, location.y * frequency);
        float XZ = Mathf.PerlinNoise(location.x * frequency, location.z * frequency);
        float YZ = Mathf.PerlinNoise(location.y * frequency, location.z * frequency);
        
        float YX = Mathf.PerlinNoise(location.y * frequency, location.x * frequency);
        float ZX = Mathf.PerlinNoise(location.z * frequency, location.x * frequency);
        float ZY = Mathf.PerlinNoise(location.z * frequency, location.y * frequency);
        


        float XYZ = (XY + XZ + YZ) / 3f;

        if (useOctaves) {
            for (int octaveNumber = 1; octaveNumber <= numberOfOctaves; octaveNumber++){
                float octaveFreq = frequency * Mathf.Pow(lacunarity, octaveNumber);
                float octavePersistance = Mathf.Pow(persistance, octaveNumber);

                XY = Mathf.PerlinNoise(location.x * octaveFreq, location.y * octaveFreq);
                XZ = Mathf.PerlinNoise(location.x * octaveFreq, location.z * octaveFreq);
                YZ = Mathf.PerlinNoise(location.y * octaveFreq, location.z * octaveFreq);
                
                YX = Mathf.PerlinNoise(location.y * octaveFreq, location.x * octaveFreq);
                ZX = Mathf.PerlinNoise(location.z * octaveFreq, location.x * octaveFreq);
                ZY = Mathf.PerlinNoise(location.z * octaveFreq, location.y * octaveFreq);

                float octaveValue = (XY + XZ + YZ + YX + ZX + ZY) / 6f;

                //standardize so that diff octave levels can be tested against one another in terms of patterns they produce w/o changing cutoff
                XYZ = XYZ - (.5f * persistance);
                

                XYZ += (octaveValue * persistance);
                
            }
        }
        return XYZ;
    }

    void randomNoise (){
		seedXY.x = Random.Range (0, 1000);
		seedXY.y = Random.Range (0, 1000);
        seedXY.z = Random.Range (0, 1000);
	}
}

