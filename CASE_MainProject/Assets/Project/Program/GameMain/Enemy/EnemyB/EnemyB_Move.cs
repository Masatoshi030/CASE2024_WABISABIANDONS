using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �㉺�Ɉړ�����
public class EnemyB_Move : EnemyState
{
    [SerializeField, Header("�����ʒu"), ReadOnly]
    Vector3 initPos;
    [SerializeField, Header("�ړ�����")]
    float moveTime;
    float moveAmount;
    [SerializeField, Header("�ړ����x")]
    float moveSpeed;
    [SerializeField, Header("�����ړ���������")]
    bool initIsUp = true;
    bool nowIsUp;

    [Space(pad), Header("�J�ڐ惊�X�g")]
    [SerializeField, Header("���G�������̑J��")]
    string searchSuccessTransiton = "�U��";
    [SerializeField, Header("���Ԍo�ߌ�̑J��")]
    string elapsedTransition = "�ҋ@";
    [SerializeField, Header("��e���̑J��")]
    string damagedTransition = "��e";

    public override void Initialize()
    {
        initPos = enemy.transform.position;
        nowIsUp = initIsUp;
    }

    public override void Enter()
    {

    }

    public override void MainFunc()
    {
        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedTransition);
            return;
        }

        // 1�t���[��������̈ړ���
        float moveValue = moveSpeed * Time.deltaTime;
        if(machine.Cnt >= moveTime)
        {
            // ���ߗʂ����ǂ�
            float gap = machine.Cnt - moveTime;
            moveValue -= gap * moveSpeed;
        }


        if (nowIsUp)
        {
            enemy.transform.Translate(enemy.transform.up * moveValue);
        }
        else
        {
            enemy.transform.Translate(-enemy.transform.up * moveValue);
        }

        if(machine.Cnt >= moveTime)
        {
            nowIsUp = !nowIsUp;
            machine.TransitionTo(elapsedTransition);
        }

        if(enemy.IsFindPlayer)
        {
            machine.TransitionTo(searchSuccessTransiton);
        }
    }
}
