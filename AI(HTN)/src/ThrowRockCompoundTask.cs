using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRockCompoundTask : CompoundTask
{
    public CompoundTask FindTask()
    {
        return null;
    }

    public List<Task> GetSubtasks()
    {
        List<Task> subtasks = new List<Task>();
        subtasks.Add(new WalkToRockPrimitiveTask());
        subtasks.Add(new GrabAndThrowRockPrimitiveTask());
        return subtasks;
    }

    public bool isCompound()
    {
        return true;
    }

    // only primitive tasks
    public bool NeedsToFindTask()
    {
        return false;
    }

}
