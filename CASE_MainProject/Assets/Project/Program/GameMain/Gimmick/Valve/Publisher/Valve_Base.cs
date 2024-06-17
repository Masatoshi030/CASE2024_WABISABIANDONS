using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Valve_Base : Publisher
{
    public enum Valve_Type
    { open, close }

    [SerializeField, Header("�o���u�^�C�v"), Toolbar(typeof(Valve_Type))]
    Valve_Type type = Valve_Type.close;

    private int typeNunber = 0;  //�o���u�̎�ނ𑗐M
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

    public void SetCommand()
    {
        if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
        {
            SendMsg<bool>(moveNunber, true);
            Debug.Log("�쓮");
        }
    }
}
