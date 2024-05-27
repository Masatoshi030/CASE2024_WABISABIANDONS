using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Publisher : Connection
{
    [SerializeField, Header("通知先リスト")]
    protected List<Subscriber> subscribers;

    /*
     * <summary>
     * 登録関数
     * <param>
     * Subscriber subscriber
     * <return>
     * bool success
     */
    public virtual bool Subscribe(Subscriber subscriber)
    {
        if(subscribers.Contains(subscriber))
        {
            // すでに含んでる
            return false;
        }
        else
        {
            // 登録できた
            subscribers.Add(subscriber);
            return true;
        }
    }

    // 解除
    public virtual void UnSubscribe(Subscriber subscriber)
    {
        subscribers.Remove(subscriber);
    }

    public override void SendMsg<T>(int msgType, T msg)
    {
        // 通知を行う
        for (int i = 0; i < subscribers.Count; i++)
        {
            subscribers[i].ReceiveMsg<T>(this, msgType, msg);
        }
    }

    public virtual void SendMsg<T>(int msgType, T msg, int address)
    {
        if (subscribers.Count > address)
        {
            subscribers[address].ReceiveMsg<T>(this, msgType, msg);
        }
    }
}
