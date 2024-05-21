using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Tracking : EnemyState
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
    [SerializeField, Header("�U���J�ڋ���")]
    float attackDistance;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�ǐՎ��s���̑J��")]
    public string failedTransition = "�ҋ@";
    [SerializeField, Header("�������ȓ��̑J��")]
    public string successfulTransition = "�U��";
    [SerializeField, Header("��e���̑J��")]
    public string damagedTransition = "��e";
    [SerializeField, Header("�Փˎ��̑J��")]
    public string collisionTransition = "�ҋ@";

    public override void Initialize()
    {
        patrol = enemy.GetComponent<NavMeshPatrol>();
    }

    public override void Enter()
    {
        base.Enter();
        patrol.ExcuteCustom(Enemy.Target.transform.position);
        enemy.EnemyRigidbody.velocity = Vector3.zero;
    }

    public override void MainFunc()
    {
        if(machine.Cnt>= trackingInterval)
        {
            machine.TransitionTo(failedTransition);
            return;
        }
        if(enemy.IsFindPlayer)
        {
            patrol.ExcuteCustom(Enemy.Target.transform.position);
            if(enemy.ToPlayerDistace <= attackDistance * attackDistance)
            {
                machine.TransitionTo(successfulTransition);
            }
        }
        else
        {
            // ���s�Ƃ݂Ȃ�
            machine.TransitionTo(failedTransition);
        }
    }

    public override void Exit()
    {
        patrol.Stop();
    }

    public override void CollisionEnterOpponent(GameObject other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedTransition);
            else
                machine.TransitionTo(collisionTransition);
        }
    }

    public override void TriggerEnterOpponent(GameObject other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedTransition);
            else
                machine.TransitionTo(collisionTransition);
        }
    }
}
