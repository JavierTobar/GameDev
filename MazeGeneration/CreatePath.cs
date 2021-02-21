using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class CreatePath : MonoBehaviour
{

    public int endPoint;
    public TerrainLayer terrainLayer;
    public Terrain terrain;
    // Start is called before the first frame update
    void Start()
    {
        CreateRandomPath();
    }

    // Creates random unicursal path to the start of the maze (endPoint coordinates)
    void CreateRandomPath()
    {
        Vector3 x = terrain.terrainData.size;
        Debug.Log(x);
        //  tilemap.ClearAllTiles();

        //   terrain.terrainData.
    }

    // Update is called once per frame
    void Update()
    {

    }
}
