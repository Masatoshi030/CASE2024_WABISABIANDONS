using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Publisher : MonoBehaviour
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

    public virtual void NortifyToSubscribers<T>(int MsgType, T value)
    {
        // �ʒm���s��
        for (int i = 0; i < subscribers.Count; i++)
        {
            subscribers[i].ReceiveMsg<T>(this, MsgType, value);
        }
    }
}
