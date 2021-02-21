using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigateToSquarePrimitiveTask : PrimitiveTask
{
    public void ExecuteTaskAndUpdateWorldState()
    {
        CaveMonster.move = true;
    }

    public bool isCompound()
    {
        return false;
    }

    public bool ValidPreconditions()
    {
        return true;
    }

    public override string ToString()
    {
        return "Navigate to Square";
    }

    public bool IsDone()
    {
        Debug.Log("Waiting");
        // we've reached our destination
        if (CaveMonster.monster.position == (Vector3) WorldState.get(11)){
            CaveMonster.move = false;
        }
        return CaveMonster.monster.position == (Vector3) WorldState.get(11);
    }
}
