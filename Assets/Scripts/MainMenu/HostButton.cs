using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostButton : Button
{
    public override void ButtonPressed()
    {
        controllerScript.StartAsHost();
    }
}
