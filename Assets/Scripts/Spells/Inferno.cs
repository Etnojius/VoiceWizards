using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inferno : Projectile
{
    bool raining = false;
    float timer = 0.01f;
    int fireballsLaunched = 0;
    public GameObject fireball;
    private void Update()
    {
        BaseUpdate();
        if (raining)
        {
            if (timer <= 0)
            {
                Projectile tempFireball = Instantiate(fireball, transform.position, transform.rotation).GetComponent<Projectile>();
                tempFireball.gameObject.transform.rotation = Quaternion.LookRotation(Vector3.down, Vector3.forward);
                tempFireball.transform.Translate(Vector3.forward * -10);
                tempFireball.transform.Translate(new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0));
                tempFireball.nSpeed.Value = nSpeed.Value / 2;
                tempFireball.nSize.Value = nSize.Value;
                tempFireball.damage = damage;
                tempFireball.GetComponent<NetworkObject>().Spawn();
                fireballsLaunched += 1;
                if (fireballsLaunched > 1000)
                {
                    Destroy(gameObject);
                }
                timer = 0.01f;
            }
            timer -= Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Shield"))
            StopClientRpc(transform.position);
            raining = true;
        }
    }

    [ClientRpc]
    void StopClientRpc(Vector3 position)
    {
        active = false;
        transform.position = position;
    }
}
