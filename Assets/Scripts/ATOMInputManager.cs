using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class ATOMInputManager : MonoBehaviour
{
    [SerializeField] GoGoHand GoGoHand;
    [SerializeField] TransformFollower LocalCopyHandFollower;

    [SerializeField] InputActionReference SelectActionReference;

    // Start is called before the first frame update
    void Start()
    {
        // Apparently the position changed events only fire at the start and end of the program... useless!
        //InputAction posChange = inputActionAsset.FindAction("XRI LeftHand/Position");
        //posChange.started += Input_HandPositionUpdated;   

        // Grip / select
        InputAction selectAction = SelectActionReference.ToInputAction();
        selectAction.started += Input_Select;
        selectAction.canceled += Input_Select;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Input_Select(InputAction.CallbackContext c)
    {
        Debug.Log("Select state changed!");
        bool pressed = c.ReadValueAsButton();

        if(pressed)
        {
            Lock();
        }
        else
        {
            Unlock();
        }
    }

    void Lock()
    {
        Debug.Log("Locking positions!");

        GoGoHand.enabled = false;
        LocalCopyHandFollower.enabled = false;
    }

    void Unlock()
    {
        Debug.Log("Unlocking positions!");

        GoGoHand.enabled = true;
        LocalCopyHandFollower.enabled = true;
    }
}
