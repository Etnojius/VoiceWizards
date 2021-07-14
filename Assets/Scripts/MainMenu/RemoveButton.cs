using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveButton : Button
{
    public override void ButtonPressed()
    {
        if (controllerScript.writeTo == 1)
        {
            controllerScript.ip = controllerScript.ip.Substring(0, controllerScript.ip.Length - 1);
        }
        else if (controllerScript.writeTo == 2)
        {
            controllerScript.port = controllerScript.port.Substring(0, controllerScript.port.Length - 1);
        }
        Debug.Log("button pressed");
    }
}
