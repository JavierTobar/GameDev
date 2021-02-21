using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// need to rename filename to GrabAndThrowCratePrimitiveTask
public class GrabAndThrowCratePrimitiveTask : PrimitiveTask
{
    public void ExecuteTaskAndUpdateWorldState()
    {
        // grab crate near you
        Collider[] colliders = Physics.OverlapSphere((Vector3) WorldState.get(7), 4f);
        // we pick the first crate that comes in our overlapsphere
        foreach (Collider c in colliders){
            if (c.transform.name.Contains("Crate")){
                // will this work? yes :)
                Crate crate = c.gameObject.GetComponent<Crate>();
                // throw at current destination of player
                crate.ThrowCrate((Vector3)WorldState.get(8));
                return; // otherwise we throw all the nearby crates
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
        return "Grab and Throw Crate";
    }

    public bool IsDone()
    {
        throw new System.NotImplementedException();
    }
}
