using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : NetworkBehaviour
{
    public NetworkVariableFloat nSpeed = new NetworkVariableFloat(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly, ReadPermission = NetworkVariablePermission.Everyone, SendTickrate = -1 });
    public float speed;
    public NetworkVariableFloat nSize = new NetworkVariableFloat(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly, ReadPermission = NetworkVariablePermission.Everyone, SendTickrate = -1 });
    public float damage;
    public float lifetime;
    public float spinSpeed;
    public Vector3 locationOffset;
    public Vector3 rotationOffset;
    public bool active;

    private void Awake()
    {
        transform.Translate(locationOffset);
        transform.Rotate(rotationOffset);
    }

    private void Start()
    {
        speed *= nSpeed.Value;
        transform.localScale *= nSize.Value;
    }

    private void Update()
    {
        BaseUpdate();
    }

    [ClientRpc]
    public void ActivateClientRPC(bool state)
    {
        active = state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PublicPlayerController tempPlayer = other.gameObject.GetComponentInParent<CollisionLink>().linkedObject.GetComponent<PublicPlayerController>();
                if (tempPlayer.hP.Value > damage)
                {
                    tempPlayer.hP.Value -= damage;
                    damage = 0;
                    Destroy(gameObject);
                }
                else
                {
                    float tempFloat = tempPlayer.hP.Value;
                    tempPlayer.hP.Value = 0;
                    damage -= tempFloat;
                }
            }
            else if (other.gameObject.CompareTag("Shield"))
            {
                Shield tempShield = other.gameObject.GetComponentInParent<Shield>();
                if (tempShield.health > damage)
                {
                    tempShield.health -= damage;
                    damage = 0;
                    Destroy(gameObject);
                }
                else
                {
                    float tempFloat = tempFloat = tempShield.health;
                    tempShield.health = 0;
                    Destroy(tempShield.gameObject);
                    damage -= tempFloat;

                }
            }
        }
    }
    public void BaseUpdate()
    {
        if (active)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.Rotate(Vector3.forward * Time.deltaTime * spinSpeed);
            lifetime -= Time.deltaTime;
            if ((lifetime < 0 || damage <= 0) && IsServer)
            {
                Destroy(gameObject);
            }
        }
    }
}
