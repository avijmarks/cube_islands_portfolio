using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularObstacleThin : Obstacle
{

    //MUST BE MANUALLY SET TO MATCH OBSTACLE TYPE INDEX IN CUBEFACTORY ARRAY
    public int obstacleType = 0;

    public Vector2 scaleRangeType;

    // 0 for random spawning
    public int spawnLocationType = 0;
    public int standardSpawnQuantity = 200;
    public int activeQuantityType = 100;


    public Obstacle normalObstacle = new RegularObstacle();

    private void Awake()
    {
        SetObstacleData(spawnLocationType, obstacleType, scaleRangeType, standardSpawnQuantity, activeQuantityType);
    }

}
