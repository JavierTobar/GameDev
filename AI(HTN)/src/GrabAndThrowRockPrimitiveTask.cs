using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAndThrowRockPrimitiveTask : PrimitiveTask
{
 public void ExecuteTaskAndUpdateWorldState()
    {
        // grab rock near you
        Collider[] colliders = Physics.OverlapSphere((Vector3) WorldState.get(7), 4f);
        // we pick the first rock that comes in our overlapsphere
        foreach (Collider c in colliders){
            if (c.transform.name.Contains("Rock")){
                Rock rock = c.gameObject.GetComponent<Rock>();
                // throw at current destination of player
                rock.ThrowRock((Vector3)WorldState.get(8));
                return; // otherwise we throw all the nearby rocks
            }
        }
    }

    public bool isCompound()
    {
        return false;
    }

    public bool ValidPreconditions()
    {   
        // check if nearby crates > 0
        return (int) WorldState.get(2) > 0;
    }

    public override string ToString()
    {
        return "Grab and Throw Rock";
    }

    public bool IsDone()
    {
        throw new System.NotImplementedException();
    }
}
