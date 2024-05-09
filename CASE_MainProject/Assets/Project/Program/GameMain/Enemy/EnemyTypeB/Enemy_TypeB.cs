using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeB : Enemy_Mob
{
    [Space(padA), Header("--���C���p�����[�^--")]
    [SerializeField, Header("�ėp�J�E���g"), ReadOnly]
    float cnt;
    [SerializeField, Header("�ړ����x")]
    float moveSpeed = 3.0f;
    [SerializeField, Header("�ǌ��ˎ��̃_���[�W")]
    float clashDamege = 1.0f;
    

    [Space(padB), Header("--�U���֘A--")]
    [SerializeField, Header("�U����������")]
    float attackPrepareTime = 1.2f;
    [SerializeField, Header("�U���������̌�����Ԃ̊���"), Range(0.0f, 3.0f)]
    float attackPrepareRotationRate = 1.5f;
    [SerializeField, Header("�ːi�����x")]
    float tackleSpeed = 8.0f;
    [SerializeField, Header("�ːi�U����")]
    float tacklePower = 3.0f;
    [SerializeField, Header("�U���p������")]
    float attackContinueTime = 2.0f;
    [SerializeField, Header("�ǏՓˎ��̒Ǔˏ������N��������"), Range(-1.0f, 0.0f)]
    float clashDot = -0.7f;

    [Space(padA), Header("--�C���^�[�o��--")]
    [SerializeField, Header("�ҋ@�C���^�[�o��")]
    float waitInterval = 2.0f;
    [SerializeField, Header("�ߋ����U���C���^�[�o��")]
    float closedInterval = 5.0f;
    [SerializeField, Header("�ǒǓˎ��C���^�[�o��")]
    float clashInterval = 2.0f;

    [Space(padA), Header("--�p�g���[���֘A--")]
    [SerializeField, Header("�p�g���[���R���|�[�l���g"), ReadOnly]
    Patrol patrol;
    [SerializeField, Header("�ړI�n��"), ReadOnly]
    int targetNum = 0;
    [SerializeField, Header("�ړI�n�̃C���f�b�N�X"), ReadOnly]
    int targetIndex = 0;
    [SerializeField, Header("���̖ړI�n"), ReadOnly]
    Vector3 nextTargetPos;
    bool bInitialize;

    [Space(padA), Header("--�A�j���[�V�����֘A--")]
    [SerializeField, Header("�A�j���[�^�[")]
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        patrol = GetComponent<Patrol>();
        animator = GetComponent<Animator>();
        targetNum = patrol.GetTargets().Length;
        nextTargetPos = patrol.GetTargets()[0].position;
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
        if(FindPlayerAtFOV().isFind)
        {
            cnt = 0.0f;
            state = State.Tracking;
            return;
        }
        if(cnt > waitInterval)
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
        if(FindPlayerAtFOV().isFind)
        {
            cnt = 0.0f;
            GetComponent<Rigidbody>().velocity =Vector3.zero;
            state = State.Tracking;
            return;
        }

        bool b = patrol.ExcutePatrol(targetIndex, moveSpeed, true);
        if(b)
        {
            targetIndex++;
            if(targetIndex >= targetNum)
            {
                targetIndex = 0;
            }
            nextTargetPos = patrol.GetTargets()[targetIndex].position;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            state = State.Idle;
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
        cnt += Time.deltaTime;
        // �v���C���[�̕����ɏ���������
        Vector3 targetVector = target.transform.position - transform.position;
        Vector3 axis = Vector3.Cross(transform.forward, targetVector);
        float angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0.0f ? -1.0f : 1.0f);
        angle = Mathf.Lerp(0.0f, angle, attackPrepareRotationRate);
        transform.Rotate(0.0f, angle * Time.deltaTime, 0.0f);
        if(Vector3.Dot(targetVector, transform.forward) > 0.9f)
        {
            transform.LookAt(target.transform.position);
            Vector3 Euler = transform.localEulerAngles;
            Euler.x = 0.0f;
            Euler.z = 0.0f;
            transform.localEulerAngles = Euler;
        }
        if (cnt > attackPrepareTime)
        {
            cnt = 0.0f;
            state = State.AttackA;
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
        
    }

    /*
    * <summary>
    * �U���֐�A
    * <param>
    * void
    * <return>
    * void
    */
    protected override void AttackFuncA()
    {
        cnt += Time.deltaTime;
        if(cnt >= attackContinueTime)
        {
            state = State.Idle;
        }
        transform.Translate(Vector3.forward * tackleSpeed * Time.deltaTime);
    }

    /*
    * <summary>
    * �U���֐�B
    * <param>
    * void
    * <return>
    * void
    */
    protected override void AttackFuncB()
    {
        
    }

    /*
    * <summary>
    * ����֐�A
    * <param>
    * void
    * <return>
    * void
    */
    protected override void SpecialFuncA()
    {
        // �ǒǓˎ��̃N���b�V������
        cnt += Time.deltaTime;
        if(cnt > clashInterval)
        {
            cnt = 0.0f;
            animator.SetBool("bClash", false);
        }
    }

    /*
    * <summary>
    * ����֐�B
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
    * �񕜊֐�
    * <param>
    * void
    * <return>
    * void
    */
    protected override void HealFunc()
    {
        
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

    private void OnCollisionEnter(Collision collision)
    {
        if(state == State.AttackA)
        {
            if(collision.transform.tag == "Wall")
            {
                Vector3 cNormal = collision.GetContact(0).normal;
                float dot = Vector3.Dot(cNormal, transform.forward);
                if (dot < clashDot)
                {
                    if(currentHp > 1)
                    {
                        state = State.SpecialA;
                        animator.SetBool("bClash", true);
                        Damage(clashDamege, Vector3.zero);
                    }
                    else
                    {
                        Damage(clashDamege, Vector3.zero);
                    }
                }
            }
        }
    }
}
