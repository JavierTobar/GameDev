using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowCrateCompoundTask : CompoundTask
{
    public CompoundTask FindTask()
    {
        return null;
    }

    public List<Task> GetSubtasks()
    {
        List<Task> subtasks = new List<Task>();
        subtasks.Add(new WalkToCratePrimitiveTask());
        subtasks.Add(new GrabAndThrowCratePrimitiveTask());
        return subtasks;

    }

    public bool isCompound()
    {
        return true;
    }

    // all subtasks are primitive
    public bool NeedsToFindTask()
    {
        return false;
    }
}
