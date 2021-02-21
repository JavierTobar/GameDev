using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// HTN AI LOGIC 
public class CaveMonster : MonoBehaviour {
    public static Transform monster;
    public TextMeshProUGUI displayCurrentPlan; // durationLeftOnShield into text for the player
    public TextMeshProUGUI displayCurrentTask; // i.e. shield active and shield not active
    public Material normalMonsterBodyMaterial;
    public Material redMonsterBodyMaterial;
    private static List<PrimitiveTask> Plan;
    private static bool hasPlan;
    public static bool move; // if true, move to destination in world state
    private static string wholePlan;
    private static bool updateWholePlan;
    private static string currentPlan;
    private static bool updateCurrentPlan;
    private static PrimitiveTask currentTask;
    private static int index;
    // Start is called before the first frame update
    void Start () {
        updateCurrentPlan = updateWholePlan = move = hasPlan = false;
        monster = GetComponent<Transform> ();
        Plan = new List<PrimitiveTask> ();
        index = 0;
        SimpleForwardPlanner ();
        ExecutePlan ();
    }

    void SimpleForwardPlanner () {
        Plan.Clear (); // we clear the plan before we start populating it
        Stack<Task> tasks = new Stack<Task> ();
        tasks.Push (new CaveMonsterCompoundTask ()); // HTN ROOT
        while (tasks.Count > 0) {

            Task task = tasks.Pop ();

            if (task.isCompound ()) {
                // then we can safely cast it 

                CompoundTask compoundTask = (CompoundTask)task;
                // not all compound tasks need to find a task
                if (compoundTask.NeedsToFindTask ()) {
                    CompoundTask method = compoundTask.FindTask ();
                    // if we found a task
                    if (method != null) {

                        // CODE TO SAVE STATE HERE NOT NEEDED BECAUSE OF PRE CONDITIONS

                        // we push the subtasks into our tasks stack 
                        foreach (Task t in method.GetSubtasks ()) {
                            tasks.Push (t);
                        }
                    } else {
                        // if method == null
                        // CODE TO RESTORE STATE HERE
                    }
                } 
                // if don't need to find task, then we just add all subtasks of the compoundtask to tasks
                else {
                    foreach(Task t in compoundTask.GetSubtasks()){
                        tasks.Push(t);
                    }
                }
            } // ELSE task is primitive
            else {
                // can safely cast
                PrimitiveTask primitiveTask = (PrimitiveTask)task;
                if (primitiveTask.ValidPreconditions ()) {
                    // Also need to Update State but we will do that when we execute the primitive task, not here
                    Plan.Add (primitiveTask);
                } else {
                    // if preconditions were false
                    // CODE TO RESTORE STATE HERE
                }
            }
        }
        hasPlan = true;
        Plan.Reverse(); // because we built it bottom up
    }

    // executes the plan, updates world state, displays plan on text 
    void ExecutePlan () {
        // reset texts
        string wholePlanStr = "Monster's current plan : ";
        foreach (PrimitiveTask t in Plan) {
            wholePlanStr += t.ToString () + " => ";
        }
        wholePlan = wholePlanStr.Substring(0, wholePlanStr.Length - 3); // remove the arrow
        updateWholePlan = true;

        // main execution of plan
        // foreach (PrimitiveTask t in Plan) {
        //     displayCurrentPlan.SetText(t.ToString ());
        //     float time = 2f;
        //     currentTask = t;
        //     Invoke("ExecuteCurrentTask", time); // we delay it by 1 second more so that 1s in between each one
        //     time+=5f;

            
        //     // busy loop, we won't go to next PrimitiveTask until the current one is done
        //     // while (!t.IsDone ());
        // }
        Invoke("ExecuteCurrentTaskHelperWithTimeDelay", 1f);
    }

    private void ExecuteCurrentTaskHelperWithTimeDelay(){
        if (index >= Plan.Count){
            index = 0;
            hasPlan = false; // because we finished this plan

        } else {
            PrimitiveTask task = Plan[index];
            // Debug.Log("Currently executing " + task.ToString());
            task.ExecuteTaskAndUpdateWorldState();
            index++;
            Invoke("ExecuteCurrentTaskHelperWithTimeDelay", 1f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // if its red, set to red, else set to green
        if ((bool)WorldState.get(9)){
            transform.Find("Body").GetComponent < MeshRenderer > ().material = redMonsterBodyMaterial;
        } else {
            transform.Find("Body").GetComponent < MeshRenderer > ().material = normalMonsterBodyMaterial;
        }
        if (updateWholePlan){
            displayCurrentPlan.SetText(wholePlan);
            updateWholePlan = false;
        }

        if (index < Plan.Count && hasPlan){
            // fix offset issue with the index
            // for TA : don't try to figure it out, it's just a minor string issue
            // displayCurrentTask.SetText("Currently executing " + Plan[index == 0 ? index : index - 1].ToString());
            displayCurrentTask.SetText("Currently executing " + Plan[index].ToString());
        } else {
            displayCurrentTask.SetText("Currently executing nothing");
        }
        // if no plan then we get a plan
        if (!hasPlan){
            hasPlan = true;
            SimpleForwardPlanner(); // find plan
            ExecutePlan(); // execute plan and turns hasPlan to false when done
        }

        if (move){
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, (Vector3) WorldState.get(11), 16.0f * Time.deltaTime);
        }
        // update position
        WorldState.set(7, monster.transform.position);

    }
}