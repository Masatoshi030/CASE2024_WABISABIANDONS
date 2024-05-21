using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_General_Idle : GimmickState
{
    [SerializeField, Header("待機時間")]
    float waitInterval;

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("時間経過後の遷移")]
    string elapsedTransition = "作動";

    public override void Enter()
    {
        base.Enter();
    }

    public override void MainFunc()
    {
        if (machine.Cnt >= waitInterval) machine.TransitionTo(elapsedTransition);
    }
}
