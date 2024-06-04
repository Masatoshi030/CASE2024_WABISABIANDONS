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
        enemy.EnemyRigidbody.velocity = Vector3.zero;
    }

    public override void MainFunc()
    {
        if(enemy.IsFindPlayer)
        {
            machine.TransitionTo(searchSuccessID);
        }
        else if(enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedID);
        }
        else if(machine.Cnt >= waitInterval)
        {
            machine.TransitionTo(elapsedID);
        }
    }

    public override void Exit()
    {
        
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
