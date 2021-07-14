using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterButton : Button
{
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            bRenderer.material.SetColor("_Color", Color.green);
        }
    }

    public override void ButtonPressed()
    {
        controllerScript.writeTo++;
    }
}
