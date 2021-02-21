using System.Collections;
using System.Collections.Generic;

public interface PrimitiveTask : Task
{
    // Makes sure the preconditions are valid
    bool ValidPreconditions();

    // executes the task and postconditions
    void ExecuteTaskAndUpdateWorldState();

    // returns true when the task is completed
    // allows us to control workflow and dictate when we move to the next task
    // WE NO LONGER NEED THIS 
    bool IsDone();

}
