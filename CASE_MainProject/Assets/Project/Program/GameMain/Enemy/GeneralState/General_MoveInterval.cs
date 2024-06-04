using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_MoveInterval : EnemyState
{
    [SerializeField, Header("�ړ����x")]
    float moveSpeed;
    [SerializeField, Header("�����x")]
    float acceleration;
    [SerializeField, Header("��]���x")]
    float angularSpeed;
    [SerializeField, Header("�ړ�����")]
    float moveInterval;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�o�ߌ�̑J�ڐ�")]
    public int elapsedID;
    [SerializeField, Header("���G�������̑J�ڐ�")]
    public int serchSuccessID;
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
            machine.TransitionTo(serchSuccessID);
            return;
        }
        else if(enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedID);
            return;
        }
        else if(machine.Cnt >= moveInterval)
        {
            patrol.Stop();
            machine.TransitionTo(elapsedID);
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
}
