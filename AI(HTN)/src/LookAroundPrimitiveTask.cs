using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class LookAroundPrimitiveTask : PrimitiveTask
{
    private bool taskDone;
    public void ExecuteTaskAndUpdateWorldState()
    {
        taskDone = false;
        Quaternion randomRotation = Random.rotation;
        CaveMonster.monster.rotation = new Quaternion(randomRotation.x, 90, randomRotation.z, randomRotation.w); // make it look somewhere random
    }

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
        return "Look Around";
    }

    // not needed
    public bool IsDone()
    {
        return taskDone;
    }
}
