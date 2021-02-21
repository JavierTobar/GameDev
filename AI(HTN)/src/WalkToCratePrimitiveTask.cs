using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToCratePrimitiveTask : PrimitiveTask
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

    public bool ValidPreconditions()
    {
        Collider[] colliders = Physics.OverlapSphere((Vector3)WorldState.get(7), 40f);
        // we pick the first crate that comes in our overlapsphere
        foreach (Collider c in colliders)
        {
            if (c.transform.name.Contains("Crate"))
            {
                // make whichever crate we found the monster's destination
                WorldState.set(11, new Vector3(c.transform.position.x, 1.0f, c.transform.position.z));
                return true;
            }
        }
        // else false
        return false;
    }

    public override string ToString()
    {
        return "Walking to Crate";
    }
}
