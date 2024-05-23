using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Subscriber : MonoBehaviour
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

    public virtual void ReceiveMsg<T>(Publisher observer, int MsgType,  T value)
    {
        Debug.Log(value);
    }

    protected U GetValue<T, U>(T value)
    {
        if (typeof(T) == typeof(U))
            return (U)Convert.ChangeType(value, typeof(U));
        else return default(U);
    }
}
