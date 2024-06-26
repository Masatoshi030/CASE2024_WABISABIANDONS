using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 
public class State_A_Interval : EnemyState
{
    [SerializeField, Header("待ち時間")]
    float waitTime = 3.0f;

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("経過時の遷移")]
    StateKey elapsedKey;
    [SerializeField, Header("ダメージ時の遷移")]
    StateKey damagedKey;

    public override void Enter()
    {
        base.Enter();
        enemy.IsVelocityZero = true;
    }

    public override void MainFunc()
    {
        base.MainFunc();

        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedKey);
        }
        else if(machine.Cnt >= waitTime)
        {
            machine.TransitionTo(elapsedKey);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.IsVelocityZero = false;
    }
}
