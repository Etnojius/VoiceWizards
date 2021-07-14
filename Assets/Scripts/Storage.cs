using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public PrivatePlayerController prPC;
    public PublicPlayerController puPC;
    public List<StorageItem> storageItems = new List<StorageItem>();
    public GameObject player;
    public Vector3[] positions;
    public bool active = false;
    private Quaternion baseRot;

    private void Start()
    {
        baseRot = transform.rotation;
        puPC = prPC.publicPlayerScript;
    }
    private void Update()
    {
        if (active)
        {
            if (prPC.xButtonState)
            {
                int n = 0;
                foreach (StorageItem i in storageItems)
                {
                    if (!i.active)
                    {
                        i.transform.SetPositionAndRotation(transform.position, transform.rotation);
                        i.transform.Translate(positions[n]);
                        i.transform.rotation = baseRot;
                    }
                    n++;
                }
            }
            else
            {
                foreach (StorageItem i in storageItems)
                {
                    if (i.active)
                    {
                    
                    }
                    else
                    {
                        i.gameObject.SetActive(false);
                    }
                }
                active = false;
                if (!prPC.rightHandOccupied)
                {
                    puPC.MoveItemToHandServerRpc(0, "Right");
                }
                if (!prPC.leftHandOccupied)
                {
                    puPC.MoveItemToHandServerRpc(0, "Left");
                }
            }
        }
        else if (prPC.xButtonState)
        {
            active = true;
            transform.SetPositionAndRotation(player.transform.position, player.transform.rotation);
            foreach (StorageItem i in storageItems)
            {
                int n = 0;
                if (!i.gameObject.activeInHierarchy)
                {
                    i.gameObject.SetActive(true);
                    i.transform.SetPositionAndRotation(transform.position, transform.rotation);
                    i.transform.Translate(positions[n]);
                    i.transform.rotation = baseRot;
                }
                n++;
            }
        }


    }
}

