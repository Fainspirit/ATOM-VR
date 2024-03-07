using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

// Not the cleanest but it works for now...
public class ExperimentTaskBlock_Raycast : ExperimentTaskBlock
{
    public override EInteractionType interactionType => EInteractionType.Raycast;

    public override void OnDeselectXRObject(SelectExitEventArgs args)
    {
        
    }

    public override void OnSelectXRObject(SelectEnterEventArgs args)
    {
        
    }
}
