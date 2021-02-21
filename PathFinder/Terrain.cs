using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }


    // We randomly select a square such that x in [-18, 106] and y = 0 and z in [-15, 140] 
    // These coordinates englobe our terrain, however there's 3 possibilities :
    // Square is free and valid -> return 
    // Square is not on terrain -> on empty space -> square not valid -> fetch new coordinates
    // Square is on one of the L shapes or another agent or an already existing destination that we've generated 
    // -> square not free -> fetch new coordinates
    public static PathVertex getValidSquare()
    {
        RaycastHit hit;
        // busy loop that we break out of by returning a valid square
        while (true)
        {
            // because we're projecting a ray and sphere downwards, 
            // we start at y = 15 so that we can hit the terrain at y = 0
            Vector3 position = new Vector3(Random.Range(-18, 106), 15.0f, Random.Range(-15, 140));
            // We either hit free square in terrain or an L shape
            // we don't want to spherecast here because it's 3x3 
            // and our agent might be spawned too much outside of the terrain
            if (Physics.Raycast(position, Vector3.down, out hit))
            {

                // if not wall return the vector else keep looping
                // if (hit.transform.tag != "Wall" && hit.transform.tag != "Agent" && hit.transform.tag != "Goal"){
                // because our agents or 3x3 in size, we need to do an additional spherecast now
                // want to spawn far from LShape
                if (Physics.SphereCast(position, 12.0f, Vector3.down, out hit))
                {
                    if (hit.transform.tag != "LShape")
                    {
                        // less strict for walls because we want to spawn inside acloves 
                        if (Physics.SphereCast(position, 3.0f, Vector3.down, out hit))
                        {
                            if (hit.transform.tag != "Wall" && hit.transform.tag != "Agent" && hit.transform.tag != "Goal")
                            {
                                return new PathVertex(new Vector3(position.x, 1.0f, position.z));
                            }
                        }
                    }
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}

