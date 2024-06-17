using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnObserver : Publisher
{
    public override void ReceiveMsg<T>(Connection sender, int msgType, T msg)
    {
        if(msgType == 0)
        {
            SendMsg<int>(msgType, 0);
        }
    }
}
