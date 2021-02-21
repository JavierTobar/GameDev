using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{

    private List<PathVertex> fringe;
    private List<PathVertex> exploredVertices;
    private Vector3 start;
    private Vector3 destination;
    public int pathPlaningTime;

    public PathFinding(Vector3 pStart, Vector3 pDestination){
        pathPlaningTime = 0;
        start = pStart; 
        destination = pDestination; 
        fringe = new List<PathVertex>();
        // exploredVertices = new List<PathVertex>();

    }


    // Runs A* on our graph : f(n) = g(n) + h(n)
    // g(n) : distance from the source
    // h(n) : estimate of the actual cost to get to the destination, it will be calculated from Vector3.Distance()
    // our destination node is added into our graph but it isnt displayed
    // returns the order of nodes that we need to walk through to reach our destination
    public List<PathVertex> pathFind(){
        System.DateTime before = System.DateTime.Now;
        System.DateTime after;

        PathVertex startVertex = new PathVertex(start);
        startVertex.gCost = 0.0f; // by definition
        startVertex.fCost = Vector3.Distance(startVertex.pos, destination);
        startVertex.setFCost();
        fringe.Add(startVertex);


        List<PathVertex> pathToTake = new List<PathVertex>();
        // here we set the g cost to ALL our nodes to be infinity and we calculate the fcost
        List<PathVertex> allNodes = VisibilityGraphGenerator.getVertices();

        foreach (PathVertex v in allNodes){
            v.parentVertex = null;
            v.gCost = float.MaxValue;
            v.setFCost();
        }

        // PSEUDO CODE FROM CLASS (DEAR TA, YOU DON'T NEED TO READ THIS, IT'S JUST FOR LOGIC/DEBUGGING)
        // we might need to see node more than once
        // g(start) = 0
        // f(start) = g(start) + h(start) = h(start)
        // g(n =/= start) = infinity, thus f(n =/= start) = infinity also
        // fringe = {start} we start from the frige
        // we keep going, while (fringe is not empty set):
        // c = find node in the fringe that has the smallest f function (initially its start)
        // if c == destination => done
        // else : look at all neighbours n of C:
        // compute new distance = g(c) + cost(getting from c to n)
        // if dist < g(n) :
        //      if n is not in the fringe and dist + h(n) < f (n) :
        //      add n to the fringe
        //  either way, we're gonna update g(n) = dist
        //  f(n) = g(n) + h(n)
        // our heuristic will be the straight distance to our destination 

        // basically f(start) = g(start) + h(start) = h(start) since g(start) = 0
        while (fringe.Count > 0){
            PathVertex current = getVertexWithSmallestFCost(fringe); // returns the one with smallest f function
            if (current.pos == destination){
                // we're done
                after = System.DateTime.Now;
                pathPlaningTime = after.Subtract(before).Milliseconds;

                return getPath(current);
            }
            // if current != destination THEN :
            fringe.Remove(current); // we remove bc we've already searched it -> prevent infinite loop
            // exploredVertices.Add(current);

            // now we look at all neighbours of C
            foreach (PathVertex neighbor in VisibilityGraphGenerator.getNeighbors(current)){
                // we only want the neighbors that we haven't explored already
                // if (exploredVertices.Contains(neighbor)){
                //     continue;
                // }
                    float transitionDistance = Vector3.Distance(current.pos, neighbor.pos);
                    float alternativeBestDistance = current.gCost + transitionDistance; // g(neighbor)

                // if we found new best distance for neighbor then we update our neighbor
                if (alternativeBestDistance < neighbor.gCost){
                    neighbor.gCost = alternativeBestDistance;
                    neighbor.parentVertex = current;
                    neighbor.hCost = Vector3.Distance(current.pos, destination);
                    neighbor.setFCost();
                    // if neighbor isn't in fringe, this just returns false, we don't care
                    // our way of updating the neighbor whether it was in our fringe or not
                    fringe.Remove(neighbor);
                    fringe.Add(neighbor);
                }
            }
        }
        // outside of while -> empty fringe -> couldn't find path -> return null
        // sometimes path is not found, why?
        after = System.DateTime.Now;
        pathPlaningTime = after.Subtract(before).Milliseconds; 
        return null;
    }


    // in python we would've simply used a priority dictionary, but C# doesn't really have it
    // so we need to implement a bunch of stuff by ourselves
    private PathVertex getVertexWithSmallestFCost(List<PathVertex> list)
    {
        // standard "get min from array" algorithm
        PathVertex min = list[0];
        foreach (PathVertex v in list)
        {
            if (v.fCost < min.fCost)
            {
                min = v;
            }
        }
        return min;
    }

    // this method will return the list of vertices that we need to go through
    // to reach our destination. think of it as an inverted linked list traversal
    // recall : each pathvertex has a reference to its parent vertex
    private List<PathVertex> getPath(PathVertex destination){
        List<PathVertex> path = new List<PathVertex>();
        path.Add(destination);
        PathVertex current = destination;

        // the node that will not have a parent is our starting vertex
        while (current.parentVertex != null){
            path.Add(current.parentVertex);
            current = current.parentVertex;
        }

        // because we inversely traversed our 'linked list', we still need to
        // revert it before we return it
        path.Reverse(); // void method
        return path;
    }





}
