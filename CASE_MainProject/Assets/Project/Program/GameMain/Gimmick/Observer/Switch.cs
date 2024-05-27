using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// オブザーバーへの通知用
public class Switch : Connection
{
    [SerializeField, Header("オン/オフ"), ReadOnly]
    bool isActive = false;
    public bool IsActive { get => isActive; }

    [SerializeField, Header("適用先の回路")]
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
