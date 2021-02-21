using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAttackCompoundTask : CompoundTask
{
    // we prioritize throwing rocks/crates, we only melee when we can't find rocks/crates nearby
    public CompoundTask FindTask()
    {
        return null;
    }

    public List<Task> GetSubtasks()
    {
        List<Task> subtasks = new List<Task>();
        subtasks.Add(new FacePlayerPrimitiveTask());
        subtasks.Add(new AttackCompoundTask());
        subtasks.Add(new RecoverPrimitiveTask());

        return subtasks;
    }

    public bool isCompound()
    {
        return true;
    }


    // linear order, no options to be made here, options to pick attack are in AttackCompoundTask
    public bool NeedsToFindTask()
    {
        return false;
    }
}
