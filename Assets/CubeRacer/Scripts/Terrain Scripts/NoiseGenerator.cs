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
	Vector2 seedXY;

	public TerrainType[] regions;

	  

	// Use this for initialization
	void Awake () {
		randomNoise ();
	}
	


	public float[] QuadHeights (float x, float y, Vector2 chunkXY, float globalCellSize){

		//+1s here will make the perlin scale remain consistent with an increase in gridcellsize currently, if want to change will need to feed scale into qheights function
		//keep in mind perlin noise has same value at integer coordinates
		//thus the noiseScale

		//height array and vertex float variables for summing octaves (order move vertically by rows on a quad -- bottom left is a)
		float[] heights = new float[4];



		//casting to floats (not sure if necessary--happened because didnt understand same value at int coords in perlin noise problem)
		xNoise = (float)x + chunkXY.x + seedXY.x;
		yNoise = (float)y + chunkXY.y + seedXY.y;

		float frequency;
		float amplitude;
		int vertTracker;

		//MAIN OCTAVE (Octave 0)
		vertTracker = 0;
		for (int quadX = 0; quadX < 2; quadX++) {
			for (int quadY = 0; quadY < 2; quadY++) {
				frequency = noiseScale;
				amplitude = maxHeight;
				//globalcellsize was mapscale
				//calculating 4 height values for quad being drawn | since this is first octave frequency and amplitude are unchanged (since ^ 0 is identity)
				float perlinValue = Mathf.PerlinNoise ((xNoise + (quadX * globalCellSize)) * frequency, (yNoise + (quadY * globalCellSize)) * frequency);

				//making value -.5 - .5


				//********** experiment with forumas here -- default is perlinval * 2 - 1, pow
				perlinValue = Mathf.Pow((perlinValue * -2 + 1f), 2f);

				heights [vertTracker] = perlinValue * amplitude;
				vertTracker++;
			}
		}
			

		if (useOctaves){
			for (int octaveNumber = 1; octaveNumber <= octaves; octaveNumber++) {
				vertTracker = 0;
				for (int quadX = 0; quadX < 2; quadX++) {
					for (int quadY = 0; quadY < 2; quadY++) {
						frequency = noiseScale * Mathf.Pow (lacunarity, octaveNumber);

						float perlinValue = Mathf.PerlinNoise ((xNoise + (quadX * globalCellSize)) * frequency, (yNoise + (quadY * globalCellSize)) * frequency);

						//*********** everything vheightratio
						float vHeightRatio = .1f;
						if (heights [vertTracker] > 0f) {
							vHeightRatio = heights [vertTracker] / maxHeight + .4f;
							if (vHeightRatio > 1) {
								vHeightRatio = 1f;
							}
						}
						amplitude = maxHeight * Mathf.Pow ((persistance * vHeightRatio), octaveNumber);
						perlinValue = perlinValue * 2 - 1;

						heights [vertTracker] += perlinValue * amplitude;

						vertTracker++;
					}
				}
			}
		}



		return heights;
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
