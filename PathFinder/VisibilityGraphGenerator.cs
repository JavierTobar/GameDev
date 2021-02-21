using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityGraphGenerator : MonoBehaviour
{
    public Terrain terrain;
    public LineRenderer line;
    public GameObject vertex;
    private static List<LineRenderer> edges;
    // we need to know where the LShapes are so that we can extend the visibility graph
    private static GameObject[] LShapeArray; 
    private static List<PathVertex> terrainVertices; // should include obstacles as well
    private static int arrayIndex; // index for LShapeArray
    // Start is called before the first frame update
    void Start()
    {
        edges = new List<LineRenderer>();
        terrainVertices = new List<PathVertex>();
        arrayIndex = 0;
        InitializeTerrainVertices();

        Invoke("DrawGraph", 0.2f);
    }

    // return vertices of our graph
    public static List<PathVertex> getVertices(){
        return terrainVertices;
    }

    public static void removePathVertex(PathVertex vertex){
        terrainVertices.Remove(vertex);
    }

    // adds new vertex to our data structure, this is where we add destination vertex of our agents
    public static void addPathVertex(PathVertex vertex){
        terrainVertices.Add(vertex);
    }
    // public static void addLShape(GameObject LShape){
    //     LShapeArray[arrayIndex++] = LShape;
    //     MeshFilter[] mfs = LShape.GetComponentsInChildren<MeshFilter>();
    //     List<Vector3> vList = new List<Vector3>();
    //     foreach (MeshFilter mf in mfs) {
    //         vList.AddRange (mf.mesh.vertices);
    //         }
    // }

    // we retrieve the neighbors of a vertex
    // note that if we had a large number of vertex or more time for assignment
    // i would've implemeneted this differently. e.g. vertex class that stores neighbors in a list
    // so that we don't recalculate every single time
    // make this return path vertices
    public static List<PathVertex> getNeighbors(PathVertex vertex){
        Physics.SyncTransforms();

        bool add;
        RaycastHit hit;
        List<PathVertex> neighbors = new List<PathVertex>();
        for (int i = 0; i < terrainVertices.Count; i++)
        {
            if (Physics.Linecast(vertex.pos, terrainVertices[i].pos, out hit) && hit.transform.tag == "Wall"){
                continue; // we skip if we hit a wall
            }
            add = true;
            // if collision with spherecast, we can still add if it wasn't with a wall or Lshape
            Vector3 direction = terrainVertices[i].pos - vertex.pos;
                float distance = Vector3.Distance(terrainVertices[i].pos, vertex.pos);

                RaycastHit[] hits = Physics.SphereCastAll(vertex.pos, 3.0f, direction, distance);
                // do linecast to see if we go over wall as well

                for (int j = 0; j < hits.Length; j++)
                {
                    if (hits[j].transform.tag == "LShape"){
                    // Debug.Log(hits[j].transform.tag);
                    add = false;
                    break; // we don't add this vertex
                    }
                }
                // finished loop and everything was okay
                if(add){
                    neighbors.Add(terrainVertices[i]);
                }
            }
        return neighbors;
    }

    public static void setNumberOfLShapes(int size){
        LShapeArray = new GameObject[size];
    }
    // O(n^2) kinda expensive but n = 20
    // we only cast ray that has length of the distance between the 2 nodes 
    private void DrawGraph(){
        RaycastHit hit;
        // spawns first vertex (edge case)
        Instantiate(vertex, terrainVertices[0].pos, Quaternion.identity);
        for (int i = 0; i < terrainVertices.Count-1; i++){
            Vector3 start = terrainVertices[i].pos;
            for (int j = i + 1; j < terrainVertices.Count; j++) {
                Vector3 destination = terrainVertices[j].pos;
                // spawns vertex 
                Instantiate(vertex, destination, Quaternion.identity);
                Vector3 direction = destination - start;
                float distance = Vector3.Distance(destination, start);
                if (Physics.SphereCast(start, 3.0f, direction, out hit, distance) && hit.transform.tag == "LShape"){

                    continue; // we don't draw edge if we collided with wall
                    }
                    // if not terrain, then we're out of bounds and we don't want to draw that edge

                if (Physics.Raycast(start,direction, out hit, distance) && hit.transform.tag != "Terrain"){
                    continue;
                }
                else {
                    // we draw edge if we didn't collide
                    LineRenderer temp = Instantiate(line);
                    temp.SetPosition (0, new Vector3(start.x, 0.75f, start.z));
                    temp.SetPosition (1, new Vector3(destination.x, 0.75f, destination.z));
                    edges.Add(temp);
                }

            }
        }   
    }




    // This is hard coded vertices of the graph (from step 1 requirement)
    // I've attached a PDF displaying the vertex order & coordinates as well
    // however we only want the reflex vertices
    private void InitializeTerrainVertices(){
        terrainVertices.Add(new PathVertex(new Vector3(-0.5f, 0.0f, 12.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(-0.5f, 0.0f, 27.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(-0.5f, 0.0f, 44.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(-0.5f, 0.0f, 64.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(-0.5f, 0.0f, 84.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(-0.5f, 0.0f, 96.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(13.5f, 0.0f, 119.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(33.5f, 0.0f, 119.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(53.5f, 0.0f, 119.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(73.5f, 0.0f, 119.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(79.5f, 0.0f, 102.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(79.5f, 0.0f, 77.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(79.5f, 0.0f, 60.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(79.5f, 0.0f, 45.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(79.5f, 0.0f, 31.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(79.5f, 0.0f, 11.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(60.5f, 0.0f, -0.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(35.5f, 0.0f, -0.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(19.5f, 0.0f, -0.5f)));
        terrainVertices.Add(new PathVertex(new Vector3(3.5f, 0.0f, -0.5f)));  
    }


}
