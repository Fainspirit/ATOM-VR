using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskPool : MonoBehaviour
{
    [Tooltip("The tasks to pick from. Should have 12 tasks")]
    public ExperimentTask[] tasks;
    public Queue<ExperimentTask> taskQueue;

    public int selectionsRemaining;

    private void Awake()
    {
        if (tasks.Length != 18)
        {
            Debug.LogWarning("There aren't 18 tasks in the pool!");
        }

        // If any task is duplicated
        foreach (ExperimentTask task in tasks)
        {
            if (tasks.Count(t => t == task) > 1){
                Debug.LogWarning($"DUPLICATE TASK IN THE TASK LIST! ({task})");
            }
        }
    
        ResetPool();
        
    }

    public void ResetPool()
    {
        selectionsRemaining = tasks.Length;

        foreach (ExperimentTask task in tasks)
        {
            task.gameObject.SetActive(false);
        }

        // Random "acclimation" trials (to discard)
        for (int i = 0; i < 3; i++)
        {
            taskQueue.Enqueue(GetRandomUnselectedTask());
        }

        selectionsRemaining = tasks.Length;
        // Rest of them

        for (int i = 0; i < tasks.Length; i++)
        {
            taskQueue.Enqueue(GetRandomUnselectedTask());
        }
    }
    

    ExperimentTask GetRandomUnselectedTask()
    {
        if (selectionsRemaining <= 0) return null;

        int idx = Random.Range(0, selectionsRemaining);
        ExperimentTask ret = tasks[idx];

        // "remove" from pool by swapping the one we took with the end one
        // and decreasing the selection range
        tasks[idx] = tasks[selectionsRemaining - 1];
        tasks[selectionsRemaining - 1] = ret;
        selectionsRemaining--;

        return ret;
    }
    public bool CanGetTask()
    {
        return taskQueue.Count > 0;
    }

    public ExperimentTask GetNextTaskFromPool()
    {
        if (!CanGetTask()) { return null; }

        return taskQueue.Dequeue();
    }

}
