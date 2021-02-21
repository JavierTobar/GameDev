using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCompoundTask : CompoundTask
{
    // find which way to attack
    public CompoundTask FindTask()
    {

        if (Random.Range(0, 10) > 7){
            return new MeleeAttackCompoundTask(); // basically ~30% chance of doing melee
        }
        
        // try to do range attack but if no objects to throw then melee

        // if crates and rocks are available then randomly pick one to throw
        if ((int) WorldState.get(1) > 0 && (int) WorldState.get(2) > 0){
            // return Random.Range(0, 1f) <= 0.5f ? new ThrowRockCompoundTask() : new ThrowCrateCompoundTask();
            if (Random.Range(0, 1f) <= 0.5f){
                return new ThrowCrateCompoundTask();
            }
            return new ThrowRockCompoundTask();
        }

        // then we either have rocks or crates, return whichever one we have
        if ((int)WorldState.get(1) > 0 || (int)WorldState.get(2) >0)
        {
            // if rock, return rock, if not rock, then crates must've been true so return crates
            // return (bool)WorldState.get(1) ? new ThrowRockCompoundTask() : new ThrowCrateCompoundTask();
            if ((int)WorldState.get(1) > 0){
                return new ThrowRockCompoundTask();
            }
            return new ThrowCrateCompoundTask();
        }

        // last resort : melee the player
        return new MeleeAttackCompoundTask(); 

    }

    // we don't get subtasks here, we just decide which attack to do and from that one we get the subtasks
    public List<Task> GetSubtasks()
    {
        return null;
    }

    public bool isCompound()
    {
        return true;
    }

    public bool NeedsToFindTask()
    {
        return true;
    }
}
