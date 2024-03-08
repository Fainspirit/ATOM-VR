using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentRaycast : MonoBehaviour
{
    public Transform casterTransform;
    public Transform transformToMoveToHit;
    public LayerMask layerMask;

    private void Update()
    {
        Ray ray = new Ray(casterTransform.position, casterTransform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 9999, layerMask)) {
            // Move hand
            transformToMoveToHit.position = hit.collider.gameObject.transform.position;
            // Draw line
            Debug.DrawLine(casterTransform.position, hit.point, Color.blue);
        }
        else
        {
            Debug.DrawRay(casterTransform.position, casterTransform.forward, Color.red);
        }
    }
}
