using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    private LineRenderer lr;
    private Button button;

    public bool pressedLastFrame = false;
    public bool pressed;
    public bool tempPressed2;

    List<UnityEngine.XR.InputDevice> rightHandedControllers = new List<UnityEngine.XR.InputDevice>();
    UnityEngine.XR.InputDeviceCharacteristics rightDesiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(rightDesiredCharacteristics, rightHandedControllers);
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                lr.SetPosition(1, hit.point);
                if (hit.collider.gameObject.CompareTag("Button"))
                {
                    button = hit.collider.gameObject.GetComponent<Button>();
                    button.HoveredOver();
                    if (pressedLastFrame)
                    {
                        button.ButtonPressed();
                    }
                }
            }
        }
        else lr.SetPosition(1, transform.forward * 5000);

        bool tempPressed = false;
        foreach (var device in rightHandedControllers)
        {
            tempPressed2 = device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out tempPressed) && tempPressed;
        }
        if (tempPressed && !pressed)
        {
            pressedLastFrame = true;
        }
        else
        {
            pressedLastFrame = false;
        }
        pressed = tempPressed2;
    }
}
