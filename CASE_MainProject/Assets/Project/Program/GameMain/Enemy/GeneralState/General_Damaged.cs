using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Damaged : EnemyState
{
    [SerializeField, Header("�d������")]
    float stiffnessTime;
    bool bUpdate = true;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�o�ߌ�̑J��")]
    string elapsedTransition = "�ҋ@";
    [SerializeField, Header("��e���̑J��")]
    string damagedTransition = "��e";
    [SerializeField, Header("HP��0�̑J��")]
    string noHitPointTransition = "���S";

    public override void Enter()
    {
        base.Enter();
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

    public override void CollisionEnterOpponent(GameObject other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedTransition);
        }
    }

    public override void TriggerEnterOpponent(GameObject other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedTransition);
        }
    }
}
