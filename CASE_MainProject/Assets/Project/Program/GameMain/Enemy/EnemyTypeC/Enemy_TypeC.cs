using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeC : Enemy_Mob
{
    enum MainState
    {[InspectorName("����")] Common, [InspectorName("�ŗL")] Unique };

    [Space(padA), Header("--���C���p�����[�^--")]
    [SerializeField, Header("�X�e�[�g"), Toolbar(typeof(MainState))]
    MainState subState = MainState.Common;
    [SerializeField, Header("�ėp�J�E���g"), ReadOnly]
    float cnt;
    [SerializeField, Header("��]���[�g"), Range(0.0f, 3.0f)]
    float rotationRate = 0.6f;
    [SerializeField, Header("�ړ����x"), Range(0.0f, 30.0f)]
    float moveSpeed = 5.0f;
    [SerializeField, Header("�ړ����̏����"), Range(0.0f, 20.0f)]
    float pressureForMove = 2.0f;
    [SerializeField, Header("�ǌ����̈ړ��{��"), Range(1.0f, 10.0f)]
    float trackingSpeedRate = 1.5f;
    [SerializeField, Header("1�b������̈��͉񕜗�")]
    float pressurePerHeal = 5.0f;
    [SerializeField, Header("���S��������"), Range(0.0f, 5.0f)]
    float timeToPrepareEscape = 1.0f;
    [SerializeField, Header("���S����]���[�g"), Range(0.0f, 2.0f)]
    float rotationRateForEscape = 0.9f;
    [SerializeField, Header("���S����"), Range(0.0f, 20.0f)]
    float timeToEscape = 2.0f;

    [Space(padA), Header("--�C���^�[�o��--")]
    [SerializeField, Header("�ҋ@�C���^�[�o��")]
    float waitInterval;
    [SerializeField, Header("�������U���C���^�[�o��")]
    float rangedInterval;

    [Space(padA), Header("--��ԑJ�ڃp�����[�^--")]
    [SerializeField, Header("���S�Ɉڍs���鈳�͗�")]
    float pressureForEscapeMode = 12.0f;
    [SerializeField, Header("�񕜂Ɉڍs���鈳�͗�")]
    float pressureForHealMode = 15.0f;
    [SerializeField, Header("�񕜂��畜�A���鈳�͗�")]
    float pressureForStoppedHeal = 50.0f;
    [SerializeField, Header("���S��U�����邩"), ReadOnly]
    bool bAttackAfterEscape = false;

    [Space(padA), Header("--�U���֘A--")]

    [SerializeField, Header("�J�ڋ���")]
    float distanceForAttack = 12.0f;
    [SerializeField, Header("���S��U���J�ڋ���")]
    float distanceForEscapeAfterAttack = 6.0f;
    [SerializeField, Header("�����")]
    float pressureForAttack = 5.0f;
    [SerializeField, Header("�Œ�K�v����")]
    float lineForAttack = 10.0f;
    [SerializeField, Header("�������U���̒e")]
    GameObject objectForAttack;
    [SerializeField, Header("�������U���̑��x")]
    float rangedAttackSpeed;
    [SerializeField, Header("�U���I�u�W�F�N�g�����ʒu")]
    Transform attackTransform;
    [SerializeField, Header("�U����������")]
    bool bPrepareAttack = false;

    [Space(padA), Header("--�A�j���[�V�����֘A--")]
    [SerializeField, Header("�A�j���[�^�[�R���|�[�l���g"), ReadOnly]
    Animator animator;

    [Space(padA), Header("--�p�g���[���֘A--")]

    [SerializeField, Header("�p�g���[���R���|�[�l���g"), ReadOnly]
    Patrol patrol;
    [SerializeField, Header("�ړI�n��"), ReadOnly]
    int targetNum = 0;
    [SerializeField, Header("�ړI�n�̃C���f�b�N�X"), ReadOnly]
    int targetIndex = 0;
    [SerializeField, Header("���̖ړI�n"), ReadOnly]
    Vector3 nextTargetPos;

    Rigidbody rb;

    void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        patrol = GetComponent<Patrol>();
        animator = GetComponent<Animator>();
        targetNum = patrol.GetTargets().Length;
        attackTransform = transform.Find("AttackTransform");
        nextTargetPos = patrol.GetTargets()[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (subState)
        {
            case MainState.Common: base.Update(); break;
            case MainState.Unique: PrepareEscapeFunc(); break;
        }
    }

    /*
    * <summary>
    * �ҋ@��Ԋ֐�
    * <param>
    * void
    * <return>
    * void
     */
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
        angle = Mathf.Lerp(0.0f, angle, cnt / waitInterval) * rotationRate;
        transform.Rotate(0.0f, angle, 0.0f);

        // �ړ����[�h�Ɉڍs
        if (cnt > waitInterval)
        {
            cnt = 0.0f;
            state = State.Move;
            patrol.SetInitPosition(transform.position, moveSpeed, targetIndex);
        }
    }

    /*
    * <summary>
    * �ړ���Ԋ֐�
    * <param>
    * void
    * <return>
    * void
    */
    protected override void MoveFunc()
    {
        cnt += Time.deltaTime;
        // �ړ��ň��͂��g��
        currentPressure -= pressureForMove * Time.deltaTime;
        // ���͉񕜃��[�h�ɑJ��
        if (currentPressure < pressureForHealMode)
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
            if (targetIndex >= targetNum)
            {
                targetIndex = 0;
            }
            nextTargetPos = patrol.GetTargets()[targetIndex].position;
            rb.velocity = Vector3.zero;
        }
    }

    /*
    * <summary>
    * �ǔ���Ԋ֐�
    * <param>
    * void
    * <return>
    * void
    */
    protected override void TrackingFunc()
    {
        // �ǔ��Ɉ��͂��g��
        currentPressure -= pressureForMove * trackingSpeedRate * Time.deltaTime;

        (bool isFind, float distance) = FindPlayerAtFOV();
        // �ҋ@���[�h�Ɉڍs
        if (!isFind)
        {
            state = State.Idle;
        }
        else
        {
            // ���������ȉ������݈��͂��������Œ჉�C���Ɠ��S�ɕK�v�Ȉ��͈ȏ゠��Ƃ�
            if (distance < distanceForEscapeAfterAttack * distanceForEscapeAfterAttack && currentPressure > lineForAttack + pressureForMove * trackingSpeedRate * timeToEscape)
            {
                cnt = 0.0f;
                state = State.Escape;
                bAttackAfterEscape = true;
                // �X�e�[�g���ŗL�X�e�[�g��
                subState = MainState.Unique;
                return;
            }
            else if (distance < distanceForAttack * distanceForAttack && currentPressure > lineForAttack)
            {
                cnt = 0.0f;
                currentPressure -= pressureForAttack;
                bPrepareAttack = true;
                animator.SetBool("bRanged", bPrepareAttack);
                state = State.AttackB;
                return;
            }
            else if (currentPressure < pressureForEscapeMode)
            {
                cnt = 0.0f;
                state = State.Escape;
                subState = MainState.Unique;
                return;
            }

            transform.LookAt(target.transform.position);
            Vector3 Euler = transform.localEulerAngles;
            Euler.x = 0.0f;
            Euler.z = 0.0f;
            transform.localEulerAngles = Euler;
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * trackingSpeedRate);
        }
    }

    /*
    * <summary>
    * ���S��Ԋ֐�
    * <param>
    * void
    * <return>
    * void
    */
    protected override void EscapeFunc()
    {
        cnt += Time.deltaTime;
        currentPressure -= pressureForMove * trackingSpeedRate * Time.deltaTime;

        transform.Translate(Vector3.forward * moveSpeed * trackingSpeedRate * Time.deltaTime);

        if (cnt >= timeToEscape)
        {
            cnt = 0.0f;
            if (bAttackAfterEscape)
            {
                cnt = 0.0f;
                currentPressure -= pressureForAttack;
                bPrepareAttack = true;
                animator.SetBool("bRanged", bPrepareAttack);
                state = State.AttackB;
                bAttackAfterEscape = false;
                return;
            }
            else
            {
                cnt = 0.0f;
                state = State.Idle;
            }
        }
    }

    /*
    * <summary>
    * �U����Ԋ֐�A
    * <param>
    * void
    * <return>
    * void
    */
    protected override void AttackFuncA()
    {
        // �ߋ����U������
    }

    /*
    * <summary>
    * �U����Ԋ֐�B
    * <param>
    * void
    * <return>
    * void
    */
    protected override void AttackFuncB()
    {
        cnt += Time.deltaTime;

        // �v���C���[�̕����֏�������]
        if (bPrepareAttack)
        {
            Vector3 targetVector = target.transform.position - transform.position;
            Vector3 axis = Vector3.Cross(transform.forward, targetVector);
            float angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0 ? -1.0f : 1.0f);
            if(Mathf.Abs(angle) > 10.0f)
            {
                angle = Mathf.Lerp(0.0f, angle, cnt / waitInterval) * rotationRate;
                transform.Rotate(0.0f, angle, 0.0f);
            }
            else
            {
                transform.LookAt(target.transform.position);
                Vector3 Euler = transform.localEulerAngles;
                Euler.x = 0.0f;
                Euler.z = 0.0f;
                transform.localEulerAngles = Euler;
            }
        }

        if (cnt > rangedInterval)
        {
            cnt = 0.0f;
            JudgeState();
        }
    }

    /*
    * <summary>
    * �����Ԋ֐�A
    * <param>
    * void
    * <return>
    * void
    */
    protected override void SpecialFuncA()
    {
    }

    /*
    * <summary>
    * �����Ԋ֐�B
    * <param>
    * void
    * <return>
    * void
    */
    protected override void SpecialFuncB()
    {
    }

    /*
    * <summary>
    * �񕜏�Ԋ֐�
    * <param>
    * void
    * <return>
    * void
    */
    protected override void HealFunc()
    {
        cnt += Time.deltaTime;
        currentPressure += pressurePerHeal * Time.deltaTime;
        // �ҋ@���[�h�Ɉڍs
        if (currentPressure > pressureForStoppedHeal)
        {
            cnt = 0.0f;
            state = State.Idle;
        }
    }

    /*
    * <summary>
    * ���S��Ԋ֐�
    * <param>
    * void
    * <return>
    * void
    */
    protected override void DeathFunc()
    {
        // ���̑O�Ɏ��S�G�t�F�N�g����ꂽ��
        Destroy(gameObject);
    }

    /*
    * <summary>
    * �j�󎞌Ăяo���֐�
    * <param>
    * void
    * <return>
    * void
    */
    protected override void DestroyFunc()
    {
        base.DestroyFunc();

        animator.SetBool("bDeath", true);
    }

    /*
    * <summary>
    * ���S�����֐�
    * ���S��Ԃɓ���O�̏����֐�
    * <param>
    * void
    * <return>
    * void
    */
    void PrepareEscapeFunc()
    {
        cnt += Time.deltaTime;
        // �v���C���[�Ƌt�����֏�������]
        Vector3 targetVector = transform.position - target.transform.position;
        Vector3 axis = Vector3.Cross(transform.forward, targetVector);
        float angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0 ? -1.0f : 1.0f);
        angle = Mathf.Lerp(0.0f, angle, rotationRateForEscape);
        transform.Rotate(0.0f, angle * Time.deltaTime, 0.0f);
        if (cnt > timeToPrepareEscape)
        {
            cnt = 0.0f;
            subState = MainState.Common;
        }
    }

    /*
    * <summary>
    * ��Ԕ��f�֐�
    * <param>
    * void
    * <return>
    * void
    */
    protected override void JudgeState()
    {
        state = State.Idle;
    }

    /*
    * <summary>
    * �������U�������֐�
    * <param>
    * void
    * <return>
    * void
    */
    protected void PlayRangedAttack()
    {
        Vector3 AttackVector = target.transform.position - attackTransform.position;
        AttackVector.Normalize();
        GameObject bullet = Instantiate(objectForAttack, attackTransform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(AttackVector * rangedAttackSpeed, ForceMode.Impulse);
        // �������U��bool��false��
        bPrepareAttack = false;
        animator.SetBool("bRanged", bPrepareAttack);
    }

    /*
     * <summary>
     * �_���[�W�֐�
     * <param>
     * float damageValue, Vector3 direction
     * <return>
     * void
     */
    public override void Damage(float val, Vector3 direction)
    {
        rb.AddForce(direction * val, ForceMode.Impulse);
        base.Damage(val, direction);
    }
}
