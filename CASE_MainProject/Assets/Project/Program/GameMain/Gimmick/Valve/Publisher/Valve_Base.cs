using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valve_Base : Publisher
{
    static GameObject player;

    [SerializeField, Header("MsgTypeî‘çÜ")]
    int msgTypeNunber;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject==player)
        {
            SendMsg<int>(msgTypeNunber, 1);
        }
    }


}
