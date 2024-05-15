using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Tracking : EnemyState
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
    [SerializeField, Header("�ǐՎ��s���̑J�ڐ�")]
    public string failedTransition = "�ҋ@";
    [SerializeField, Header("�ǐՐ������̑J�ڐ�")]
    public string successfulTransition = "�U��";


    public override void Initialize()
    {
        patrol = enemy.GetComponent<NavMeshPatrol>();
    }

    public override void Enter()
    {
        patrol.SetAgentParam(moveSpeed, acceleration, angularSpeed);
        patrol.ExcuteCustom(Enemy.Target.transform.position);
    }

    public override void MainFunc()
    {
        patrol.ExcuteCustom(Enemy.Target.transform.position);
        if (machine.Cnt >= trackingInterval)
        {
            Machine.TransitionTo(failedTransition);
        }
    }

    public override void Exit()
    {
        
    }
}
