using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunkManager : MonoBehaviour {
    //NEEDS TO GEN HEIGHTS ON SET
    //make sure this script only sets positional data not rotational data, height gen will do the rest. 

    //terrain structs that contain position and reference to generation script
    public TerrainData Terrain1 = new TerrainData();
    public TerrainData Terrain2 = new TerrainData();
    public TerrainData Terrain3 = new TerrainData();

    [System.Serializable]
    public struct TerrainData {
        public GameObject TerrainPosition;
        public GenDiscreteNormMesh Generator;
        public MeshCollider collider;
        public MeshFilter mesh;

    }

    //heightmap generator reference as needed
    public NoiseGenerator heightMapGenerator;

    //pool
    public Queue<TerrainData> Pool = new Queue<TerrainData>();




	void Start () {
        StartState();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void StartState() {
        //pooling terrain not in use
        Pool.Enqueue(Terrain2);
        Pool.Enqueue(Terrain3);

        //setting first terrain chunk's position and pooling
        Terrain1.TerrainPosition.transform.position = new Vector3(0f, 0f, 500);
        Terrain1.Generator.MakePlane();
        Terrain1.Generator.GenObstacles(new Vector3(0f, 0f, 500));
        Pool.Enqueue(Terrain1);
    }

    public void NewChunk(Vector3 location) {
        //THIS SHOULD EVENTUALLY RETURN A HEIGHTMAP TO THE TILE MANAGER FOR OBSTACLE USE
        //for creation of this function -- delete in 5 min -- PRETEND GETTING LOCATION 0 0 1500 (CHUNK B)
        TerrainData NewTerrain = Pool.Dequeue();
        NewTerrain.TerrainPosition.transform.position = location;
        NewTerrain.Generator.MakePlane();
        NewTerrain.Generator.GenObstacles(location);
        Pool.Enqueue(NewTerrain);
        /*
        NewTerrain.collider.sharedMesh = null;
        NewTerrain.collider.sharedMesh = NewTerrain.mesh.mesh;
        */       
    }

    public void setTerrainCollider(TerrainData currentTerrain) {
         
    }
}
