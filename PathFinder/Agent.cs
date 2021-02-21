using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Agent : MonoBehaviour
{
    // this is for testing purposes if we want to manually enter a destination,
    // otherwise it's taken care of by AgentGenerator.cs
    public PathVertex destination;
    public int planningTime; // for our graph
    // public GameObject goalPrefab; // the square that will display the goal
    private int failedAttempts;
    private GameObject goal;
    public int totalFailedAttempts; // for stats analysis
    public int totalSucceededAttempts; // for stats analysis
    public int numOfPathsPlanned;
    private Color color;
    private bool walkToNearbyNeighbor; // lets us know if our agent needs to go to a vertex in our roadmap
    private static List<LineRenderer> edges;
    private PathVertex nearbyVertex; // nearby vertex that is part of the visibility graph
    private bool canReachGoal;
    private bool hasPathPlan; // to know if our agent currently has a plan or not
    private int i;
    private List<PathVertex> pathPlan;
    private Vector3 prevPosition; // to know if we're done moving or not


    // Start is called before the first frame update
    void Start()
    {
        totalFailedAttempts = planningTime = numOfPathsPlanned = totalSucceededAttempts = failedAttempts = 0;
        // randomly pick a color (two agents CAN have the same colour but this is extremely rare)

        color = UnityEngine.Random.ColorHSV();
        goal.GetComponent<Renderer>().material.color = color;
        GetComponent<Renderer>().material.color = color;

        nearbyVertex = getNearbyVertex();

        walkToNearbyNeighbor = false; // true when we collided, we randomly walk to nearby neighbor
        hasPathPlan = false;
        // canReachGoal = canReachDestination();
        setPathPlan(); // we start with a plan

    }

    public void setGoalSquare(GameObject pGoal){
        goal = pGoal;
    }

    public void setDestination(PathVertex pDestination){
        destination = pDestination;
    }

    // just to check if we can reach our goal from spawning point
    // edge case for spawning point
    // private bool canReachDestination(){
    //     float distance = Vector3.Distance(transform.position, destination.pos);

    //     Vector3 direction = destination.pos - transform.position;
    //     RaycastHit hitInfo;
    //     // currently can walk through air 
    //     if (Physics.SphereCast(transform.position, 3.0f, destination.pos, out hitInfo, distance)){
    //         // then we can move directly, we assume we won't collide with other agents 
    //         // if we can reach our goal
    //         Debug.Log("CanReachWillReturn " + hitInfo.transform.tag != "Wall" && hitInfo.transform.tag != "Agent" && hitInfo.transform.tag != "LShape");
    //         Debug.Log(hitInfo.transform.tag);
    //         return hitInfo.transform.tag != "Wall" && hitInfo.transform.tag != "Agent" && hitInfo.transform.tag != "LShape";
    //         }
    //         return false;
    //     }

    // This is will look at nearby vertices that are accessible from the agent's current position 
    // and return the nearest one
    private PathVertex getNearbyVertex(){
        List<PathVertex> allVertices = VisibilityGraphGenerator.getVertices();
        List<PathVertex> potentialVertices = new List<PathVertex>();

        RaycastHit hitInfo; 
        for (int i = 0; i < allVertices.Count; i++){
            Vector3 direction = allVertices[i].pos - transform.position;
            float distance = Vector3.Distance(transform.position, allVertices[i].pos);  
            if (Physics.SphereCast(transform.position, 3.0f, direction, out hitInfo, distance)){
                if (hitInfo.transform.tag != "Wall" && hitInfo.transform.tag != "LShape"){
                    potentialVertices.Add(allVertices[i]);
                }
            }
        }
        float min = float.MaxValue;
        int index = 0;
        for (int i = 0; i < potentialVertices.Count; i++){
            float dist = Vector3.Distance(transform.position, potentialVertices[i].pos);
            if (dist < min){
                min = dist;
                index = i;
            }
        }
        return potentialVertices[index];
    }


    // this method handles the situation when the agent cannot reach
    // it's current destination for whatever reason
    // will allow our agent to walk again
    private void handleCollision(){
        totalFailedAttempts++; // we almost increment this for the graph at the end
        // if we have already failed 3 times, then we find new destination
        if (failedAttempts == 3){
            // get new path
            reset(); // we reset 
        } else {
             
            // else we can still fail and keep same destination
            // get new path to same destination
            // wait between 100-500 ms
            Invoke("startWalking", Random.Range(0.1f, 0.5f));
        }
    }
    // we need these helper methods so we can wait the 100-500ms
    private void startWalking(){
        walkToNearbyNeighbor = true; // we set this to true to get a new path to our old destination
        hasPathPlan = false;
        failedAttempts++;
    }


     
    /* 
        HOW OUR PATH FINDING WORKS :
        First step : Sphere.Cast to check if our agent can reach the destination without needing to enter 
        visibility graph 'road map'. 
        If yes -> Move. Done. (Assuming no collision). 
        If not -> Since we just need a sensible path and not the most optimal, and since we need to pick a new 
        random sensible path when we collide/can't reach our destination anymore :
        1. Before we enter our road map, we will look at nearby vertices of our roadmap that our agent can reach.
        2. Our agent will move to of these vertices. (pseudo randomly picked)
        3. We run A* from this vertex to our destination vertex

        The path won't be optimal, but it's still sensible. We just move to a random nearby node to "randomize"
        the path.
     */

    // when we collide we stop moving
    // we also throw away out path plan and move the nearest vertex again
    void OnTriggerEnter(Collider collision){
        if (collision.transform.tag == "Agent"){
            Invoke("handleCollision", Random.Range(0.1f, 0.5f));
        }
    }

    // gets called when our agent reached our destination
    // we reset a bunch of variables and prepare to move to our next destination
    private void reachedDestination(){
        totalSucceededAttempts++; // need to keep track for our graph at the end
        Invoke("reset", Random.Range(0.2f, 1.0f)); // waiting period 
    }

    private void reset(){
        VisibilityGraphGenerator.removePathVertex(destination);
        destination = Terrain.getValidSquare();
        goal.transform.position = destination.pos;
        VisibilityGraphGenerator.addPathVertex(destination);
        // canReachGoal = canReachDestination();
        walkToNearbyNeighbor = false; 
        setPathPlan();
        i = 0;
    }

    private void setPathPlan(){
        
        numOfPathsPlanned++;
        i = 0; // we reset our index

        PathFinding pf = new PathFinding(transform.position, destination.pos);
        pathPlan = pf.pathFind();
        planningTime += pf.pathPlaningTime;
        // then we couldn't find path, maybe it's too close to obstacles 
        // i took care of it, but writing code to take care of it if it ever happens again (very rare)
        if (pathPlan == null){
            hasPathPlan = false;
            // we will consider it as a failure, i.e. replan, if doesn't work x3, then just find a new destination
            handleCollision();
        } else {
            // if path != null then we have a path
            hasPathPlan = true;
        }
    }


    // to know if our agent is still moving or not
    // if not moving = we reached our temporary destination or final destination
    private bool isNotMoving(){
        if (transform.position == prevPosition){
            return true;
        }
        prevPosition = transform.position;
        return false;
    }

    // Update is called once per frame
    void Update()
    {

            // to get to our roadmap
            // we only walk to neighbor if we collided
        if (walkToNearbyNeighbor){
            // Debug.Log(nearbyVertex);
            // to know length of our spherecast
            float distance = Vector3.Distance(transform.position, nearbyVertex.pos);
            Vector3 direction = nearbyVertex.pos - transform.position;
            RaycastHit hitInfo;
            // currently can walk through air 
            if (Physics.SphereCast(transform.position, 3.0f, direction, out hitInfo, distance)){

                // then we can move directly, we assume we won't collide with other agents 
                if (hitInfo.transform.tag != "LShape" && hitInfo.transform.tag != "Agent"){
                    transform.position = Vector3.MoveTowards(transform.position, nearbyVertex.pos, 25.0f * Time.deltaTime);
                }
            }
            // once we reached nearbyVertex we no longer walk to it
            if (isNotMoving()){
                walkToNearbyNeighbor = false;
                setPathPlan();
                }   
            }
            // if we have pathplan then we execute it 
            else if(hasPathPlan){
                transform.position = Vector3.MoveTowards(transform.position, pathPlan[i].pos, 35.0f * Time.deltaTime);
                if (transform.position == destination.pos){
                    hasPathPlan = false; // we've reached our destination
                    reachedDestination(); // resets and finds new destination
            }
                if (isNotMoving()){
                    i++;
                }
            }

// }

        
        }

        
    }

