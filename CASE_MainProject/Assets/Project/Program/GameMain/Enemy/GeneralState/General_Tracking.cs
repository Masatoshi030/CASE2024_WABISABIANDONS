using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Tracking : EnemyState
{
    [SerializeField, Header("�p�g���[��"), ReadOnly]
    NavMeshPatrol patrol;
    [SerializeField, Header("�ړ����x")]
    float moveSpeed;
    [SerializeField, Header("�����x")]
    float acceleration;
    [SerializeField, Header("��]���x")]
    float angularSpeed;
    [SerializeField, Header("�ǐՎ���")]
    float trackingInterval;
    [SerializeField, Header("�ړ�����")]
    float moveDistance;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�ǐՎ��s���̑J��")]
    public string failedTransition = "�ҋ@";
    [SerializeField, Header("��e���̑J��")]
    public string damagedTransition = "��e";
    [SerializeField, Header("�Փˎ��̑J��")]
    public string collisionTransition = "�U��";

    public override void Initialize()
    {
        patrol = enemy.GetComponent<NavMeshPatrol>();
    }

    public override void Enter()
    {
        base.Enter();
        patrol.SetAgentParam(moveSpeed, acceleration, angularSpeed);
        Vector3 Direction = Enemy.Target.transform.position - enemy.gameObject.transform.position;
        Direction.Normalize();
        Direction *= moveDistance;
        patrol.ExcuteCustom(enemy.gameObject.transform.position + Direction);
    }

    public override void MainFunc()
    {
        if (machine.Cnt >= trackingInterval)
        {
            machine.TransitionTo(failedTransition);
        }
        else if(enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedTransition);
        }
        Vector3 Direction = Enemy.Target.transform.position - enemy.gameObject.transform.position;
        Direction.Normalize();
        Direction *= moveDistance;
        patrol.ExcuteCustom(enemy.gameObject.transform.position + Direction);
        
    }

    public override void Exit()
    {
        patrol.Stop();
    }

    public override void CollisionEnterSelf(Collision collision)
    {
        if (collision.transform.name == "Player" && !enemy.IsDamaged)
        {
            Vector3 direction = collision.transform.position - transform.position;
            direction.Normalize();
            direction.y = 0.5f;
            PlayerController.instance.KnockBack(moveSpeed, direction);
            machine.TransitionTo(collisionTransition);
        }
    }
    public override void TriggerEnterSelf(Collider other)
    {
        if (other.name == "Player" && !enemy.IsDamaged)
        {
            Vector3 direction = other.transform.position - transform.position;
            direction.Normalize();
            direction.y = 0.5f;
            PlayerController.instance.KnockBack(moveSpeed, direction);
            machine.TransitionTo(collisionTransition);
        }
    }
}
