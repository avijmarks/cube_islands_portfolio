using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFactory : MonoBehaviour {
    /// <summary>
    /// make cube types inherit from cubeshape.cs -- will make list/array in editor of all obstacle type scripts inheriting from cubeshape CALLED OBSTACLE TYPES
    /// obstacle types will contain e.g. regularthin, regularmedium, regularlarge, which will all be prefabs with regularobstacle.cs on them
    /// 
    /// 
    /// percentage of 
    /// </summary>
    [SerializeField]
    public Obstacle[] obstacleTypes;

    public List<Obstacle[]> obstacles;

    SpawnManager spawnManager = new SpawnManager();


   



    private void Start ()
    {
        
        //initializing arrays that active objects will be stored in
        //only needs to be done once (and can use arrays) because were not adding new types of obstacles after the beginning of the game
        obstacles = new List<Obstacle[]>();
        //TEMP LOCAL VARIABLE FOR INITIALIZING: creating an array of integers that represent target quantities for initializing obstacle pools 
        List<int> obstacleQuantities = new List<int>();

        //initializing activequantity arrays in this list, again only needs to be done once
        for (int i = 0; i < obstacleTypes.Length; i++)
        {
            obstacleTypes[i].UpdatePosition();
            //creating arrays for obstacle max quantities
            obstacles.Add(new Obstacle[obstacleTypes[i].spawnQuantity]);
            obstacleQuantities.Add(obstacleTypes[i].spawnQuantity);
           
        }

        //using initialization array to spawn obstacle object pools
        InstantiateObstacles(obstacleQuantities);
    }

    //for initialization of object pools (spawns them into the world at pool location on game startup)
    public void InstantiateObstacles(List<int> quantities)
    {

        for (int i = 0; i < obstacleTypes.Length; i++)
        {
            for (int j = 0; j < obstacleTypes[i].spawnQuantity; j++)
            {
                //instantiating an obstacle of type i
                Obstacle instance = Instantiate(obstacleTypes[i]);

                //making the obstacle instance we just created in the array of obstacles of that type (for access by spawn manager)
                obstacles[i][j] = instance;
                ObstacleInPoolLocation(instance);

            }
        }
    }


    public void ObstacleInPoolLocation(Obstacle instance) 
    {
        instance.position = new Vector3(transform.position.x, (transform.position.y - 1000f), transform.position.z);
        instance.UpdatePosition();
    }




    public void SetObstaclePositions() 
    {
       
        //getting the ACTIVE quantities from the obstacle types, these may have been changed by the difficulty algorithm
        int[] activeObstacleQuantities = new int[obstacleTypes.Length];
        for (int i = 0; i < activeObstacleQuantities.Length; i++)
        {
            activeObstacleQuantities[i] = obstacleTypes[i].activeQuantity;
        }
       

        //main iteration through obstaclereference array lists in cube factory -- setting the position of every object we have spawned, whether active or
        for (int i = 0; i < obstacleTypes.Length; i++)
        {
            Debug.Log("hello world");
            //handling case the set active amount of obstacles is greater than base number spawned in pool -- for when perfecting difficulty algorithm
            if (obstacleTypes[i].spawnQuantity < obstacleTypes[i].activeQuantity) {
                obstacleTypes[i].activeQuantity = obstacleTypes[i].spawnQuantity;
                Debug.Log("active amount of obstacles exceeded base spawn pool");
            }
            for (int j = 0; j < obstacles[i].Length; j++)
            {
                /// this looping works because obstacletypes is the array in cube factory with a size of the amount of types of obst we have. It has the references to obstacle types for 
                /// instantiation and therefore also gives us access to obstacle data

                if (j < obstacleTypes[i].activeQuantity)
                {
                    //sets only active amount as obstacles, sending rest to pool
                    spawnManager.Spawn(transform.position, obstacles[i][j]);

                } 
                else 
                {
                    ObstacleInPoolLocation(obstacles[i][j]);

                }


            }
            //
        }

    }


    public Obstacle[] GetObstacleData()
    {
        return obstacleTypes;
    }

   

}

