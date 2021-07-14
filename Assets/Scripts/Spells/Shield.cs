using MLAPI;
using MLAPI.NetworkVariable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : NetworkBehaviour
{
    public float health;
    public NetworkVariableFloat nSize = new NetworkVariableFloat(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly, ReadPermission = NetworkVariablePermission.Everyone, SendTickrate = -1 });
    public NetworkVariableFloat id = new NetworkVariableFloat(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly, ReadPermission = NetworkVariablePermission.Everyone, SendTickrate = -1 });
    public NetworkVariableString hand = new NetworkVariableString(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly, ReadPermission = NetworkVariablePermission.Everyone, SendTickrate = -1 });
    public float timer;

    private void Start()
    {
        transform.localScale *= nSize.Value;
    }
    private void Update()
    {
        if (IsServer)
        {
            DecreaseTimer();
        }

    }

    public void DecreaseTimer()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}

