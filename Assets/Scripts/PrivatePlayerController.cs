using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class PrivatePlayerController : MonoBehaviour
{
    public bool xButtonState;
    public bool leftGripState;
    public bool rightGripState;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject head;
    public WandController activeWand;
    public PublicPlayerController publicPlayerScript;

    public bool leftHandOccupied = false;
    public bool rightHandOccupied = false;

    List<UnityEngine.XR.InputDevice> leftHandedControllers = new List<UnityEngine.XR.InputDevice>();
    UnityEngine.XR.InputDeviceCharacteristics leftDesiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
    List<UnityEngine.XR.InputDevice> rightHandedControllers = new List<UnityEngine.XR.InputDevice>();
    UnityEngine.XR.InputDeviceCharacteristics rightDesiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller;

    public string[] keywords;
    public KeywordRecognizer m_Recognizer;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(leftDesiredCharacteristics, leftHandedControllers);
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(rightDesiredCharacteristics, rightHandedControllers);
        m_Recognizer = new KeywordRecognizer(keywords);
        m_Recognizer.OnPhraseRecognized += KeywordRecognized;
    }

    // Update is called once per frame
    void Update()
    {
        bool tempLeftGripState = false;
        bool tempXButtonState = false;
        foreach (var device in leftHandedControllers)
        {
            leftGripState = device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out tempLeftGripState) && tempLeftGripState;
            xButtonState = device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out tempXButtonState) && tempXButtonState;
        }
        bool tempRightGripState = false;
        foreach (var device in rightHandedControllers)
        {
            rightGripState = device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out tempRightGripState) && tempRightGripState;
        }

        //Debugging
        if (publicPlayerScript == null)
        {
            Debugger.LogError("PublicPlayerController destroyed");
        }
        publicPlayerScript.lastUpdate += Time.deltaTime;
        if (publicPlayerScript.lastUpdate > 5)
        {
            Debugger.LogError("5 seconds since last update");
        }
    }

    public void KeywordRecognized(PhraseRecognizedEventArgs args)
    {
        activeWand.OnPhraseRecognized(args);
    }
}
