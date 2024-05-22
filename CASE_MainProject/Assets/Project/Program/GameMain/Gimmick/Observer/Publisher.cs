using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Publisher : MonoBehaviour
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

    public virtual void NortifyToSubscribers<T>(int MsgType, T value)
    {
        // 通知を行う
        for (int i = 0; i < subscribers.Count; i++)
        {
            subscribers[i].ReceiveMsg<T>(this, MsgType, value);
        }
    }
}
