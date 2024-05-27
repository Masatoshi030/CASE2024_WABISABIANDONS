using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �I�u�U�[�o�[�ւ̒ʒm�p
public class Switch : Connection
{
    [SerializeField, Header("�I��/�I�t"), ReadOnly]
    bool isActive = false;
    public bool IsActive { get => isActive; }

    [SerializeField, Header("�K�p��̉�H")]
    List<Circuit_ActiveOperation> circuits;
    public override void SendMsg<T>(int msgType, T msg)
    {
        if(msgType == 0)
        {
            bool val = GetValue<T, bool>(msg);
            isActive = val;
        }

        for(int i = 0; i < circuits.Count; i++)
        {
            circuits[i].Operate(this);
        }
    }
}
