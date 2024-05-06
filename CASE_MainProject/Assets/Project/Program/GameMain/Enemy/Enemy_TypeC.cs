using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeC : Enemy_Mob
{
    const float pad = 20.0f;

    [Space(pad), Header("--�C���^�[�o��--")]
    [SerializeField, Header("�ҋ@�C���^�[�o��")]
    float waitInterval;
    [SerializeField, Header("�ړ��C���^�[�o��")]
    float moveInterval;
    [SerializeField, Header("�ߋ����U���C���^�[�o��")]
    float closeInterval;
    [SerializeField, Header("�������U���C���^�[�o��")]
    float rangedInterval;

    [Space(pad), Header("--���C���p�����[�^--")]

    [SerializeField, Header("�ėp�J�E���g"), ReadOnly]
    float cnt;
    [SerializeField, Header("��]���x"), Range(0.0f, 1.0f)]
    float rotationRate = 0.3f;
    [SerializeField, Header("�ړ����x"), Range(0.0f, 30.0f)]
    float moveSpeed = 5.0f;
    [SerializeField, Header("�ړ����̏����"), Range(0.0f, 20.0f)]
    float pressureForMove = 2.0f;
    [SerializeField, Header("�ǌ����̈ړ��{��"), Range(1.0f, 10.0f)]
    float trackingSpeedRate = 1.5f;
    [SerializeField, Header("�񕜂Ɉڍs���鈳�͗�")]
    float pressureForHealMode = 15.0f;
    [SerializeField, Header("1�b������̈��͉񕜗�")]
    float pressurePerHeal = 5.0f;
    [SerializeField, Header("�񕜂��畜�A���鈳�͗�")]
    float pressureForStoppedHeal = 50.0f;

    [Space(pad), Header("--�������U��--")]

    [SerializeField, Header("�J�ڋ���")]
    float distanceForRangedAttack = 12.0f;
    [SerializeField, Header("�����")]
    float pressureForRangedAttack = 5.0f;
    [SerializeField, Header("�Œ�K�v����")]
    float lineForRagnedAttack = 10.0f;
    [SerializeField, Header("�������U���̒e")]
    GameObject objectForRangedAttack;
    [SerializeField, Header("�������U���̑��x")]
    float rangedAttackSpeed;

    [Space(pad), Header("--�ߋ����U��--")]

    [SerializeField, Header("�J�ڋ���")]
    float distanceForCloseAttack = 5.0f;
    [SerializeField, Header("�����")]
    float pressureForCloseAttack = 2.0f;
    [SerializeField, Header("�Œ�K�v����")]
    float lineForCloseAttack = 7.0f;

    [Space(pad), Header("--�p�g���[���֘A--")]

    [SerializeField, Header("�p�g���[���R���|�[�l���g"), ReadOnly]
    Patrol patrol;
    [SerializeField, Header("�ړI�n��"),ReadOnly]
    int targetNum = 0;
    [SerializeField, Header("�ړI�n�̃C���f�b�N�X"), ReadOnly]
    int targetIndex = 0;
    [SerializeField, Header("���̖ړI�n"), ReadOnly]
    Vector3 nextTargetPos;

    void Start()
    {
        base.Start();
        patrol = GetComponent<Patrol>();
        targetNum = patrol.GetTargets().Length;
        nextTargetPos = patrol.GetTargets()[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void IdleFunc()
    {
        cnt += Time.deltaTime;
        (bool isFind, float distance) = FindPlayerAtFOV();

        // �ǔ����[�h�Ɉڍs
        if (isFind)
        {
            cnt = 0.0f;
            state = State.Tracking;
        }

        // �v���C���[�̕����֏�������]
        //Vector3 targetVector = target.transform.position - transform.position;
        //Vector3 axis = Vector3.Cross(transform.forward, targetVector);
        //float angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0 ? -1.0f : 1.0f);
        //angle = Mathf.Lerp(0.0f, angle, rotationRate);
        //transform.Rotate(0.0f, angle * Time.deltaTime, 0.0f);


        // ���̖ړI�n�։�]
        Vector3 targetVector = nextTargetPos - transform.position;
        Vector3 axis = Vector3.Cross(transform.forward, targetVector);
        float angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0 ? -1.0f : 1.0f);
        angle = Mathf.Lerp(0.0f, angle, cnt / waitInterval) * 0.1f;
        transform.Rotate(0.0f, angle, 0.0f);

        // �ړ����[�h�Ɉڍs
        if (cnt > waitInterval)
        {
            cnt = 0.0f;
            state = State.Move;
            patrol.SetInitPosition(transform.position, moveSpeed, targetIndex);
        }
    }

    protected override void MoveFunc()
    {
        cnt += Time.deltaTime;
        currentPressure -= pressureForMove * Time.deltaTime;
        // ���͉񕜃��[�h�ɑJ��
        if(currentPressure < pressureForHealMode)
        {
            state = State.Heal;
            return;
        }
        (bool isFind, float distance) = FindPlayerAtFOV();

        // �ǔ����[�h�Ɉڍs
        if (isFind)
        {
            cnt = 0.0f;
            state = State.Tracking;
            return;
        }
        bool b = patrol.ExcutePatrol(targetIndex, moveSpeed);

        // �ҋ@���[�h�Ɉڍs
        if (b)
        {
            cnt = 0.0f;
            state = State.Idle;
            targetIndex++;
            if(targetIndex >= targetNum)
            {
                targetIndex = 0;
            }
            nextTargetPos = patrol.GetTargets()[targetIndex].position;
        }
    }

    protected override void TrackingFunc()
    {
        // �ҋ@���[�h�Ɉڍs
        if(!FindPlayerAtFOV().isFind)
        {
            state = State.Idle;
        }
        else
        {
            transform.LookAt(target.transform.position);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * trackingSpeedRate);

            // �U�����[�h�ւ̑J��
            Vector3 Diff = target.transform.position - transform.position;
            float dis2 = Diff.x * Diff.x + Diff.y * Diff.y + Diff.z * Diff.z;
            float range1 = distanceForCloseAttack * distanceForCloseAttack;
            float range2 = distanceForRangedAttack * distanceForRangedAttack;

            // �������U���ɏd����u��
            if (dis2 < range2 && currentPressure > lineForRagnedAttack)
            {
                Debug.Log("�������U�����[�h�Ɉڍs");
                currentPressure -= pressureForRangedAttack;
                // �������U�����s���x�N�g��
                Diff.Normalize();
                GameObject bullet = Instantiate(objectForRangedAttack, transform.position + transform.forward * 1.5f, Quaternion.identity);
                bullet.GetComponent<Rigidbody>().AddForce(Diff * rangedAttackSpeed, ForceMode.Impulse);

                state = State.AttackB;
                return;
            }
            else if (dis2 < range1 && currentPressure > lineForCloseAttack)
            {
                Debug.Log("�ߋ����U�����[�h�Ɉڍs");
                currentPressure -= pressureForCloseAttack;
                state = State.AttackA;
                return;
            }
        }
    }

    protected override void EscapeFunc()
    {
        throw new System.NotImplementedException();
    }

    protected override void AttackFunc1()
    {
        Debug.Log("�ߋ����U��");
    }

    protected override void AttackFunc2()
    {
        cnt += Time.deltaTime;
        // �A�j���[�V�����I���҂�

        if(cnt >= rangedInterval)
        {
            // �ҋ@���s����
            state = State.Idle;
            cnt = 0.0f;
            // �ēx�������U����

            // �ߋ����U����

            // �񕜂�
        }
    }

    protected override void SpecialFuncA()
    {
        throw new System.NotImplementedException();
    }

    protected override void SpecialFuncB()
    {
        throw new System.NotImplementedException();
    }

    protected override void HealFunc()
    {
        cnt += Time.deltaTime;
        currentPressure += pressurePerHeal * Time.deltaTime;
        // �ҋ@���[�h�Ɉڍs
        if(currentPressure > pressureForStoppedHeal)
        {
            cnt = 0.0f;
            state = State.Idle;
        }
    }

    protected override void DeathFunc()
    {
        throw new System.NotImplementedException();
    }

    protected override void DestroyFunc()
    {
        base.DestroyFunc();
    }
}
