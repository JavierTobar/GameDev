using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickSquareNearCavePrimitiveTask : PrimitiveTask
{
    private bool taskDone;
    public void ExecuteTaskAndUpdateWorldState()
    {
        taskDone = false;
        // Get random square near cave 
        // Range was manually found, y value is always 1 though
        int xValue = Random.Range(34, 53);
        int zValue = Random.Range(55, 96);

        Vector3 square = new Vector3(xValue, 1.0f, zValue);
        // set the square in WorldState
        WorldState.set(11, square);
    }

    public bool isCompound()
    {
        return false;
    }

    public bool ValidPreconditions()
    {
        return true; // nothing to check
    }

    public override string ToString()
    {
        return "Pick Square";
    }

    public bool IsDone()
    {
        return taskDone;
    }
}
