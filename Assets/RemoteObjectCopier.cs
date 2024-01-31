using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteObjectCopier : MonoBehaviour
{
    [SerializeField] ClonedObjectsManager clonedObjectsManager;

    private void OnTriggerEnter(Collider other)
    {
        // If not a copy
        if (!other.CompareTag("Copy"))
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
