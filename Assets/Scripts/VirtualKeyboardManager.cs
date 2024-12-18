using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent( typeof( OVRVirtualKeyboardInputFieldTextHandler ) )]
public class VirtualKeyboardManager : MonoBehaviour
{
    private void FixKeyboardPosition()
    {
        transform.localPosition = new Vector3 (-0.5f, 1f, 0);
        transform.localRotation = Quaternion.identity;
    }

    private void Update()
    {
        FixKeyboardPosition();
    }

    public void SetInputField(GameObject inputField)
    {
        transform.GetComponent<OVRVirtualKeyboardInputFieldTextHandler>().InputField = inputField.GetComponent<InputField>();
    }
}
