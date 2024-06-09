using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC_Idle : EnemyState_C
{
    [SerializeField, Header("待機時間")]
    float waitInterval;

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("時間経過後の遷移ID")]
    int elapsedID;
    [SerializeField, Header("被弾時の遷移")]
    int damagedID;

    public override void Enter()
    {
        base.Enter();
    }

    public override void MainFunc()
    {
        base.MainFunc();

        if (enemy.IsDamaged)
        {
            machine.TransitionTo(damagedID);
            return;
        }
        else if(machine.Cnt >= waitInterval)
        {
            machine.TransitionTo(elapsedID);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
