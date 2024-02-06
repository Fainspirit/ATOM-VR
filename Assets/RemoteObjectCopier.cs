using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RemoteObjectCopier : MonoBehaviour
{
    [SerializeField] ClonedObjectsManager clonedObjectsManager;

    private void OnTriggerEnter(Collider other)
    {
        // If not a copy
        if (!other.CompareTag("Copy") && other.GetComponent<XRGrabInteractable>() != null)
        {
            clonedObjectsManager.CopyRemoteToLocal(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If not a copy
        if (!other.CompareTag("Copy"))
        {
            clonedObjectsManager.RemoveLocalCopyOfRemote(other.gameObject);
        }
    }
}
