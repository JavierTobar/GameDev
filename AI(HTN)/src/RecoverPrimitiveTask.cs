using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverPrimitiveTask :PrimitiveTask
{
    
    public void ExecuteTaskAndUpdateWorldState()
    {
        // taskDone = false;
        // // Invoke("Done", 1f);
        // Done();
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
        return "Recover";
    }

    public bool IsDone()
    {
        return true;
    }
}
