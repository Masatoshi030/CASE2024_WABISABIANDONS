using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_General_Idle : GimmickState
{
    [SerializeField, Header("‘Ò‹@ŽžŠÔ")]
    float waitInterval;

    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("ŽžŠÔŒo‰ßŒã‚Ì‘JˆÚ")]
    string elapsedTransition = "ì“®";

    public override void Enter()
    {
        base.Enter();
    }

    public override void MainFunc()
    {
        if (machine.Cnt >= waitInterval) machine.TransitionTo(elapsedTransition);
    }
}
