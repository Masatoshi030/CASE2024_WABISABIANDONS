using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_A_Move : EnemyState
{
    [SerializeField, Header("�ړ����x")]
    float moveSpeed;
    [SerializeField, Header("�����x")]
    float accelerationSpeed;
    [SerializeField, Header("��]���x")]
    float angularSpeed;

    NavMeshPatrol patrol;
    int targetNum;
    int targetIdx;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�������̑J��")]
    StateKey reachedKey;
    [SerializeField, Header("�������̑J��")]
    StateKey findingKey;
    [SerializeField, Header("�_���[�W���̑J��")]
    StateKey damagedKey;

    public override void Initialize()
    {
        base.Initialize();

        // �p�g���[���p�R���|�[�l���g���擾
        patrol = enemy.GetComponent<NavMeshPatrol>();
        targetNum = patrol.GetTargets().Length;
        targetIdx = 0;
    }

    public override void Enter()
    {
        base.Enter();

        // �G�[�W�F���g���̂�enabled
        patrol.Agent.enabled = true;
        patrol.enabled = true;
        patrol.Agent.velocity = Vector3.zero;
        // �p�����[�^�̐ݒ�
        patrol.SetAgentParam(moveSpeed, accelerationSpeed, angularSpeed);
        // �p�X�̐ݒ�
        patrol.ExcutePatrol(targetIdx);
        enemy.IsVelocityZero = true;
    }

    public override void MainFunc()
    {
        base.MainFunc();

        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedKey);
        }
        else if(enemy.IsFindPlayer)
        {
            machine.TransitionTo(findingKey);
        }
        else if(patrol.GetPatrolState() == NavMeshPatrol.PatrolState.Idle)
        {
            machine.TransitionTo(reachedKey);
        }
    }

    public override void Exit()
    {
        base.Exit();
        // �p�g���[���̏I��
        patrol.Stop();
        patrol.Agent.enabled = false;
        // �C���f�b�N�X�����i�߂�
        targetIdx++;
        if (targetIdx >= targetNum)
        {
            targetIdx = 0;
        }
    }
}
