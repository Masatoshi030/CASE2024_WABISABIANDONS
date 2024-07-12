using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationObserver : Publisher
{
    public void EndAnimation(string name)
    {
        Boss.AnimationMsg msg;
        msg.booleanValue = false;
        msg.booleanName = name;
        SendMsg(1, msg);
    }
}
