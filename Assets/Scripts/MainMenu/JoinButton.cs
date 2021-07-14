using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinButton : Button
{
    public override void ButtonPressed()
    {
        controllerScript.StartAsClient();
    }
}
