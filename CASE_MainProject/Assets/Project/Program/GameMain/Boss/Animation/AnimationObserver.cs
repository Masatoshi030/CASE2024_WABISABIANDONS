using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationObserver : Publisher
{
    public void EndAnimation(int msgType)
    {
        SendMsg<int>(msgType, 0);
    }
}
