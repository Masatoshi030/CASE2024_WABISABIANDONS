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
    string elapsedTransition = "�ҋ@";
    [SerializeField, Header("���G�������̑J��")]
    string searchSuccessTransition = "�ǐ�";
    [SerializeField, Header("��e���̑J��")]
    string damagedTransition = "��e";
    [SerializeField, Header("�Փˎ��̑J��")]
    string collisionTransition = "�ǐ�";
    

    [SerializeField, Header("�p�g���[��"), ReadOnly]
    NavMeshPatrol patrol;
    [SerializeField, Header("�ړI�n�̐�"), ReadOnly]
    int targetNum;
    [SerializeField, Header("�ړI�n��Index"), ReadOnly]
    int targetIdx;

    public override void Initialize()
    {
        patrol = enemy.GetComponent<NavMeshPatrol>();
        targetNum = patrol.GetTargets().Length;
        targetIdx = 0;
    }

    public override void Enter()
    {
        base.Enter();
        // �ړ�����
        patrol.SetAgentParam(moveSpeed, acceleration, angularSpeed);
        patrol.ExcutePatrol(targetIdx);
        enemy.EnemyRigidbody.velocity = Vector3.zero;
    }

    public override void MainFunc()
    {
        if(enemy.IsFindPlayer)
        {
            Machine.TransitionTo(searchSuccessTransition);
            return;
        }
        else if(enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedTransition);
        }
        if (patrol.GetPatrolState() == NavMeshPatrol.PatrolState.Idle)
        {          
             Machine.TransitionTo(elapsedTransition);
        }
    }

    public override void Exit()
    {
        patrol.Stop();
        targetIdx++;
        if (targetIdx >= targetNum)
        {
            targetIdx = 0;
        }
    }

    public override void CollisionEnterSelf(GameObject other)
    {
        if (other.name == "Player")
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
