using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPool : MonoBehaviour
{
    [Tooltip("The tasks to pick from. Should have 12 tasks")]
    public ExperimentTask[] tasks;

    public int selectionsRemaining;

    private void Awake()
    {
        if (tasks.Length != 12)
        {
            Debug.LogWarning("There aren't 12 tasks in the pool!");
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
    }
    
    public bool CanGetTask()
    {
        return selectionsRemaining > 0;
    }

    public ExperimentTask GetTaskFromPool()
    {
        if (!CanGetTask()) { return null; }

        int idx = Random.Range(0, selectionsRemaining);
        ExperimentTask ret = tasks[idx];

        // "remove" from pool by swapping the one we took with the end one
        // and decreasing the selection range
        tasks[idx] = tasks[selectionsRemaining - 1];
        tasks[selectionsRemaining - 1] = ret;
        selectionsRemaining--;
        
        return ret;
    }

}
