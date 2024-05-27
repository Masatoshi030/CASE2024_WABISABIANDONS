using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Publisher_Switch : Publisher
{
    [SerializeField, Header("スイッチ")]
    List<Switch> switches;
    public void ReceiveMsg<T>(Switch obj, int msgType, T msg)
    {
        if(msgType == 0)
        {
            if (switches.Contains(obj))
            {
                int index = switches.IndexOf(obj);
                SendMsg<T>(msgType, msg, index);
            }
        }
    }
}
