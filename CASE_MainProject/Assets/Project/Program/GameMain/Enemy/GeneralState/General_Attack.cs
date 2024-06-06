using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Attack : EnemyState
{
    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("時間経過後の遷移")]
    int elapsedID;

    [SerializeField, Header("被弾時の遷移")]
    int damegedID;


    public override void Enter()
    {
        base.Enter();
    }

    public override void MainFunc()
    {
        base.MainFunc();
        if (!machine.IsUpdate) return;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
