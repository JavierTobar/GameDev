using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayerPrimitiveTask : PrimitiveTask
{
    private bool taskDone;
    public void ExecuteTaskAndUpdateWorldState()
    {
        // look at current position of player
        CaveMonster.monster.LookAt((Vector3) WorldState.get(8));
        taskDone = false;
        // Invoke("Done", 1f);
        // Done();
    }

    // private void Done(){
    //     taskDone = true;
    // }
    public bool isCompound()
    {
        return false;
    }

    // check if we have a direct line of vision to the player
    public bool ValidPreconditions()
    {
        return true;
    }
    
    public override string ToString()
    {
        return "Face Player";
    }

    public bool IsDone()
    {
        return taskDone;
    }
}
