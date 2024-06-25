using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyA_Tracking : EnemyState
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
    [SerializeField, Header("�U����")]
    float attackPower = 5.0f;


    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�ǐՎ��s���̑J��")]
    public int failedID;
    [SerializeField, Header("��e���̑J��")]
    public int damagedID;
    [SerializeField, Header("�Փˎ��̑J��")]
    public int collisionID;

    public override void Initialize()
    {
        base.Initialize();

        patrol = enemy.GetComponent<NavMeshPatrol>();
    }

    public override void Enter()
    {
        base.Enter();
        enemy.IsVelocityZero = true;
        patrol.enabled = true;
        patrol.Agent.enabled = true;
        patrol.Agent.velocity = Vector3.zero;
        patrol.SetAgentParam(moveSpeed, acceleration, angularSpeed);
        Vector3 Direction = Enemy.Target.transform.position - enemy.gameObject.transform.position;
        Direction.Normalize();
        Direction *= moveDistance;
        patrol.ExcuteCustom(enemy.gameObject.transform.position + Direction);
    }

    public override void MainFunc()
    {
        base.MainFunc();
        if (!continueProcessing) return;

        if (machine.Cnt >= trackingInterval)
        {
            machine.TransitionTo(failedID);
            return;
        }
        else if (enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedID);
            return;
        }
        Vector3 Direction = Enemy.Target.transform.position - enemy.gameObject.transform.position;
        Direction.Normalize();
        Direction *= moveDistance;
        patrol.ExcuteCustom(enemy.gameObject.transform.position + Direction);
    }

    public override void Exit()
    {
        base.Exit();

        patrol.Stop();
        patrol.Agent.enabled = false;
    }

    public override void CollisionEnterSelf(Collision collision)
    {
        if (collision.transform.name == "Player" && !enemy.IsDamaged)
        {
            PlayerController.instance.Damage(attackPower);
            machine.TransitionTo(collisionID);
        }
    }

    public override void TriggerEnterSelf(Collider other)
    {
        if (other.transform.name == "Player" && !enemy.IsDamaged)
        {
            PlayerController.instance.Damage(attackPower);
            machine.TransitionTo(collisionID);
        }
    }
}
