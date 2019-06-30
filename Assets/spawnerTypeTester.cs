using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerTypeTester : MonoBehaviour {

	public ObstacleFactory obstacleFactory;

	Obstacle[][] obstacles;


	// Use this for initialization
	void Awake () {
		obstacles = obstacleFactory.GetObstacleArray();
	}
	
	// Update is called once per frame
	void Start () {
		Vector3 offsetPos = new Vector3(transform.position.x - 250f, transform.position.y, transform.position.z - 250f);
		obstacleFactory.SetObstaclePositions(offsetPos, obstacles);
	}
}
