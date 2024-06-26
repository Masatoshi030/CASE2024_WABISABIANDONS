using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_A_Tracking : EnemyState
{
    [SerializeField, Header("�ړ����x")]
    float moveSpeed;
    [SerializeField, Header("�����x")]
    float accelerationSpeed;
    [SerializeField, Header("��]���x")]
    float angularSpeed;
    [SerializeField, Header("�ǐՎ���")]
    float trackingTime = 3.0f;
    [SerializeField, Header("�ǐՐ�������")]
    float trackSuccessDistance = 7.0f;

    NavMeshPatrol patrol;
    float subCnt = 0.0f;
    bool allowActive = false;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�������ȓ��̑J��")]
    StateKey successedKey;
    [SerializeField, Header("�o�ߎ��̑J��")]
    StateKey elapsedKey;
    [SerializeField, Header("�_���[�W����Key")]
    StateKey damagedKey;

    public override void Initialize()
    {
        base.Initialize();

        // �p�g���[���p�R���|�[�l���g���擾
        patrol = enemy.GetComponent<NavMeshPatrol>();
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
        patrol.ExcuteCustom(Enemy.Target.transform.position);
        enemy.IsVelocityZero = true;
        subCnt = 0.0f;
        allowActive = true;
        enemy.SetAllowActive(allowActive);
    }

    public override void MainFunc()
    {
        base.MainFunc();
        enemy.AllowObject.GetComponent<TargetAllow>().StartDesignation(enemy.EyeTransform.position, Enemy.Target.transform.position);
        subCnt += Time.deltaTime;
        if(subCnt >= 1.0f)
        {
            // 1�b���Ƀp�X�̐ݒ�
            patrol.ExcuteCustom(Enemy.Target.transform.position);
            subCnt = 0.0f;
            // ���\���̔��]
            allowActive = !allowActive;
            enemy.SetAllowActive(allowActive);
        }
        

        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedKey);
        }
        else if(machine.Cnt >= trackingTime)
        {
            machine.TransitionTo(elapsedKey);
        }
        else if(enemy.ToPlayerDistace <= (trackSuccessDistance * trackSuccessDistance))
        {
            machine.TransitionTo(successedKey);
        }
    }

    public override void Exit()
    {
        // ���\���̍폜
        enemy.AllowObject.GetComponent<TargetAllow>().EndDesignation();
        enemy.AllowObject.SetActive(false);
        patrol.Stop();
        patrol.Agent.enabled = false;
    }
}
