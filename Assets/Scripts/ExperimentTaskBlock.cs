using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class ExperimentTaskBlock : MonoBehaviour
{
    public TaskPool taskPool;
    public abstract EInteractionType interactionType { get; }

    ExperimentRunner experimentRunner;
    public ExperimentTask currentTask;



    public bool IsTaskBlockFinished()
    {
        return !taskPool.CanGetTask();
    }

    public void AdvanceToNextTask()
    {
        if (IsTaskBlockFinished())
        {
            FinishTaskBlock();
        }
        else
        {
            // Make copy of it so we can just destroy it later
            currentTask = Instantiate(taskPool.GetNextTaskFromPool());
            currentTask.gameObject.SetActive(true);
            currentTask.block = this;
            currentTask.StartTask(experimentRunner.statistics);

            // Have the object start this... maybe weirdness with initializing
            //Debug.Log($"Starting task: {nextTask}");
            //nextTask.StartTask(this);
        }

    }

    public void StartTaskBlock(ExperimentRunner expRun)
    {
        taskPool.ResetPool();
        experimentRunner = expRun;
        AdvanceToNextTask();
    }

    public void FinishTaskBlock()
    {
        experimentRunner.CompleteBlock(this);
    }

    public void CompleteTask(ExperimentTask task)
    {

        // Leave it around as we need to do stuff with it maybe
        // Can prob delete it if we make the "success" based on letting go...
        // task.gameObject.SetActive(false);
        OnEndTask();

        // Remove it since stats are already recorded
        // We're working on instances so this is fine
        Destroy(task.gameObject);

        // DING!
        experimentRunner.audioSource.Play();

        // Move on
        AdvanceToNextTask();
    }





    //public void Write(StreamWriter sw)
    //{
    //    // Name
    //    sw.Write(name + ", ");

    //    // What technique
    //    sw.Write(interactionType.ToString() + ", ");

    //    // Write all the tasks
    //    foreach(TaskStatistics taskStats in stats)
    //    {
    //        taskStats.Write(sw);
    //    }
    //}


    // unity events don't want to let me assign to them, so we're doing a sketchy wrapper here
    public void SelectXRObject_BlockLevel(SelectEnterEventArgs args)
    {
        OnSelectXRObject(args);

        currentTask?.OnSelectObjectInTask(args);

    }
    public abstract void OnSelectXRObject(SelectEnterEventArgs args);

    public void DeselectXRObject_BlockLevel(SelectExitEventArgs args)
    {
        OnDeselectXRObject(args);

        currentTask?.OnDeselectObjectInTask(args);
    }
    public abstract void OnDeselectXRObject(SelectExitEventArgs args);

    public abstract void OnEndTask();
}
