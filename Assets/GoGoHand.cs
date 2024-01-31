using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class GoGoHand : MonoBehaviour
{
    // The XR camera origin
    [SerializeField] Transform cameraOrigin;

    // The tracked hand whose position will be used to calculate the new position
    [SerializeField] Transform trueHand;

    [SerializeField] float baseMagnitude = 0.2f;
    [SerializeField] float scale = 6f;


    // Start is called before the first frame update
    void Start()
    {


        //inputActions.Enable();
        //inputActions.XRILeftHand.Enable();
        //inputActions.XRILeftHand.Position.started += Input_HandPositionUpdated;
    }

    private void Update()
    {
        UpdatePosition(trueHand.transform.position);
    }



    void UpdatePosition(Vector3 trueHandPosition)
    {
        Vector3 displacement = trueHandPosition - cameraOrigin.position;
        float offsetDistance = displacement.magnitude;

        // Calc offset now
        offsetDistance -= baseMagnitude;


        // calc scale
        transform.localScale = Vector3.one * (1 + offsetDistance * 5);

        // Now we have how far we are from it
        // Jump forward that difference and then offset it again
        offsetDistance *= scale;
        offsetDistance += baseMagnitude;

        Vector3 dir = displacement.normalized;
        Vector3 offset = dir * offsetDistance;

        transform.position = cameraOrigin.position + offset;
    }

    void Input_HandPositionUpdated(InputAction.CallbackContext c)
    {
        Debug.Log("pos changed");
        Vector3 trueHandPosition = c.ReadValue<Vector3>();
        UpdatePosition(trueHandPosition);
    }
}
