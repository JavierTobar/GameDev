using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainIdleCompoundTask : CompoundTask
{
    public CompoundTask FindTask()
    {
        return null;
    }

    public List<Task> GetSubtasks()
    {
        List<Task> subtasks = new List<Task>();
        subtasks.Add(new PickSquareNearCavePrimitiveTask());
        subtasks.Add(new NavigateToSquarePrimitiveTask());
        subtasks.Add(new StandingCompoundTask());

        return subtasks;
    }

    public bool isCompound()
    {
        return true;
    }

    // subtasks are linear, nothing to find
    public bool NeedsToFindTask()
    {
        return false;
    }
}
