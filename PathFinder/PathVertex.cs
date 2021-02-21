using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVertex
{
    // reference to its parentVertex
    public PathVertex parentVertex;
    public Vector3 pos;
    // instead of setters/getters let's just leave it public
    // horrible design pattern for workfield, but less boilerplate for assignments
    public float fCost;
    public float gCost;
    public float hCost;

    public PathVertex(Vector3 pVertex){
        pos = pVertex;
    }

    public void setFCost(){
        fCost = gCost + hCost;
    }

    
}
