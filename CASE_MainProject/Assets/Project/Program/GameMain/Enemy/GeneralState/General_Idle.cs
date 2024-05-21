using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Idle : EnemyState
{
    [SerializeField, Header("�ҋ@����")]
    float waitInterval = 3.0f;
    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("���Ԍo�ߌ�̑J��")]
    public string elapsedTransition = "�ړ�";
    [SerializeField, Header("���G�������̑J��")]
    public string searchSuccessTransition = "�ǐ�";
    [SerializeField, Header("��e���̑J��")]
    public string damagedTransition = "��e";
    [SerializeField, Header("�Փˎ��̑J��")]
    public string collisionTransition = "�ǐ�";

    public override void Enter()
    {
        base.Enter();
        enemy.EnemyRigidbody.velocity = Vector3.zero;
    }

    public override void MainFunc()
    {
        if(enemy.IsFindPlayer)
        {
            machine.TransitionTo(searchSuccessTransition);
        }
        else if(enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedTransition);
        }
        else if(machine.Cnt >= waitInterval)
        {
            machine.TransitionTo(elapsedTransition);
        }
    }

    public override void Exit()
    {
        
    }

    public override void CollisionEnterSelf(GameObject other)
    {
        if(other.name == "Player")
        {
            machine.TransitionTo(collisionTransition);
        }
    }

    public override void TriggerEnterSelf(GameObject other)
    {
        if (other.name == "Player")
        {
            machine.TransitionTo(collisionTransition);
        }
    }
}
