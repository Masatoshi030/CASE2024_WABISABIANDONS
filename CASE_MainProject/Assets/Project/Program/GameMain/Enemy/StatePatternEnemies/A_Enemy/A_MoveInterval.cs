using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_MoveInterval : EnemyState
{
    [SerializeField, Header("�ړ����x")]
    float moveSpeed;
    [SerializeField, Header("�����x")]
    float acceleration;
    [SerializeField, Header("��]���x")]
    float angularSpeed;
    [SerializeField, Header("�ړ�����")]
    float moveInterval;
    [SerializeField, Header("�o�ߌ�̑J�ڐ�")]
    string elapsedTransition = "�ҋ@";
    [SerializeField, Header("���G�������̑J�ڐ�")]
    string serchSuccessTransition = "�ǐ�";

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
        // �ړ�����
        patrol.SetAgentParam(moveSpeed, acceleration, angularSpeed);
        patrol.ExcutePatrol(targetIdx);
    }

    public override void MainFunc()
    {
        if(enemy.IsFindPlayer)
        {
            patrol.Stop();
            Machine.TransitionTo(serchSuccessTransition);
        }
        if(machine.Cnt >= moveInterval)
        {
            patrol.Stop();
            Machine.TransitionTo(elapsedTransition);
        }
    }

    public override void Exit()
    {
        targetIdx++;
        if (targetIdx >= targetNum)
        {
            targetIdx = 0;
        }
    }
}
