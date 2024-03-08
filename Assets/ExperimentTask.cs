using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

// One task that can be loaded
public class ExperimentTask : MonoBehaviour
{
    XRGrabInteractable[] grabbables;

    public TaskStatistics taskStatistics;
    public ExperimentTaskBlock block;

    private void Start()
    {
        Debug.Log($"Starting task: {name}");
        StartTask();
    }

    public void StartTask()
    {
        Debug.Log("Started task " + name);
        taskStatistics = new TaskStatistics()
        {
            totalTaskTime = 0,

            incorrectSelections = 0,
            totalSelections = 0,

            taskName = this.name,
        };

        grabbables = GetComponentsInChildren<XRGrabInteractable>();

        // Go through all grabbables in the task and add the appropriate listeners

        foreach (XRGrabInteractable grabbable in grabbables)
        {
            // How do I track this if it removes my listeners????
            // Do it through the block I guess...
            //grabbable.selectEntered.AddListener(OnSelectTrackStats);

            // Behavior based on how the block says these should be handled
            //grabbable.selectEntered.AddListener(block.OnSelectXRObject);
            //grabbable.selectExited.AddListener(block.OnDeselectXRObject);
        }
    }

    public void CompleteTask()
    {
        Debug.Log("Completed task " + name);

        block.CompleteTask(this);
    }

    private void Update()
    {
        taskStatistics.totalTaskTime += Time.deltaTime;
    }

    public void OnSelectTrackStats(SelectEnterEventArgs seea)
    {
        taskStatistics.totalSelections++;

        ExperimentObjectSettings eos = seea.interactableObject.transform.GetComponent<ExperimentObjectSettings>();
        if (eos != null)
        {
            // wrong one
            if (!eos.isGrabTarget)
            {
                taskStatistics.incorrectSelections++;
            }
        }
    }

    public void OnDeselectTrackStats(SelectExitEventArgs seea)
    {
        // ok we let go of the correct one, time to move on
        ExperimentObjectSettings eos = seea.interactableObject.transform.GetComponent<ExperimentObjectSettings>();
        if (eos != null)
        {
            if (eos.isGrabTarget)
            {
                // done with this task
                CompleteTask();
            }
        }
    }
}
