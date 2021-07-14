using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileShield : Shield
{
    PublicPlayerController publicPlayerController;
    private void Start()
    {
        transform.localScale *= nSize.Value;
        PublicPlayerController[] publicPlayerControllers = GameObject.FindObjectsOfType<PublicPlayerController>();
        foreach(PublicPlayerController i in publicPlayerControllers)
        {
            if (i.id.Value == id.Value)
            {
                publicPlayerController = i;
            }
        }
    }
    void Update()
    {
        if (IsServer)
        {
            DecreaseTimer();
        }
        if (hand.Value == "Right")
        {
            transform.SetPositionAndRotation(publicPlayerController.privateRightHandPos, publicPlayerController.rightHandRot.Value);
            transform.Translate(Vector3.forward * 0.5f);
        }
        else if (hand.Value == "Left")
        {
            transform.SetPositionAndRotation(publicPlayerController.privateLeftHandPos, publicPlayerController.leftHandRot.Value);
            transform.Translate(Vector3.forward * 0.5f);
        }
    }
}
