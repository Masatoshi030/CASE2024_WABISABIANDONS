using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_MoveTarget : EnemyState
{
    [SerializeField, Header("�ړ����x")]
    float moveSpeed;
    [SerializeField, Header("�����x")]
    float acceleration;
    [SerializeField, Header("��]���x")]
    float angularSpeed;
    

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�o�ߌ�̑J��")]
    public int elapsedID;
    [SerializeField, Header("���G�������̑J��")]
    public int searchSuccessID;
    [SerializeField, Header("��e���̑J��")]
    public int damagedID;
    [SerializeField, Header("�Փˎ��̑J��")]
    public int collisionID;
    

    [SerializeField, Header("�p�g���[��"), ReadOnly]
    NavMeshPatrol patrol;
    [SerializeField, Header("�ړI�n�̐�"), ReadOnly]
    int targetNum;
    [SerializeField, Header("�ړI�n��Index"), ReadOnly]
    int targetIdx;

    public override void Initialize()
    {
        base.Initialize();

        patrol = enemy.GetComponent<NavMeshPatrol>();
        targetNum = patrol.GetTargets().Length;
        targetIdx = 0;
    }

    public override void Enter()
    {
        base.Enter();

        // �ړ�����
        patrol.Agent.enabled = true;
        patrol.enabled = true;
        patrol.Agent.velocity = Vector3.zero;
        patrol.SetAgentParam(moveSpeed, acceleration, angularSpeed);
        patrol.ExcutePatrol(targetIdx);
        enemy.IsVelocityZero = true;
    }

    public override void MainFunc()
    {
        base.MainFunc();
        if (!continueProcessing) return;

        if (enemy.IsFindPlayer)
        {
            Machine.TransitionTo(searchSuccessID);
            return;
        }
        else if(enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedID);
        }
        if (patrol.GetPatrolState() == NavMeshPatrol.PatrolState.Idle)
        {          
             Machine.TransitionTo(elapsedID);
        }
    }

    public override void Exit()
    {
        base.Exit();
        patrol.Stop();
        patrol.Agent.enabled = false;
        targetIdx++;
        if (targetIdx >= targetNum)
        {
            targetIdx = 0;
        }
    }

    public override void CollisionEnterSelf(Collision collision)
    {
        if (collision.transform.name == "Player")
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
