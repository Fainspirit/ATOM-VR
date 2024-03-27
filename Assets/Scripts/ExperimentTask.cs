using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

// One task that can be loaded
public class ExperimentTask : MonoBehaviour
{
    XRGrabInteractable[] grabbables;

    public TaskStatistics taskStatistics;
    public ExperimentTaskBlock block;

    // Just here for stats - set in inspector
    public int distanceMeters;
    public int occlusionAngleDegrees;

    // for tracking grab time
    public ExperimentObjectTaskData grabbedData;
    StreamWriter streamWriter;

    //private void Start(StreamWriter writer)
    //{
    //    Debug.Log($"Starting task: {name}");
    //    streamWriter = writer;

    //    StartTask();
    //}

    public void StartTask(TaskStatistics stats)
    {
        Debug.Log("Started task " + name);
        taskStatistics = stats;
        taskStatistics.StartNewTask(name, block.interactionType, distanceMeters, occlusionAngleDegrees);

        //grabbables = GetComponentsInChildren<XRGrabInteractable>();

        // Go through all grabbables in the task and add the appropriate listeners

        //foreach (XRGrabInteractable grabbable in grabbables)
        //{
        //    // How do I track this if it removes my listeners????
        //    // Do it through the block I guess...
        //    //grabbable.selectEntered.AddListener(OnSelectTrackStats);

        //    // Behavior based on how the block says these should be handled
        //    //grabbable.selectEntered.AddListener(block.OnSelectXRObject);
        //    //grabbable.selectExited.AddListener(block.OnDeselectXRObject);
        //}
    }


    private void Update()
    {
        taskStatistics.totalTaskTime += Time.deltaTime;

        // Track hold time
        if (grabbedData != null)
        {
            if (grabbedData.isGrabTarget)
            {
                taskStatistics.timeHoldingCorrectObject += Time.deltaTime;
            }
            else
            {
                taskStatistics.timeHoldingIncorrectObject += Time.deltaTime;
            }
        }
    }

    // Ending the task
    public void EndSuccessfully() { EndTask(true); }

    public void EndUnsuccessfully() { EndTask(false); }
    public void EndTask(bool wasCompletedSuccessfully)
    {
        Debug.Log("Completed task " + name);

        taskStatistics.wasTaskCompletedSuccessfully = wasCompletedSuccessfully;
        taskStatistics.Write();

        block.CompleteTask(this);
    }

    // Selection handlers
    public void OnSelectObjectInTask(SelectEnterEventArgs seea)
    {
        taskStatistics.totalSelections++;

        ExperimentObjectTaskData eos = seea.interactableObject.transform.GetComponent<ExperimentObjectTaskData>();
        if (eos != null)
        {
            grabbedData = eos;

            // wrong one
            if (!eos.isGrabTarget)
            {
                taskStatistics.incorrectSelections++;
            }
        }
    }

    public void OnDeselectObjectInTask(SelectExitEventArgs seea)
    {
        // ok we let go of the correct one, time to move on
        ExperimentObjectTaskData eos = seea.interactableObject.transform.GetComponent<ExperimentObjectTaskData>();
        if (eos != null)
        {
            grabbedData = null;

            if (eos.isGrabTarget)
            {
                // done with this task
                EndSuccessfully();
            }
        }
    }
}
