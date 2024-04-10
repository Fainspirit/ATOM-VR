using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// Implements https://dl.acm.org/doi/pdf/10.1145/237091.237102
public class OfficialGoGoHand : MonoBehaviour
{
    // The XR camera origin
    [SerializeField] Transform xrOrigin;
    InputDevice HMD;

    // The tracked hand whose position will be used to calculate the new position
    [SerializeField] Transform trueHand;

    // Linear/nonlinear threshold
    // Original paper uses 2/3 of arm length
    // My old code used 0.3m
    [SerializeField] float D = 0.3f;
    
    // They don't explain it well, but the graph they give uses 1/6
    [SerializeField, ] float k = 1.0f/6.0f;

    // Not part of the paper, but used for visual ease
    [SerializeField] float sizeScale = 25;

    private void Awake()
    {
        HMD = InputDevices.GetDeviceAtXRNode(XRNode.Head);
    }

    private void Update()
    {
        UpdatePosition(trueHand.transform.position);
    }

    // Length of virtual arm
    float F(float Rr)
    {
        if (Rr < D) return Rr;

        float RrMinusD = Rr - D; 
        return Rr + k * (RrMinusD * RrMinusD);
    }

    void UpdatePosition(Vector3 trueHandPosition)
    {
        Vector3 centereyePos;
        HMD.TryGetFeatureValue(CommonUsages.devicePosition, out centereyePos);
        centereyePos += xrOrigin.transform.position;

        Vector3 offset = trueHandPosition - centereyePos;

        // Real length
        float Rr = offset.magnitude;

        Vector3 dir = offset.normalized;
        
        // Virtual length
        float Rv = F((float)Rr);

        Vector3 virtualOffset = dir * Rv;


        // Not in original paper, included here for ease of perception
        // calc scale
        transform.localScale = Vector3.one * (1 + Rv * sizeScale);

        // Now we have how far we are from it
        // Jump forward that difference and then offset it again

        transform.position = centereyePos + virtualOffset;
    }
}


