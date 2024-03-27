using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

// Not the cleanest but it works for now...
public class ExperimentTaskBlock_Raycast : ExperimentTaskBlock
{
    public ExperimentRaycast experimentRaycast;
    int objOrigLayer;

    public override EInteractionType interactionType => EInteractionType.Raycast;

    public override void OnEndTask()
    {

    }

    public override void OnDeselectXRObject(SelectExitEventArgs args)
    {
        experimentRaycast.heldObject.transform.gameObject.layer = objOrigLayer;

        experimentRaycast.heldObject = null;
    }

    public override void OnSelectXRObject(SelectEnterEventArgs args)
    {
        experimentRaycast.heldObject = args.interactableObject.transform;

        objOrigLayer = experimentRaycast.heldObject.transform.gameObject.layer;
        experimentRaycast.heldObject.transform.gameObject.layer = 2; // ignore raycast
    }
}
