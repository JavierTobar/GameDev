using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePlayerPrimitiveTask : PrimitiveTask
{
    public void ExecuteTaskAndUpdateWorldState()
    {
        // check if player is within melee range (this could be a worldstate instead)
        // but it would require me to refactor and redraw my tree and i don't have enough time to change
        // my codebase and redraw a tree :(
        Collider[] colliders = Physics.OverlapSphere((Vector3) WorldState.get(7), 8f);
        foreach(Collider c in colliders){
            // if player is in range
            if (c.transform.name.Equals("Player"))
            {
                // we want to see if he had shield on
                // if yes => we don't take his health away, if no => we take health away 
                int playerCurrentHealth = (int)WorldState.get(3);
                WorldState.set(3,
                    (bool)WorldState.get(10) ? playerCurrentHealth : playerCurrentHealth - 1);

                // our monster is no longer red
                WorldState.set(9, false);
                return; // break the loop
                }
            }
            WorldState.set(9, false);
        }


    public bool isCompound()
    {
        return false;
    }

    public bool ValidPreconditions()
    {
        // check if player is within immediate range of player
        return true;
    }

    public override string ToString()
    {
        return "Melee Player";
    }

    public bool IsDone()
    {
        throw new System.NotImplementedException();
    }
}
