using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{
    public virtual void SendMsg<T>(int msgType, T msg)
    {

    }

    public virtual void SendMsg<T>(Connection receiver, int msgType, T msg)
    {

    }

    public virtual void ReceiveMsg<T>(Connection sender, int msgType, T msg)
    {

    }

    protected U GetValue<T, U>(T value)
    {
        if (typeof(T) == typeof(U))
            return (U)Convert.ChangeType(value, typeof(U));
        else return default(U);
    }
}
