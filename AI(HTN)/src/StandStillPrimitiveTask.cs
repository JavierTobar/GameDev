using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandStillPrimitiveTask : PrimitiveTask
{
    public void ExecuteTaskAndUpdateWorldState()
    {
        // taskDone = false;
        // // Invoke("Done", 1f);
        // Done();
        // return;
    }

    // private void Done(){
    //     taskDone = true;

    // }

    public bool isCompound()
    {
        return false;
    }

    public void UpdateWorldState()
    {
        return; // nothing to update
    }

    public bool ValidPreconditions()
    {
        return true; // nothing to check
    }

    public override string ToString()
    {
        return "Stand Still";
    }

    public bool IsDone()
    {
        return true;
    }
}
