using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public GameObject wall; // for wall prefab
    public int xSize = 5; // rows for x
    public int ySize = 5; // columns for x
    public float wallLength = 1.0f; // Z value of the wall

    private Vector3 initialPosition;
    private GameObject wallHolder; // for better organization, all our walls will be inside this game object. This will also store ALL our walls.
    private Cell[] cells; // array our Cell object
    public int currentCell = 0; // cell tracker to know which cell we're in
    private int currentNeighbour = 0;
    private int totalCells;
    private int visitedCells = 0;
    public bool destroyedSolution = false; // to know when we destroyed the maze solution
    private bool init = false; // lets us know if we already initialized construction of our maze
    private List<int> lastCells; // used a stack to pop off when we backtrack
    private int backtrack = 0; // used for when if we didn't find any neighbours, we want to backtrack
    private int wallToDelete = 0;

    // System.Serializable makes our class appear in Unity's inspector
    // Cell class will be used to create our perfect maze with DFS
    [System.Serializable]
    public class Cell
    {
        public bool visited;
        // arbritary index assigned to our walls
        public GameObject north; // 1
        public GameObject south; // 2
        public GameObject east; // 3
        public GameObject west; // 4

    }


    // Start is called before the first frame update
    void Start()
    {
        CreateWalls(); // init
    }

    // Update is called once per frame
    void Update()
    {
        // check if solution got destroyed and set destroyedSolution to true if yes
    }

    void CreateCells()
    {
        lastCells = new List<int>();
        lastCells.Clear();
        totalCells = xSize * ySize;
        GameObject[] walls = new GameObject[wallHolder.transform.childCount];
        cells = new Cell[totalCells]; // row * column

        // retrieve all walls
        for (int i = 0; i < wallHolder.transform.childCount; i++)
        {
            walls[i] = wallHolder.transform.GetChild(i).gameObject;
        }

        // counters
        int southWall = (xSize + 1) * ySize;
        int northWall = ((xSize + 1) * ySize) + xSize;
        int rowNum = 0;

        // Assign wall to cell
        for (int i = 0; i < cells.Length; i++)
        {
            if (i % xSize == 0 && i != 0) rowNum++;

            // logic to get the cells (assume east = right, west = left, north = up, south = down)
            // recall logic of initial maze creation => all vertical walls created first and then horizontal walls
            cells[i] = new Cell();
            cells[i].east = walls[i + 1 + rowNum];
            cells[i].west = walls[i + rowNum];
            cells[i].north = walls[i + northWall];
            cells[i].south = walls[i + southWall];
        }
        Destroy(cells[Random.Range(0, 5)].south); // entrance
        Destroy(cells[Random.Range(cells.Length - 5, cells.Length)].north); // exit
        CreateMaze();
    }


    void CreateWalls()
    {
        GameObject tempWall;
        wallHolder = new GameObject();
        wallHolder.name = "Maze";

        // we need an init position (where the cliff is)
        // the init position is the bottom left wall
        initialPosition = new Vector3(243.0f, 113.0f, 221.0f); // point of canyon where the maze will be generated 
        Vector3 myPosition = initialPosition; // tracker for algorithm

        // Creating walls (X axis)
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j <= xSize; j++) // <= because we need one extra wall to close the maze 
            {
                myPosition = new Vector3(initialPosition.x + j * wallLength - wallLength / 2, 112.0f, initialPosition.z + i * wallLength - wallLength / 2);
                tempWall = Instantiate(wall, myPosition, Quaternion.identity) as GameObject; // spawn wall at default rotation
                tempWall.transform.parent = wallHolder.transform;
            }
        }

        // Creating walls (Y axis)
        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPosition = new Vector3(initialPosition.x + j * wallLength, 112.0f, initialPosition.z + i * wallLength - wallLength);
                tempWall = Instantiate(wall, myPosition, Quaternion.Euler(0.0f, 90.0f, 0.0f)) as GameObject;// we rotate it by 90 degrees
                tempWall.transform.parent = wallHolder.transform;
            }
        }
        // Once the walls are all created, we call CreateCells
        CreateCells();
    }

    void CreateMaze()
    {
        if (visitedCells < totalCells)
        {
            if (init)
            {
                getNeighbour();
                // checking if we haven't visited neighbour and if we have visited the current cell, if yes, we delete the wall
                if (cells[currentNeighbour].visited == false && cells[currentCell].visited == true)
                {
                    DeleteWall();
                    cells[currentNeighbour].visited = true; // so we don't revisit it 
                    visitedCells++; // incremented bc we just visited a new cell
                    lastCells.Add(currentCell); // adding to our "stack" so we can revisit later 
                    currentCell = currentNeighbour;

                    if (lastCells.Count > 0)
                    {
                        backtrack = lastCells.Count - 1; // we reset our backtrack value
                    }
                }
            }
            else // we pick our current cell
            {
                init = true; // i.e. we don't need to pick a random cell again, we already started building our maze
                currentCell = Random.Range(0, totalCells); // pseudo randomly picking a cell to start our maze from
                // increment currentcell after asigning true to its value 
                cells[currentCell].visited = true;
                visitedCells++;
            }
            Invoke("CreateMaze", 0.0f);
        }
    }

    // Returns a random neighbour (for DFS)
    void getNeighbour()
    {
        int neighboursFound = 0;
        int[] neighbours = new int[4]; // there can only be 4 neighbours at most (top, bottom, left, right). Diagnonals are NOT neighbours
        int[] connectingWall = new int[4];
        int corner = (currentCell + 1) / xSize; // Checking if we're on the last element on x axis
        corner -= 1;
        corner *= xSize;
        corner += xSize;


        // check if east wall
        if (currentCell + 1 < totalCells && currentCell + 1 != corner)
        {
            if (cells[currentCell + 1].visited == false)
            {
                // increment neighboursFound after assigning value
                neighbours[neighboursFound] = currentCell + 1;
                connectingWall[neighboursFound] = 3; // chosen index to represent east
                neighboursFound++;
            }
        }

        // check if west wall
        if (currentCell - 1 >= 0 && currentCell != corner)
        {
            if (cells[currentCell - 1].visited == false)
            {
                // increment neighboursFound after assigning value
                neighbours[neighboursFound] = currentCell - 1;
                connectingWall[neighboursFound] = 4; // chosen index to represent west
                neighboursFound++;

            }
        }

        // check if north wall
        if (currentCell + xSize < totalCells)
        {
            if (cells[currentCell + xSize].visited == false)
            {
                // increment neighboursFound after assigning value
                neighbours[neighboursFound] = currentCell + xSize;
                connectingWall[neighboursFound] = 1; // chosen index to represent north
                neighboursFound++;

            }
        }

        // check if south wall
        if (currentCell - xSize >= 0) // >= 0 because it's a bottom cell
        {
            if (cells[currentCell - xSize].visited == false)
            {
                // increment neighboursFound after assigning value
                neighbours[neighboursFound] = currentCell - xSize;
                connectingWall[neighboursFound] = 2; // chosen index to represent south
                neighboursFound++;

            }
        }

        if (neighboursFound != 0) // i.e. if we found a neighbour
        {
            int index = Random.Range(0, neighboursFound);
            currentNeighbour = neighbours[index];
            wallToDelete = connectingWall[index];
        }

        // executed if neighbour wasn't found 
        else
        {
            if (backtrack > 0)
            {
                // decrement backtrack once assigning is done
                currentCell = lastCells[backtrack--];
            }
        }
    }

    void DeleteWall()
    {
        // switch statement to delete the chosen wall
        switch (wallToDelete)
        {
            case 1:
                Destroy(cells[currentCell].north);
                break;
            case 2:
                Destroy(cells[currentCell].south);
                break;
            case 3:
                Destroy(cells[currentCell].east);
                break;
            case 4:
                Destroy(cells[currentCell].west);
                break;
        }
    }



}
