using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : NetworkBehaviour
{
    public GameObject player;
    public GameObject test;
    // Start is called before the first frame update
    void Start()
    {
        test = Instantiate(player, player.transform.position, player.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
