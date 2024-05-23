using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Publisher : MonoBehaviour
{
    [SerializeField, Header("’Ê’mæƒŠƒXƒg")]
    protected List<Subscriber> subscribers;

    /*
     * <summary>
     * “o˜^ŠÖ”
     * <param>
     * Subscriber subscriber
     * <return>
     * bool success
     */
    public virtual bool Subscribe(Subscriber subscriber)
    {
        if(subscribers.Contains(subscriber))
        {
            // ‚·‚Å‚ÉŠÜ‚ñ‚Å‚é
            return false;
        }
        else
        {
            // “o˜^‚Å‚«‚½
            subscribers.Add(subscriber);
            return true;
        }
    }

    // ‰ğœ
    public virtual void UnSubscribe(Subscriber subscriber)
    {
        subscribers.Remove(subscriber);
    }

    public virtual void NortifyToSubscribers<T>(int MsgType, T value)
    {
        // ’Ê’m‚ğs‚¤
        for (int i = 0; i < subscribers.Count; i++)
        {
            subscribers[i].ReceiveMsg<T>(this, MsgType, value);
        }
    }
}
