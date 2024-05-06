using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeC : Enemy_Mob
{
    const float pad = 20.0f;
    enum SubState
    {[InspectorName("����")] Common, [InspectorName("�ŗL")] Unique };

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
    [SerializeField, Header("�X�e�[�g"), Toolbar(typeof(SubState))]
    SubState subState = SubState.Common;
    [SerializeField, Header("�ėp�J�E���g"), ReadOnly]
    float cnt;
    [SerializeField, Header("��]���[�g"), Range(0.0f, 1.0f)]
    float rotationRate = 0.8f;
    [SerializeField, Header("�ړ����x"), Range(0.0f, 30.0f)]
    float moveSpeed = 5.0f;
    [SerializeField, Header("�ړ����̏����"), Range(0.0f, 20.0f)]
    float pressureForMove = 2.0f;
    [SerializeField, Header("�ǌ����̈ړ��{��"), Range(1.0f, 10.0f)]
    float trackingSpeedRate = 1.5f;
    [SerializeField, Header("1�b������̈��͉񕜗�")]
    float pressurePerHeal = 5.0f;
    [SerializeField, Header("���S�x�N�g��")]
    Vector3 escapeVector;
    [SerializeField, Header("���S��������")]
    float timeToPrepareEscape = 1.0f;
    [SerializeField, Header("���S����"), Range(0.0f, 20.0f)]
    float timeToEscape = 2.0f;

    [Space(pad), Header("--��ԑJ�ڃp�����[�^--")]
    [SerializeField, Header("���S�Ɉڍs���鈳�͗�")]
    float pressureForEscapeMode = 12.0f;
    [SerializeField, Header("�񕜂Ɉڍs���鈳�͗�")]
    float pressureForHealMode = 15.0f;
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
    [SerializeField, Header("�U���I�u�W�F�N�g�����ʒu")]
    Transform rangedAttackTransform;

    [Space(pad), Header("--�A�j���[�V�����֘A--")]
    [SerializeField, Header("�A�j���[�^�[�R���|�[�l���g"), ReadOnly]
    Animator animator;

    [Space(pad), Header("--�p�g���[���֘A--")]

    [SerializeField, Header("�p�g���[���R���|�[�l���g"), ReadOnly]
    Patrol patrol;
    [SerializeField, Header("�ړI�n��"),ReadOnly]
    int targetNum = 0;
    [SerializeField, Header("�ړI�n�̃C���f�b�N�X"), ReadOnly]
    int targetIndex = 0;
    [SerializeField, Header("���̖ړI�n"), ReadOnly]
    Vector3 nextTargetPos;

    bool bInitialize = false;

    void Start()
    {
        base.Start();
        patrol = GetComponent<Patrol>();
        animator = GetComponent<Animator>();
        targetNum = patrol.GetTargets().Length;
        rangedAttackTransform = transform.Find("AttackTransform");
    }

    // Update is called once per frame
    void Update()
    {
        if(!bInitialize)
        {
            nextTargetPos = patrol.GetTargets()[0].position;
            bInitialize = true;
        }

        switch(subState)
        {
            case SubState.Common: base.Update();break;
            case SubState.Unique: PrepareEscape(); break;
        }
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
        // �ړ��ň��͂��g��
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
        // �ǔ��Ɉ��͂��g��
        currentPressure -= pressureForMove * trackingSpeedRate * Time.deltaTime;
        // �ҋ@���[�h�Ɉڍs
        if(!FindPlayerAtFOV().isFind)
        {
            state = State.Idle;
        }
        else
        {
            transform.LookAt(target.transform.position);
            Vector3 Euler = transform.localEulerAngles;
            Euler.x = 0.0f;
            Euler.z = 0.0f;
            transform.localEulerAngles = Euler;
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * trackingSpeedRate);

            // �U�����[�h�ւ̑J��
            Vector3 Diff = target.transform.position - transform.position;
            float dis2 = Diff.x * Diff.x + Diff.y * Diff.y + Diff.z * Diff.z;
            float range = distanceForRangedAttack * distanceForRangedAttack;

            // �������U���ɏd����u��
            if (dis2 < range && currentPressure > lineForRagnedAttack)
            {
                Debug.Log("�������U�����[�h�Ɉڍs");
                cnt = 0.0f;
                currentPressure -= pressureForRangedAttack;
                animator.SetBool("bRanged", true);
                state = State.AttackB;
                return;
            }
            if (currentPressure < pressureForEscapeMode)
            {
                cnt = 0.0f;
                transform.LookAt(transform.position - transform.forward);
                state = State.Escape;
                subState = SubState.Unique;
            }
        }
    }

    protected override void EscapeFunc()
    {
        cnt += Time.deltaTime;

        transform.Translate(Vector3.forward * moveSpeed * trackingSpeedRate * Time.deltaTime);

        if (cnt >= timeToEscape)
        {
            cnt = 0.0f;
            state = State.Idle;
        }
    }

    protected override void AttackFunc1()
    {
        // �ߋ����U������
    }

    protected override void AttackFunc2()
    {
        cnt += Time.deltaTime;
        // �A�j���[�V�����I���҂�

        // �v���C���[�̕����֏�������]
        Vector3 targetVector = target.transform.position - transform.position;
        Vector3 axis = Vector3.Cross(transform.forward, targetVector);
        float angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0 ? -1.0f : 1.0f);
        angle = Mathf.Lerp(0.0f, angle, rotationRate);
        transform.Rotate(0.0f, angle * Time.deltaTime, 0.0f);

        if(cnt > rangedInterval)
        {
            cnt = 0.0f;
            JudgeState();
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

    void PrepareEscape()
    {
        cnt += Time.deltaTime;
        // �v���C���[�Ƌt�����֏�������]
        Vector3 targetVector = target.transform.position - transform.position;
        Vector3 axis = Vector3.Cross(transform.forward, targetVector);
        float angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0 ? -1.0f : 1.0f);
        angle = Mathf.Lerp(0.0f, angle, rotationRate) * -1;
        transform.Rotate(0.0f, angle * Time.deltaTime, 0.0f);
        if(cnt > timeToPrepareEscape)
        {
            cnt = 0.0f;
            subState = SubState.Common;
        }
    }

    // �U����̃W���b�W
    protected override void JudgeState()
    {
        state = State.Idle;
    }

    // �������U���֐�
    protected void PlayRangedAttack()
    {
        Vector3 AttackVector = target.transform.position - transform.position;
        AttackVector.Normalize();
        GameObject bullet = Instantiate(objectForRangedAttack, rangedAttackTransform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(AttackVector * rangedAttackSpeed, ForceMode.Impulse);
        Debug.Log("�������U��!!");
    }

    // �������U����~�֐�
    protected void StopRangedAttack()
    {
        // �������U��bool��false��
        animator.SetBool("bRanged", false);
    }
}
