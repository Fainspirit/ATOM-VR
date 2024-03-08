using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentRaycast : MonoBehaviour
{
    public Transform casterTransform;
    public Transform transformToMoveToHit;
    public LayerMask layerMask;

    LineRenderer lineRenderer;

    public Transform heldObject;
    float raycastHitDistance;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, casterTransform.position);

        DoRaycast();
        if (heldObject != null)
        {
            SetHeldObjectPos();
        }
    }

    void DoRaycast()
    {
        Ray ray = new Ray(casterTransform.position, casterTransform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 9999, layerMask))
        {
            // Move hand
            transformToMoveToHit.position = hit.point;
            lineRenderer.SetPosition(1, hit.point);

            raycastHitDistance = hit.distance;
        }
        else
        {
            lineRenderer.SetPosition(1, casterTransform.position + casterTransform.forward * 100f);
        }
    }

    void SetHeldObjectPos()
    {
        heldObject.transform.position = casterTransform.position + casterTransform.forward * raycastHitDistance;
    }
}
