using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackCompoundTask : CompoundTask
{
    public CompoundTask FindTask()
    {
        throw new System.NotImplementedException();
    }

    public List<Task> GetSubtasks()
    {
        List<Task> subtasks = new List<Task>();
        subtasks.Add(new BecomeRedPrimitiveTask());
        subtasks.Add(new WalkTowardsPlayerPrimitiveTask());
        subtasks.Add(new MeleePlayerPrimitiveTask());
        return subtasks;
    }

    public bool isCompound()
    {
        return true;
    }

    // all subtasks are primitive tasks
    public bool NeedsToFindTask()
    {
        return false;
    }

}
