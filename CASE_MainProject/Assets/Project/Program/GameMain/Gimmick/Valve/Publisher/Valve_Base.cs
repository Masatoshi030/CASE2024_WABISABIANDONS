using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valve_Base : Publisher
{
    public enum Valve_Type
    { open, close }

    [SerializeField, Header("バルブタイプ"), Toolbar(typeof(Valve_Type))]
    Valve_Type type = Valve_Type.open;

    private int typeNunber = 0;  //バルブの種類を送信
    private int moveNunber = 1;

    static GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
        SendMsg<Valve_Type>(typeNunber, type);
    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            SendMsg<bool>(moveNunber, true);
            Debug.Log("作動");
        }
    }
}
