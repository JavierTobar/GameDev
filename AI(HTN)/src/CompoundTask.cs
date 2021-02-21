using System.Collections.Generic;
using System;

public interface CompoundTask : Task
{
    // if our compound is strictly composed of primitives, then we don't need to find a task, we just return the subtasks
    bool NeedsToFindTask();

    // if our compound has more than one compound, then we need to find the best compound to return if it exists
    // otherwise just randomly return a compound
    CompoundTask FindTask();

    // returns the subtasks of the compound
    List<Task> GetSubtasks();
}
