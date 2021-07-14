using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject controller;
    public PrivateController controllerScript;
    public string letter;
    public Renderer bRenderer;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("PrivateController");
        controllerScript = controller.GetComponent<PrivateController>();
        bRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            bRenderer.material.SetColor("_Color", Color.white);
        }
    }

    virtual public void ButtonPressed()
    {
        if (controllerScript.writeTo == 1)
        {
            controllerScript.ip += letter;
        }
        else if (controllerScript.writeTo == 2)
        {
            controllerScript.port += letter;
        }
    }

    public void HoveredOver()
    {
        bRenderer.material.SetColor("_Color", Color.yellow);
        timer = 0.1f;
    }
}
