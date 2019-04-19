using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularObstacle : Obstacle {

    //MUST BE MANUALLY SET TO MATCH OBSTACLE TYPE INDEX IN CUBEFACTORY ARRAY
    public int obstacleType = 0;

    public Vector2 scaleRangeType = new Vector2(50, 300);

    // 0 for random spawning
    public int spawnLocationType = 0;
    public int standardSpawnQuantity = 200;
    public int activeQuantityType = 100;


    private void Awake()
    {
        SetObstacleData(spawnLocationType, obstacleType, scaleRangeType, standardSpawnQuantity, activeQuantityType);
    }


}
