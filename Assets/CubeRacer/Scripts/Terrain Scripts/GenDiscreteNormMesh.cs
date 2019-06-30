using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


[RequireComponent(typeof(MeshRenderer))]
public class GenDiscreteNormMesh : MonoBehaviour {
	
	Mesh mesh;
	MeshRenderer meshRenderer;
	public NoiseGenerator noiseGenerator;

	//mesh arrays
	Vector3[] vertices;
	int[] triangles;
	Vector2[] uvs;
	Color[] colorMap;

	//SETTINGS
	public float cellSize = 1;
	public int sizeX = 3;
	public int sizeY = 3;

    public bool testing = false;

	//not used yet
	public float cellLoadTime = .1f;

	//array of Obstacle arrays containing references to actual obstacles above terrain tile -- sent to ObstacleFactory for spawning
	Obstacle[][] obstacles;
	public ObstacleFactory obstacleFactory;

	Thread threadForTerrainGen;

	



	//assigning mesh
	void Awake () {
		mesh = GetComponent<MeshFilter> ().mesh;
		meshRenderer = GetComponent<MeshRenderer> ();
		obstacles = obstacleFactory.GetObstacleArray();
	}


	// Use this for initialization
	void Start () {
        if (testing) {
            MakePlane();
        }
        //replace with function called when plane is spawned/moved
    }

	public void GenObstacles (Vector3 location)
	{
		Vector3 obstacleFactoryLocation = new Vector3(location.x - 500f, location.y, location.z-500f);
		//locations used by terrain chunks are center of tile, whereas spawner is currently min x/z values (bottom left corner)
		obstacleFactory.SetObstaclePositions(obstacleFactoryLocation, obstacles);
	}

	public void MakePlane(){
		float mapScale = transform.localScale.x; 
		Vector2 chunkXY = new Vector2 (transform.position.x, transform.position.z); 
		threadForTerrainGen = new Thread(() => ThreadedTerrainGen(mapScale, chunkXY));
		threadForTerrainGen.Start();
		StartCoroutine("CheckForThreadComplete");
		

        
	}
	
	IEnumerator CheckForThreadComplete (){
		//can get rid of this once make a List<action> functions to run on main thread, thread pooling system on gamemanager singleton
		//put update mesh in the function to call on main thread
		while (threadForTerrainGen.IsAlive){

			yield return null;
		}
		UpdateMesh();
	}

	public void ThreadedTerrainGen (float mapScale, Vector2 chunkXY){
				float globalCellSize = cellSize * mapScale;
		int gridSize = (sizeX / (int)cellSize) * (sizeY / (int)cellSize);

		vertices = new Vector3[gridSize * 6];
		triangles = new int[vertices.Length];
		uvs = new Vector2[vertices.Length];
		colorMap = new Color[gridSize]; 

        for (int w = 0; w < triangles.Length; w++) {
			//setting triangles -- super easy because vertices are produced in order & triangle.length = vertices.length
			triangles [w] = w;
		}

		//count trackers for vertex and quad respectively
		int v = 0;
		int quad = 0;



		for (float y = 0; y < sizeY; y += cellSize) {
			for (float x = 0; x < sizeX; x += cellSize) {
				


				//get heights for the current quad

				float[] quadHeights = noiseGenerator.QuadHeights((x * mapScale), (y * mapScale), chunkXY, globalCellSize); //noise gen here

				//sets vertices w/ heights -- these are set in order used in triangles
				vertices[v] = new Vector3(x, quadHeights[0], y);
				vertices [v + 1] = new Vector3 (x, quadHeights[1], (y + cellSize));
				vertices [v + 2] = new Vector3 ((x + cellSize), quadHeights[2], y);
				vertices [v + 3] = vertices [v + 2];
				vertices [v + 4] = vertices [v + 1];
				vertices [v + 5] = new Vector3 ((x + cellSize), quadHeights[3], (y + cellSize));

				//like vertices except vector 2 and identified by XY percent of width/length
				uvs [v] = new Vector2 ((float)x / sizeX, (float)y / sizeY);
				uvs [v + 1] = new Vector2 ((float)x / sizeX, (float)(y + cellSize) / sizeY);
				uvs [v + 2] = new Vector2 ((float)(x + cellSize) / sizeX, (float)y / sizeY);
				uvs [v + 3] = uvs [v + 2];
				uvs [v + 4] = uvs [v + 1];
				uvs [v + 5] = new Vector2 ((float)(x + cellSize)/ sizeX, (float)(y + cellSize) / sizeY);

				//finding color of quad and adding to color array
				for (int i = 0; i < noiseGenerator.regions.Length; i++) {
					if (quadHeights [0] <= noiseGenerator.regions [i].height) {
						
						colorMap [quad] = noiseGenerator.regions [i].color;
						
						break;

					}
				}
				//counters
				v += 6;
				quad++;
			}
		}
	}

	void UpdateMesh (){
		mesh.Clear ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		meshRenderer.material.mainTexture = TextureFromColorMap ();

		/*
		GenerateHeights ();
		*/
		mesh.RecalculateNormals ();
	}

	Texture2D TextureFromColorMap (){
		Texture2D texture = new Texture2D((sizeX / (int)cellSize), (sizeY / (int)cellSize));
		texture.SetPixels (colorMap);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		return texture;
	}


}
