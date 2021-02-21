using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTowardsPlayerPrimitiveTask : PrimitiveTask
{
    // similar to NavigateToSquare
    public void ExecuteTaskAndUpdateWorldState()
    {
        // set mosnter destination to wherever the player currently is
        Vector3 playerLocation = (Vector3)WorldState.get(8);
        WorldState.set(11, new Vector3(playerLocation.x, 1.0f, playerLocation.z));
        CaveMonster.move = true;
    }

    public bool isCompound()
    {
        return false;
    }

    public bool ValidPreconditions()
    {
        // check if player is within reach of monster, if yes then we can walk to it
        return (bool)WorldState.get(6);
    }
    public override string ToString()
    {
        return "Walk Towards Player";
    }

    public bool IsDone()
    {
        // we've reached our destination
        if (CaveMonster.monster.position == (Vector3) WorldState.get(11)){
            CaveMonster.move = false;
        }
        return CaveMonster.monster.position == (Vector3) WorldState.get(11);
    }
}
