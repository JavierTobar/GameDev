using System.Collections;

public interface Task 
{
    // True if compound, False is Primitive Task
    bool isCompound();

    // Need to override ToString to return a meaningful string of what the task is doing
    // so that the game can display the plan
    string ToString();
}

