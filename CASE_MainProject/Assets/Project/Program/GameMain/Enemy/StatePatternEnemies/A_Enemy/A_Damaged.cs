using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Damaged : EnemyState
{
    [SerializeField, Header("硬直時間")]
    float stiffnessTime;
    bool bUpdate = true;

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("経過後の遷移")]
    string elapsedTransition = "待機";
    [SerializeField, Header("HPが0の遷移")]
    string noHitPointTransition = "死亡";

    public override void Enter()
    {
        base.Enter();
        enemy.Damage(10.0f, Vector3.zero);
        if(enemy.Hp <= 0)
        {
            machine.TransitionTo(noHitPointTransition);
            bUpdate = false;
        }
        enemy.IsInvincible = true;
    }

    public override void MainFunc()
    {
        if(bUpdate) if (machine.Cnt >= stiffnessTime) machine.TransitionTo(elapsedTransition);
    }

    public override void Exit()
    {
        
    }
}
