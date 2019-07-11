using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour {


	//heightmap settings
	public float maxHeight = 1f;
	public float noiseScale = .1f;
	public bool useOctaves = true;
	public int octaves = 2;

	//used to calculate frequency -- octave detail level
	public float lacunarity = 2f;
	//controls how strong effect of octaves is
	public float persistance = .5f;

	float xNoise = 0;
	float yNoise = 0;
	public Vector2 seedXY;

	public TerrainType[] regions;

	  

	// Use this for initialization
	void Awake () {
		//needs to be awake so when constructor is called in MeshGen Start() it actually gets the new random seed
		randomNoise ();
	}
	


	
	void randomNoise (){
		seedXY.x = Random.Range (0, 1000);
		seedXY.y = Random.Range (0, 1000);
	}

	[System.Serializable]
	public struct TerrainType {
		public string name;
		public float height;
		public Color color;

	}
}
