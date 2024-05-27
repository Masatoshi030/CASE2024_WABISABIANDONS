using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Publisher : Connection
{
    [SerializeField, Header("�ʒm�惊�X�g")]
    protected List<Subscriber> subscribers;

    /*
     * <summary>
     * �o�^�֐�
     * <param>
     * Subscriber subscriber
     * <return>
     * bool success
     */
    public virtual bool Subscribe(Subscriber subscriber)
    {
        if(subscribers.Contains(subscriber))
        {
            // ���łɊ܂�ł�
            return false;
        }
        else
        {
            // �o�^�ł���
            subscribers.Add(subscriber);
            return true;
        }
    }

    // ����
    public virtual void UnSubscribe(Subscriber subscriber)
    {
        subscribers.Remove(subscriber);
    }

    public override void SendMsg<T>(int msgType, T msg)
    {
        // �ʒm���s��
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
