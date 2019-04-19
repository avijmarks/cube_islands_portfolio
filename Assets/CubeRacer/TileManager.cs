using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerInChunk { A, B, C, D }
public class TileManager : MonoBehaviour {

    //TILE DIRECTION -- WILL BE USED MORE LATER / NOT SET UP HERE (WILL BE BASED ON BOUNDARY TILE CHOICE) 012 = XYZ
    int direction = 2;

    int tilecount = 0;

    //BOUNDARY STUFF
    public BoundaryManager boundaryManager;
    //direction based position in tile
    public float positionInTile;
    //initialized at D so conditional is called in ChunkA
    public PlayerInChunk playerIsIn = PlayerInChunk.D;

    //TERRAIN CHUNK STUFF
    public TerrainChunkManager terrainManager;



   

    private void FixedUpdate()
    {
        positionInTile = Mathf.InverseLerp(boundaryManager.dimensionsCurrent.Start[direction], boundaryManager.dimensionsCurrent.End[direction], transform.position[direction]);

        //chunk position -- only for straight tiles will need to create seperate for turns and use condition of type of boundary to distinguish which to use
        //CHUNK A
        if (positionInTile < .25f) {
            if (playerIsIn != PlayerInChunk.A) {
                playerIsIn = PlayerInChunk.A;

                //Terrain Chuck B
                Vector3 location = boundaryManager.dimensionsCurrent.Start;
                location[direction] = location[direction] + 1500;
                terrainManager.NewChunk(location);
            }

        //CHUNK B
        } else if (positionInTile < .5f) {
            if (playerIsIn != PlayerInChunk.B)
            {
                playerIsIn = PlayerInChunk.B;

                //Terrain Chunk C
                Vector3 location = boundaryManager.dimensionsCurrent.Start;
                location[direction] = location[direction] + 2500;
                terrainManager.NewChunk(location);
            }

        //CHUNK C
        } else if (positionInTile < .75f) {
            if (playerIsIn != PlayerInChunk.C)
            {
                playerIsIn = PlayerInChunk.C;

                //Creating Next Boundary
                boundaryManager.NewBoundary();

                //Terrain Chunk D
                Vector3 location = boundaryManager.dimensionsCurrent.Start;
                location[direction] = location[direction] + 3500;
                terrainManager.NewChunk(location);
            }

        //CHUNK D
        } else if (positionInTile < .99) {
            if (playerIsIn != PlayerInChunk.D)
            {
                playerIsIn = PlayerInChunk.D;

                //Terrain Chunk A (Regardless of next Tile Boundary Type)
                Vector3 location = boundaryManager.dimensionsNext.Start;
                location[direction] = location[direction] + 500;
                terrainManager.NewChunk(location);
            }

        //START NEXT TILE
        } else {
            boundaryManager.PlayerInNextBoundary();
            tilecount++;
        }
        /*
        Debug.Log(playerIsIn + "position" + positionInTile);
        */       
    }

}
