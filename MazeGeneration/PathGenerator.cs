using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{

    public GameObject floorTile; // for wall prefab
    public GameObject ammo;
    public int maxAmmo; // max ammo that we can have in the game
    int counter; // we spawn ammo every 10 tiles
    private Vector3 currentPosition;
    private int directionPicker;
    private int tilesToTravel;
    private GameObject tileHolder; // This will store ALL our path tiles
    private GameObject ammoHolder; // This will store ALL our ammo

    bool xTravelCap = false; // let's us know if we can still travel in X direction
    bool zTravelCap = false; // let's us know if we can still travel in Z direction
    // Start is called before the first frame update
    void Start()
    {
        CreatePath();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // The path generation works as following:
    // From the start point, we know that we have 185 tiles we can play with in the X direction
    // From the start point, we know that we have 150 tiles we can play with in the Z direction
    // we will randomly pick a direction and subtract from the available tiles that we have in that direction 
    // while still respecting the constraints, i.e. not going out of bonds, not making loops
    // our path will stop when we reach the "maze zone" which is at 190z and 250x. 
    // i.e. when we reach this zone, we assume that player can figure out where the maze is since it's only a few tiles away and fully visible
    // we can move in positive X direction and positive Z direction. This simplifies the algorithm AND respects the assignment constraints
    // BY DEFINITION UNICURSAL PATH DOESN'T NEED TO FILL UP THE WHOLE TERRAIN, SO WE WILL ONLY FILL HALF TO SIMPLIFY THE ALGORITHM WHILE STILL RESPECTING
    // THE DEFINITION OF UNICURSAL PATH
    void CreatePath()
    {
        currentPosition = new Vector3(65.0f, 112.0f, 40.0f); // point of terrain where the path will be generated 
        Vector3 myPosition = currentPosition; // tracker for algorithm
        GameObject tempTile;
        tileHolder = new GameObject();
        tileHolder.name = "TilePath";
        ammoHolder = new GameObject();
        ammoHolder.name = "Ammo";
        counter = 0;
        int xLimit = 250; // max X tile
        int zLimit = 190; // max Z tile
        int maxAmmo = 8;
        // FOR LOOP FOR CODE LOGIC BELOW : 

        // i.e. while we still haven't reached our destination, we keep building the path
        while (!zTravelCap || !xTravelCap)
        {
            // Pick if we want to travel in X or Y direction
            directionPicker = Random.Range(0, 2); // 0 = X, Z = 1

            // to optimize our algorithm we will travel at least 5 tiles in the given direction and a maximum of 20 tiles 
            // (design decision to make the maze more realistic looking too)
            tilesToTravel = Random.Range(5, 21);

            // if we're travelling in X direction
            if (directionPicker == 0 && !xTravelCap)
            {
                // check if we reach the limit of X tiles
                if (tilesToTravel + currentPosition.x >= xLimit)
                {
                    xTravelCap = true;
                    tilesToTravel = xLimit - (int)currentPosition.x; // i.e. we travel to X's limit and set xTravelCap to True
                }
                for (int i = 0; i < tilesToTravel; i++)
                {
                    counter++;
                    if (counter >= 25)
                    {
                        spawnAmmo();
                    }
                    currentPosition = new Vector3(currentPosition.x + 1, currentPosition.y, currentPosition.z);
                    tempTile = Instantiate(floorTile, currentPosition, Quaternion.identity);
                    tempTile.transform.parent = tileHolder.transform;
                }
            }
            // else we're travelling in Z direction
            if (directionPicker == 1 && !zTravelCap)
            {
                // check if we reach the limit of X tiles
                if (tilesToTravel + currentPosition.z >= zLimit)
                {
                    zTravelCap = true;
                    tilesToTravel = zLimit - (int)currentPosition.z; // i.e. we travel to X's limit and set xTravelCap to True
                }
                for (int i = 0; i < tilesToTravel; i++)
                {
                    counter++;
                    if (counter >= 10)
                    {
                        spawnAmmo();
                    }
                    currentPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + 1);
                    tempTile = Instantiate(floorTile, currentPosition, Quaternion.identity);
                    tempTile.transform.parent = tileHolder.transform;
                }
            }
        }
    }

    // spawns ammo
    void spawnAmmo()
    {
        if (maxAmmo >= 8)
        {
            return;
        }
        maxAmmo++;

        GameObject tempAmmo;

        counter = 0; // reset the counter
        Vector3 floatingPosition = new Vector3(currentPosition.x, currentPosition.y + 1, currentPosition.z);
        tempAmmo = Instantiate(ammo, floatingPosition, Quaternion.identity);
        tempAmmo.transform.parent = ammoHolder.transform;
    }



}
