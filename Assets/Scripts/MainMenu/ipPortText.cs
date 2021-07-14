using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ipPortText : MonoBehaviour
{
    private GameObject controller;
    private PrivateController controllerScript;
    private TextMesh text;
    public TextMesh labelText;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("PrivateController");
        controllerScript = controller.GetComponent<PrivateController>();
        text = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controllerScript.writeTo == 1)
        {
            text.text = controllerScript.ip;
            labelText.text = "Enter IP address:";
        }
        else if (controllerScript.writeTo == 2)
        {
            text.text = controllerScript.port;
            labelText.text = "Enter Port:";
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
