using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subscriber_Enable : Subscriber
{
    [SerializeField, Header("���삷��I�u�W�F�N�g")]
    GameObject targetObject;

    public override void ReceiveMsg<T>(Connection sender, int msgType, T msg)
    {
        if(msgType == 0)
        {
            bool value = GetValue<T, bool>(msg);
        }
    }
}
