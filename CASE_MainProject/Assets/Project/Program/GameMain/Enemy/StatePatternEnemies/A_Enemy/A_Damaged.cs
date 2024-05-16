using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Damaged : EnemyState
{
    [SerializeField, Header("�d������")]
    float stiffnessTime;
    bool bUpdate = true;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�o�ߌ�̑J��")]
    string elapsedTransition = "�ҋ@";
    [SerializeField, Header("HP��0�̑J��")]
    string noHitPointTransition = "���S";

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
