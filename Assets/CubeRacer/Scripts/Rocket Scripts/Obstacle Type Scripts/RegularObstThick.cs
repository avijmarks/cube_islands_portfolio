using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularObstThick : Obstacle {

    //MUST BE MANUALLY SET TO MATCH OBSTACLE TYPE INDEX IN CUBEFACTORY ARRAY
    public int obstacleType = 2;

    public Vector2 scaleRangeType = new Vector2(50, 125f);

    //0 for random spawning
    public int spawnLocationType = 0;
    public int standardSpawnQuantity = 150;
    public int activeQuantityType = 40;

    private void Awake()
    {
        SetObstacleData(spawnLocationType, obstacleType, scaleRangeType, standardSpawnQuantity, activeQuantityType);

    }

}

