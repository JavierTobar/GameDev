using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRockPrimitiveTask : PrimitiveTask
{
    public void ExecuteTaskAndUpdateWorldState()
    {
        throw new System.NotImplementedException();
    }

    public bool isCompound()
    {
        return false;
    }

    public bool ValidPreconditions()
    {
        // check if nearby rocks > 0
        return (int) WorldState.get(1) > 0;    
        }

    public override string ToString()
    {
        return "Grab Rock";
    }

    public bool IsDone()
    {
        throw new System.NotImplementedException();
    }
}
