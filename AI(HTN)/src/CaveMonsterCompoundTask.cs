using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMonsterCompoundTask : CompoundTask
{
    public CompoundTask FindTask()
    {

        // checks if player is within range, i.e. prioritize attack if possible
        // return (bool) WorldState.get(6) ? new MainAttackCompoundTask() : new MainIdleCompoundTask();
        if ((bool) WorldState.get(6)){
            return new MainAttackCompoundTask();
        }
        return new MainIdleCompoundTask();
    }

    // we don't get subtasks here, we just decide which task to do
    // and then we get the subtasks from that one
    public List<Task> GetSubtasks()
    {
        return null;
    }
    
    public bool isCompound()
    {
        return true;
    }

    // true because we have main attack and main idle, which are both compounds
    public bool NeedsToFindTask()
    {
        return true;
    }
}
