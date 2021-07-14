using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageItem : NetworkBehaviour
{
    public bool active;
    public bool wasGrabbed = false;
    public PrivatePlayerController prPC;
    public PublicPlayerController puPC;
    public Storage storage;
    public bool isLocal = false;
    public Vector3 offsetDistance;
    public Vector3 offsetRotation;
    public WandController wandController = null;
    public string hand;


    // Update is called once per frame
    private void Start()
    {
        CustomStart();
    }
    void Update()
    {
        if (isLocal)
        {
            if (!active)
            {
                if (prPC.rightGripState && (puPC.privateRightHandPos - gameObject.transform.position).sqrMagnitude < 0.01f && !prPC.rightHandOccupied)
                {
                    hand = "Right";
                    active = true;
                    prPC.rightHandOccupied = true;
                }
                else if (prPC.leftGripState && (puPC.privateLeftHandPos - gameObject.transform.position).sqrMagnitude < 0.01f && !prPC.leftHandOccupied)
                {
                    hand = "Left";
                    active = true;
                    prPC.leftHandOccupied = true;
                }
            }
            else if (wasGrabbed)
            {
                if ((!prPC.rightGripState) && (hand == "Right"))
                {
                    
                    active = false;
                    wasGrabbed = false;
                    prPC.rightHandOccupied = false;
                    if (wandController != null)
                    {
                        prPC.m_Recognizer.Stop();
                    }
                    if (!prPC.gameObject.GetComponentInChildren<Storage>().active)
                    {
                        puPC.MoveItemToHandServerRpc(0, hand);
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        puPC.MoveItemToHandServerRpc(-1, hand);
                    }
                }
                else if ((!prPC.leftGripState) && (hand == "Left"))
                {
                    active = false;
                    wasGrabbed = false;
                    prPC.leftHandOccupied = false;
                    if (wandController != null)
                    {
                        prPC.m_Recognizer.Stop();
                    }
                    if (!prPC.gameObject.GetComponentInChildren<Storage>().active)
                    {
                        puPC.MoveItemToHandServerRpc(0, hand);
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        puPC.MoveItemToHandServerRpc(-1, hand);
                    }
                }
            }
            else
            {
                puPC.MoveItemToHandServerRpc(storage.storageItems.IndexOf(gameObject.GetComponent<StorageItem>()) + 1, hand);
                if (wandController != null)
                    prPC.activeWand = wandController;
                    prPC.m_Recognizer.Start();
                    wasGrabbed = true;
            }
        }
    }
    public void CustomStart()
    {
        storage = prPC.GetComponentInChildren<Storage>();
        if (gameObject.GetComponent<WandController>())
        {
            wandController = GetComponent<WandController>();
        }
    }
}
