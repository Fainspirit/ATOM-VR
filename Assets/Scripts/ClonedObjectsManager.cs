using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static Unity.VisualScripting.Member;

public class ClonedObjectsManager : MonoBehaviour
{

    // Maps <real, copy>
    public Dictionary<GameObject, GameObject> remoteToLocalCopies = new Dictionary<GameObject, GameObject>();
    public Dictionary<GameObject, GameObject> localCopyToRemoteShadowCopies = new Dictionary<GameObject, GameObject>();

    [SerializeField] GameObject remoteSphere;
    [SerializeField] GameObject localSphere;
    float remoteToLocalScaler;
    float localToRemoteScalar;

    private void Update()
    {
        remoteToLocalScaler = localSphere.transform.localScale.magnitude / remoteSphere.transform.localScale.magnitude;
        localToRemoteScalar = 1 / remoteToLocalScaler;

        UpdateCopyPositions();
    }

    // MAKE PROXY
    public void CopyRemoteToLocal(GameObject obj)
    {
        MakeProxyObject(obj, remoteToLocalCopies, remoteToLocalScaler);
    }

    public void CopyLocalToRemote(GameObject obj)
    {
        MakeProxyObject(obj, localCopyToRemoteShadowCopies, localToRemoteScalar);
    }

    void MakeProxyObject(GameObject obj, Dictionary<GameObject, GameObject> trackingDict, float scalar) 
    {
        if (!trackingDict.ContainsKey(obj))
        {
            GameObject copy = CopyObject(obj);
            trackingDict.Add(obj, copy);

            // scale
            copy.transform.localScale *= scalar;
        }
        else
        {

        }
    }


    // REMOVE PROXY
    public void RemoveLocalCopyOfRemote(GameObject obj)
    {
        // First remove the remote "shadow" copy of the local in case it was being grabbed.
        // Might be scuffed, will see
        //RemoveRemoteCopyOfLocalCopy(remoteToLocalCopies[obj]);

        RemoveProxyFrom(obj, remoteToLocalCopies);

    }

    public void RemoveRemoteCopyOfLocalCopy(GameObject obj)
    {
        RemoveProxyFrom(obj, localCopyToRemoteShadowCopies);
    }

    void RemoveProxyFrom(GameObject obj, Dictionary<GameObject, GameObject> trackingDict)
    {
        if (trackingDict.ContainsKey(obj))
        {
            GameObject copy = trackingDict[obj];
            trackingDict.Remove(obj);

            Debug.Log($"Destroying copy of {obj}");
            Destroy(copy.gameObject);
        }
    }


    // ACTUAL COPY LOGIC
    GameObject CopyObject(GameObject source)
    {
        Debug.Log($"Making copy of {source}");

        GameObject copy = Instantiate(source);
        copy.tag = "Copy";

        // Disable gravity and any other pushes
        Rigidbody rb = copy.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        // set layer to "Copy" (layer 7)
        // This is to disable physics with the rest of the world
        copy.layer = 7;

        // Check if it was a copy
        // - if so, use same origin
        // - if not, source is origin
        OriginTracker priorOriginTracker = source.GetComponent<OriginTracker>();
        OriginTracker newOriginTracker = copy.AddComponent<OriginTracker>();
        if (priorOriginTracker != null)
        {
            newOriginTracker.original = priorOriginTracker.original;
        }
        else
        {
            newOriginTracker.original = source;
        }

        return copy;
    }

    // For all copies, update their relative positions
    void UpdateCopyPositions()
    {
        Vector3 localSpherePos = localSphere.transform.position;
        Vector3 remoteSpherePos = remoteSphere.transform.position;

        // Remote to local
        foreach (GameObject key in remoteToLocalCopies.Keys)
        {
            SetObjectVirtualPosition(key, remoteToLocalCopies[key], remoteSpherePos, localSpherePos, remoteToLocalScaler);
        }

        // Local to remote
        foreach (GameObject key in localCopyToRemoteShadowCopies.Keys)
        {
            SetObjectVirtualPosition(key, localCopyToRemoteShadowCopies[key], localSpherePos, remoteSpherePos, localToRemoteScalar);
        }
    }

    void SetObjectVirtualPosition(GameObject source, GameObject copy, Vector3 sourceSpherePos, Vector3 destSpherePos, float scalar)
    {
        // Get offset of source from remote sphere
        Vector3 sourceOffset = sourceSpherePos - source.transform.position;

        // Now we position the local one accordingly
        Vector3 copyPos = destSpherePos - sourceOffset * scalar;

        // Assign pos
        copy.transform.position = copyPos;

        // Also copy rotation
        copy.transform.rotation = source.transform.rotation;

        // And scale (in case things grow)
        // I think this works... need to test
        //copy.transform.localScale = source.transform.localScale * scalar;
    }

    // Handle grab event
    public void OnSelectXRObject(SelectEnterEventArgs args)
    {
        GameObject selected = args.interactableObject.transform.gameObject;

        CopyLocalToRemote(selected);

        // stop tracking it to avoid position conflicts
        remoteToLocalCopies.Remove(selected.GetComponent<OriginTracker>().original);

        // this deletes it, don't do that xd
        //RemoveLocalCopyOfRemote(selected.GetComponent<OriginTracker>().original);
    }

    // assumes it's a local copy
    // TODO - branch the handler later
    public void OnDeselectXRObject(SelectExitEventArgs args)
    {
        GameObject grabbedLocalCopy = args.interactableObject.transform.gameObject;      
        GameObject remoteCopy = localCopyToRemoteShadowCopies[grabbedLocalCopy];
        GameObject original = grabbedLocalCopy.GetComponent<OriginTracker>().original;

        // The original disappeared (task change);
        if (original == null)
        {
            localCopyToRemoteShadowCopies.Remove(grabbedLocalCopy);
            Destroy(grabbedLocalCopy);
            Destroy(remoteCopy);
        }

        // Update the remote's position based on where the remote copy
        // was when we let go of the local copy - rot too
        original.transform.position = remoteCopy.transform.position;
        original.transform.rotation = remoteCopy.transform.rotation;

        // destroy the remote shadowcopy
        RemoveProxyFrom(grabbedLocalCopy, localCopyToRemoteShadowCopies);

        // Check if the original object is still in the sphere and should be tracked again locally
        if (ObjectInRemoteSphere(original))
        {
            // Then retrack the local copy
            // In the case where we grabbed it, then left and came back, we don't want to add it again
            if (!remoteToLocalCopies.ContainsKey(original))
            {
                remoteToLocalCopies.Add(original, grabbedLocalCopy);
            }
            else
            {
                // we need to remove the grabbed local copy
                // We could try to prevent it from being created in the first place
                // But then we need to track what it is somewhere
                Destroy(grabbedLocalCopy);
            }
        }
        else 
        {
            // destroy the local copy since we don't need it now
            // It was untracked when we grabbed it, so everything
            // should be cleaned up now
            Destroy(grabbedLocalCopy);
        }




    }
    // Need a cancel too or something. Maybe dont immediately reposition


    // Position based check - mostly for keeping thing inside or not
    // Maybe don't trust
    bool ObjectInRemoteSphere(GameObject obj)
    {
        return (remoteSphere.GetComponent<SphereCollider>().bounds.Contains(obj.transform.position));
    }

}
