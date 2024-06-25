using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA_WaitPlayer : EnemyState
{
    [SerializeField, Header("�v���C���[�̒ʉߌ��m����������")]
    public bool isDetection = false;

    [SerializeField, Header("�ړ���")]
    bool isMove = false;

    [SerializeField, Header("���m���̈ړ�����")]
    Transform detectionTransform;

    [SerializeField, Header("���m���̈ړ����x")]
    float velocitySpeed;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("���m��̍ŏI�J�ڐ�")]
    int finalTransitionID;

    [SerializeField, Header("��e���̑J�ڐ�")]
    int damagedID;

    public override void Enter()
    {
        base.Enter();
        enemy.IsVelocityZero = false;
    }

    public override void MainFunc()
    {
        base.MainFunc();
        if(enemy.IsFindPlayer && !isDetection)
        {
            isMove = true;
            enemy.EnemyRigidbody.velocity = detectionTransform.forward * velocitySpeed;
            isDetection = true;
        }
        else if(isDetection && !isMove)
        {
            isMove = true;
            enemy.EnemyRigidbody.velocity = detectionTransform.forward * velocitySpeed;
        }
        else if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedID);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void CollisionEnterSelf(Collision collision)
    {
        base.CollisionEnterSelf(collision);
        if (isDetection)
        {
            if (collision.transform.tag == "Ground")
            {
                machine.TransitionTo(finalTransitionID);
            }
        }
    }
}
