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
    string elapsedTransition = "�ҋ@";
    [SerializeField, Header("���G�������̑J�ڐ�")]
    string serchSuccessTransition = "�ǐ�";
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

    public override void CollisionEnter(Collision collision)
    {
        if (collision.transform.root.name == "Player")
        {
            patrol.Stop();
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack) machine.TransitionTo(damagedTransition);
            else machine.TransitionTo(collisionTransition);
        }
    }

    public override void TriggerEnter(Collider collider)
    {
        if (collider.transform.root.name == "Player")
        {
            patrol.Stop();
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack) machine.TransitionTo(damagedTransition);
            else machine.TransitionTo(collisionTransition);
        }
    }
}
