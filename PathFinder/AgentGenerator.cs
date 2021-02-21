using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentGenerator : MonoBehaviour
{
    public int numberOfAgents;
    public Agent agentPrefab;
    public GameObject goalPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("spawnAgents", 1.0f);
    }

    // spawns agent and their destination square and gives the destination square to the agent
    void spawnAgents(){
        while (numberOfAgents-- > 0){
        Vector3 spawnPoint = Terrain.getValidSquare().pos;
        Agent tempAgent = Instantiate(agentPrefab, spawnPoint, Quaternion.identity);

        PathVertex destinationPoint = Terrain.getValidSquare();
        VisibilityGraphGenerator.addPathVertex(destinationPoint);
        tempAgent.setDestination(destinationPoint);
        GameObject goalSquare = Instantiate(goalPrefab, destinationPoint.pos, Quaternion.identity);
        tempAgent.setGoalSquare(goalSquare);
        // once our agent is spawned and has a destination, we make it walk and we forget about it
        // the rest of the code logic is delegated to Agent.cs
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
