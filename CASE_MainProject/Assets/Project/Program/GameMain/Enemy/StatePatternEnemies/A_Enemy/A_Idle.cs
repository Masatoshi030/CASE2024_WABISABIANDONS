using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Idle : EnemyState
{
    [SerializeField, Header("�ҋ@����")]
    float waitInterval = 3.0f;
    [SerializeField, Header("�o�ߌ�̑J��")]
    public string elapsedTransition = "�ړ�";
    [SerializeField, Header("���G�������̑J��")]
    public string serchSuccessTransition = "�ǐ�";
    [SerializeField, Header("��e���̑J��")]
    public string damagedTransition = "��e";
    [SerializeField, Header("�Փˎ��̑J��")]
    public string collisionTransition = "�ǐ�";

    public override void Enter()
    {
        base.Enter();
    }

    public override void MainFunc()
    {
        if(enemy.IsFindPlayer)
        {
            Machine.TransitionTo(serchSuccessTransition);
        }

        if(machine.Cnt >= waitInterval)
        {
            Machine.TransitionTo(elapsedTransition);
        }
    }

    public override void Exit()
    {
        
    }

    public override void CollisionEnter(Collision collision)
    {
        if (collision.transform.root.name == "Player")
        {
            machine.TransitionTo(collisionTransition);
        }
    }

    public override void TriggerEnter(Collider collider)
    {
        if (collider.transform.root.name == "Player")
        {
            machine.TransitionTo(collisionTransition);
        }
    }
}
