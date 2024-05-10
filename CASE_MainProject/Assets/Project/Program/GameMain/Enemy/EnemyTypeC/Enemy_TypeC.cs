using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeC : Enemy_Mob
{
    [System.Serializable]
    public struct AttackParam
    {
        [SerializeField, Header("�J�ڋ���")]
        public float distance;
        [SerializeField, Header("�����")]
        public float consumePressure;
        [SerializeField, Header("�K�v�Œ���̈��͗�")]
        public float requiredPressure;
        [SerializeField, Header("�U���̒e")]
        public GameObject bullet;
        [SerializeField, Header("�e�̑��x")]
        public float bulletSpeed;
        [SerializeField, Header("�C���^�[�o��")]
        public float interval;
    }

    [Space(padA), Header("--���C���p�����[�^--")]
    [SerializeField, Header("�ėp�J�E���g"), ReadOnly]
    float cnt;
    [Space(padB), Header("-�ҋ@�p�����[�^-")]
    [SerializeField, Header("�ҋ@�C���^�[�o��")]
    float waitInterval;
    [Space(padB), SerializeField, Header("�ړ��p�����[�^")]
    EnemyAgentParam moveParam;
    [Space(padB), SerializeField, Header("�ǔ��p�����[�^")]
    EnemyAgentParam trackingParam;
    [SerializeField, Header("�߂����ē����鋗��")]
    float distanceTooClose = 6.0f;
    [Space(padB), SerializeField, Header("���S�p�����[�^")]
    EnemyAgentParam escapeParam;
    [Space(padB), SerializeField, Header("���S��������"), Range(0.0f, 5.0f)]
    float escapePrepareTime = 1.0f;
    [Space(padB), SerializeField, Header("���S����"), Range(0.0f, 20.0f)]
    float timeToEscape = 2.0f;
    float angle = 0.0f;
    [SerializeField, Header("���S�Ɉڍs���C��")]
    float pressureForEscapeMode = 12.0f;
    [SerializeField, Header("���S��U�����邩"), ReadOnly]
    bool bAttackAfterEscape = false;
    [Space(padB), SerializeField, Header("-�U���p�����[�^-")]
    AttackParam attackParam;
    
    
    [Space(padB), SerializeField, Header("-�񕜃p�����[�^-")]
    HealParam healParam;
    
    
    
    
    [SerializeField, Header("�v���C���[�̕��ɂǂꂾ��������")]
    float attackLookRate = 0.85f;
    [SerializeField, Header("��]���x")]
    float rotationSpeed = 5.0f;
    Transform attackTransform;
    bool bPrepareAttack = false;

    Animator animator;

    [Space(padA), Header("--�p�g���[���֘A--")]
    NavMeshPatrol patrol;
    [SerializeField, Header("�ړI�n��"), ReadOnly]
    int targetNum = 0;
    [SerializeField, Header("�ړI�n�̃C���f�b�N�X"), ReadOnly]
    int targetIndex = 0;

    Rigidbody rb;

    void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        patrol = GetComponent<NavMeshPatrol>();
        animator = GetComponent<Animator>();
        targetNum = patrol.GetTargets().Length;
        attackTransform = transform.Find("AttackTransform");
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
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
            patrol.SetAgentParam(trackingParam, true);
            state = State.Tracking;
        }
        // ���̖ړI�n�։�]
        //Vector3 targetVector = nextTargetPos - transform.position;
        //Vector3 axis = Vector3.Cross(transform.forward, targetVector);
        //float angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0 ? -1.0f : 1.0f);
        //angle = Mathf.Lerp(0.0f, angle, cnt / waitInterval) * rotationRate;
        //transform.Rotate(0.0f, angle, 0.0f);

        // �ړ����[�h�Ɉڍs
        if (cnt > waitInterval)
        {
            cnt = 0.0f;
            state = State.Move;
            patrol.SetAgentParam(moveParam, true);
            patrol.ExcutePatrol(targetIndex);
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
        currentPressure -= moveParam.consumePressure * Time.deltaTime;

        // ���͉񕜃��[�h�ɑJ��
        if (currentPressure < healParam.startHealLine)
        {
            state = State.Heal;
            return;
        }
        (bool isFind, float distance) = FindPlayerAtFOV();

        // �ǔ����[�h�Ɉڍs
        if (isFind)
        {
            cnt = 0.0f;
            patrol.SetAgentParam(trackingParam);
            state = State.Tracking;
            return;
        }

        // �ҋ@���[�h�Ɉڍs
        if (patrol.GetPatrolState() == NavMeshPatrol.PatrolState.Idle)
        {
            cnt = 0.0f;
            state = State.Idle;
            targetIndex++;
            if (targetIndex >= targetNum)
            {
                targetIndex = 0;
            }
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
        currentPressure -= trackingParam.consumePressure * Time.deltaTime;

        (bool isFind, float distance) = FindPlayerAtFOV();
        // �ҋ@���[�h�Ɉڍs
        if (!isFind)
        {
            state = State.Idle;
        }
        else
        {
            // ���S��U��
            if (distance < distanceTooClose * distanceTooClose && currentPressure > attackParam.requiredPressure + trackingParam.consumePressure * timeToEscape)
            {
                cnt = 0.0f;
                bAttackAfterEscape = true;
                // ��]�ʂ̐���
                Vector3 targetVector = transform.position - target.transform.position;
                Vector3 axis = Vector3.Cross(transform.forward, targetVector);
                angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0.0f ? -1.0f : 1.0f);    // ��]�����̌v�Z
                angle /= escapePrepareTime; // 1�b������̉�]�ʂɒ���
                state = State.UniqueA;
                return;
            }
            // �U��
            else if (distance < attackParam.distance * attackParam.distance && currentPressure > attackParam.requiredPressure)
            {
                cnt = 0.0f;
                currentPressure -= attackParam.consumePressure;
                bPrepareAttack = true;
                animator.SetBool("bAttack", bPrepareAttack);
                patrol.Stop();
                state = State.AttackA;
                return;
            }
            // ���S�����Ɉڍs
            else if (currentPressure < pressureForEscapeMode)
            {
                cnt = 0.0f;
                // ��]�ʂ̐���
                Vector3 targetVector = transform.position - target.transform.position;
                Vector3 axis = Vector3.Cross(transform.forward, targetVector);
                angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0.0f ? -1.0f : 1.0f);    // ��]�����̌v�Z
                angle /= escapePrepareTime; // 1�b������̉�]�ʂɒ���
                state = State.UniqueA;
                return;
            }

            transform.LookAt(target.transform.position);
            Vector3 Euler = transform.localEulerAngles;
            Euler.x = 0.0f;
            Euler.z = 0.0f;
            transform.localEulerAngles = Euler;
            patrol.ExcuteCustom(target.transform.position);
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
        currentPressure -= escapeParam.consumePressure * Time.deltaTime;

        // �ړ�
        Vector3 targetVector = transform.position - target.transform.position;
        targetVector.Normalize();
        patrol.ExcuteCustom(transform.position + targetVector * 5);

        if (cnt >= timeToEscape)
        {
            cnt = 0.0f;
            if (bAttackAfterEscape)
            {
                cnt = 0.0f;
                currentPressure -= attackParam.consumePressure;
                bPrepareAttack = true;
                animator.SetBool("bAttack", bPrepareAttack);
                state = State.AttackA;
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
    * �U����Ԋ֐�B
    * <param>
    * void
    * <return>
    * void
    */
    protected override void AttackFuncA()
    {
        cnt += Time.deltaTime;

        // �v���C���[�̕����֏�������]
        if (bPrepareAttack)
        {
            Vector3 targetVector = target.transform.position - transform.position;
            Vector3 axis = Vector3.Cross(transform.forward, targetVector);
            float angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0.0f ? -1.0f : 1.0f);
            angle = Mathf.Lerp(0.0f, angle, attackLookRate);
            transform.Rotate(0.0f, angle * Time.deltaTime * rotationSpeed, 0.0f);
        }

        if (cnt > attackParam.interval)
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
    protected override void UniqueFuncA()
    {
        cnt += Time.deltaTime;
        // �v���C���[�Ƌt�����֏�������]
        transform.Rotate(0.0f, angle * Time.deltaTime, 0.0f);
        if (cnt > escapePrepareTime)
        {
            state = State.Escape;
            cnt = 0.0f;
        }
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
        currentPressure += healPressureVal * Time.deltaTime;
        // �ҋ@���[�h�Ɉڍs
        if (currentPressure > healParam.endHealLine)
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
        //animator.SetBool("bDeath", true);
        //state = State.DeathWait;
        state = State.Death;

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
        GameObject bullet = Instantiate(attackParam.bullet, attackTransform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(AttackVector * attackParam.bulletSpeed, ForceMode.Impulse);
        // �������U��bool��false��
        bPrepareAttack = false;
        animator.SetBool("bAttack", bPrepareAttack);
    }

    /*
     * <summary>
     * �_���[�W�֐�
     * <param>
     * float damageValue, Vector3 direction
     * <return>
     * void
     */
    public override bool Damage(float val, Vector3 direction)
    {
        rb.AddForce(direction * val, ForceMode.Impulse);
        return base.Damage(val, direction);
    }
}
