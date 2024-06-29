using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���m�p�I�u�W�F�N�g����̖��ߔ��s�҂����
public class State_A_WaitDetection : EnemyState
{
    public bool isDetection = false;
    bool isMove = false;

    [SerializeField, Header("���m���̈ړ�����")]
    Transform detectionTransform;

    [SerializeField, Header("���m���̈ړ����x")]
    float velocitySpeed;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("���m��̍ŏI�J��")]
    StateKey detectedKey;

    [SerializeField, Header("�_���[�W���̑J��")]
    StateKey damagedKey;

    public override void Enter()
    {
        base.Enter();
        enemy.IsVelocityZero = false;
    }

    public override void MainFunc()
    {
        base.MainFunc();
        
        // ���m�I�u�W�F�N�g����̒ʒm�Ȃ��Ɏ��g�Ńv���C���[�𔭌�
        if(enemy.IsFindPlayer && !isDetection)
        {
            isMove = true;
            enemy.EnemyRigidbody.velocity = detectionTransform.forward * velocitySpeed;
            isDetection = true;
        }
        // ���m�I�u�W�F�N�g����̒ʒm�Ŕ���
        else if(isDetection && !isMove)
        {
            isMove = true;
            enemy.EnemyRigidbody.velocity = detectionTransform.forward * velocitySpeed;
        }
        // ��e���̑J��
        else if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedKey);
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
                Enemy_Manager.instance.CreateSteamEffect(enemy.transform.position, Quaternion.identity);
                machine.TransitionTo(detectedKey);
            }
        }
    }
}
