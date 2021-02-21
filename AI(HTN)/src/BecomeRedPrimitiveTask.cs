using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BecomeRedPrimitiveTask : PrimitiveTask
{
    public void ExecuteTaskAndUpdateWorldState()
    {
        WorldState.set(9, true); // isRed = true
    }

    public bool isCompound()
    {
        return false;
    }

    public bool ValidPreconditions()
    {
        return true; // always true
    }

    public override string ToString()
    {
        return "Become Red";
    }

    // not needed anymore
    public bool IsDone()
    {
        throw new System.NotImplementedException();
    }
}
