using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnObserver : Publisher
{
    [SerializeField, Header("’Ê’mŠÔŠu")]
    float msgDuration = 2.0f;
    float myCnt;
    [SerializeField, Header("’Ê’m‚ðŽó‚¯Žæ‚Á‚½‚©"), ReadOnly]
    bool isGetMsg = false;

    public override void ReceiveMsg<T>(Connection sender, int msgType, T msg)
    {
        if(msgType == 1)
        {
            isGetMsg = true;
        }
    }

    private void Update()
    {
        if(isGetMsg)
        {
            myCnt += Time.deltaTime;
            if(myCnt > msgDuration)
            {
                isGetMsg = false;
                myCnt = 0.0f;
                SendMsg<int>(1, 0);
            }
        }
    }
}
