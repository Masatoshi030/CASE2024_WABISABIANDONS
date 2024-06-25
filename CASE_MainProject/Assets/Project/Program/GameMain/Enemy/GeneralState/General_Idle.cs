using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Idle : EnemyState
{
    [SerializeField, Header("�ҋ@����")]
    float waitInterval = 3.0f;
    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("���Ԍo�ߌ�̑J��")]
    public int elapsedID;
    [SerializeField, Header("���G�������̑J��")]
    public int searchSuccessID;
    [SerializeField, Header("��e���̑J��")]
    public int damagedID;
    [SerializeField, Header("�Փˎ��̑J��")]
    public int collisionID;

    public override void Enter()
    {
        base.Enter();
        enemy.IsVelocityZero = true;
    }

    public override void MainFunc()
    {
        base.MainFunc();

        if(enemy.IsFindPlayer)
        {
            machine.TransitionTo(searchSuccessID);
            enemy.SendMsg<int>(0, 0);
        }
        else if(enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedID);
            enemy.SendMsg<int>(1, 0);
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

    public override void CollisionEnterSelf(Collision collision)
    {
        if(collision.transform.name == "Player")
        {
            machine.TransitionTo(collisionID);
        }
    }

    public override void TriggerEnterSelf(Collider other)
    {
        if (other.name == "Player")
        {
            machine.TransitionTo(collisionID);
        }
    }
}
