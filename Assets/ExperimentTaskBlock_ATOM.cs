using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

// Not the cleanest but it works for now...
public class ExperimentTaskBlock_ATOM : ExperimentTaskBlock
{
    [SerializeField] ClonedObjectsManager objectsManager;

    public override EInteractionType interactionType => EInteractionType.ATOM;

    public override void OnDeselectXRObject(SelectExitEventArgs args)
    {
        objectsManager.OnDeselectXRObject(args);
    }

    public override void OnSelectXRObject(SelectEnterEventArgs args)
    {
        objectsManager.OnSelectXRObject(args);
    }
}
