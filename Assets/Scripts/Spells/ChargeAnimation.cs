using MLAPI.NetworkVariable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAnimation : MonoBehaviour
{
    public NetworkVariableColor nColor = new NetworkVariableColor(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly, ReadPermission = NetworkVariablePermission.Everyone, SendTickrate = -1 });
    private float ups = 30;

    // Update is called once per frame
    private void Start()
    {
        GetComponent<Renderer>().material.SetColor("_Color", nColor.Value);
    }
    void Update()
    {
        if (ups > 0)
        {
            transform.Translate(Vector3.up * 0.04f);
            ups -= 1;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
