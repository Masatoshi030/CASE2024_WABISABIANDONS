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
    int elapsedID;
    [SerializeField, Header("��e���̑J��")]
    int damagedID;
    [SerializeField, Header("HP��0�̑J��")]
    int noHitPointID;

    public override void Enter()
    {
        enemy.IsDamaged = false;
        base.Enter();
        if(enemy.Hp <= 0)
        {
            machine.TransitionTo(noHitPointID);
            bUpdate = false;
        }
        enemy.IsInvincible = true;
    }

    public override void MainFunc()
    {
        base.MainFunc();
        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedID);
            return;
        }

        if (machine.Cnt >= stiffnessTime)
        {
            machine.TransitionTo(elapsedID);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void CollisionEnterOpponent(Collision collision)
    {
        if (collision.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedID);
        }
    }

    public override void TriggerEnterOpponent(Collider other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedID);
        }
    }
}
