using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToRockPrimitiveTask : PrimitiveTask
{
    public void ExecuteTaskAndUpdateWorldState()
    {
        // if validpreconditions, then make monster move to that destination
        CaveMonster.move = true;
    }

    public bool isCompound()
    {
        return false;
    }

    // not needed
    public bool IsDone()
    {
        throw new System.NotImplementedException();
    }

    // true if rock nearby 
    public bool ValidPreconditions()
    {
        Collider[] colliders = Physics.OverlapSphere((Vector3)WorldState.get(7), 40f);
        // we pick the first crate that comes in our overlapsphere
        foreach (Collider c in colliders)
        {
            if (c.transform.name.Contains("Rock"))
            {
                // make whichever rock we found the monster's destination
                WorldState.set(11, c.transform.position);
                return true;
            }
        }
        // else false
        return false;

    }
   public override string ToString()
    {
        return "Walk to Rock";
    }

}
