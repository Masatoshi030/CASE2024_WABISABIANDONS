using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Gen_Damage : EnemyState
{
    [SerializeField, Header("硬直時間")]
    float stiffnessTime = 3.0f;

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("HP0時の遷移")]
    StateKey hp0Key;
    [SerializeField, Header("ダメージ時の遷移")]
    StateKey damagedKey;
    [SerializeField, Header("経過時の遷移")]
    StateKey elapsedKey;

    public override void Enter()
    {
        base.Enter();
        enemy.IsDamaged = false;
        // ダメージを受けたという通知
        enemy.SendMsg<int>(0, 0);
        if(enemy.Hp <= 0)
        {
            machine.TransitionTo(hp0Key);
        }
    }

    public override void MainFunc()
    {
        base.MainFunc();
        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedKey);
        }
        else if(machine.Cnt >= stiffnessTime)
        {
            machine.TransitionTo(elapsedKey);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
