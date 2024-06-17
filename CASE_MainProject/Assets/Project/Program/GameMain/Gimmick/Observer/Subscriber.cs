using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Subscriber : Connection
{
    [SerializeField, Header("í ímå≥ÉäÉXÉg")]
    protected List<Publisher> observers;
    public List<Publisher> Observers { get => observers; }

    public bool Subscribe(Publisher observer)
    {
        if(observer.Subscribe(this))
        {
            observers.Add(observer);
            return true;
        }
        return false;
    }

    public void UnSubscribe(Publisher observer)
    {
        observer.UnSubscribe(this);
        observers.Remove(observer);
    }

    public override void SendMsg<T>(int msgType, T msg)
    {
        for(int i = 0; i < observers.Count; i++)
        {
            if(observers[i] != null)
            {
                observers[i].ReceiveMsg<T>(this, msgType, msg);
            }
        }
    }
}
