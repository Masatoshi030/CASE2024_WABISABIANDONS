using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeB : Enemy_Mob
{
    [System.Serializable]
    public struct AttackParam
    {
        [SerializeField, Header("�U����������")]
        public float preTime;
        [SerializeField, Header("�ːi���x")]
        public float tackleSpeed;
        [SerializeField, Header("�ːi�U����")]
        public float tacklePower;
        [SerializeField, Header("�U���p������")]
        public float continueTime;
        [SerializeField, Header("�ǏՓˎ��̒Ǔˏ������N��������"), Range(-1.0f, 0.0f)]
        public float clashDot;
        [SerializeField, Header("�ߋ����U���C���^�[�o��")]
        public float interval;
    }

    [Space(padA), Header("--���C���p�����[�^--")]
    [SerializeField, Header("�ėp�J�E���g"), ReadOnly]
    float cnt;
    [Space(padB), Header("-�ҋ@�p�����[�^-")]
    [SerializeField, Header("�ҋ@�C���^�[�o��")]
    float waitInterval = 2.0f;
    [Space(padB), SerializeField, Header("-�ړ��p�����[�^-")]
    EnemyAgentParam moveParam;
    [Space(padB), Header("-�ǔ��p�����[�^-")]
    [SerializeField, Header("��]���x")]
    float rotateSpeed;
    [SerializeField, Header("��ԑ��x")]
    float interpolationSpeed;
    [SerializeField, Header("�^�[�Q�b�g�O��ʒu"), ReadOnly]
    Vector3 lastTargetPosition;
    [Space(padB), SerializeField, Header("-�U���p�����[�^-")]
    AttackParam attackParam;
    [Space(padB), Header("-�ǌ��˃p�����[�^-")]
    [SerializeField, Header("�ǌ��ˎ��̃_���[�W")]
    float clashDamege;
    [SerializeField, Header("�ǌ��ˎ��C���^�[�o��")]
    float clashInterval;

    [Space(padA), Header("--�p�g���[���֘A--")]
    NavMeshPatrol patrol;
    [SerializeField, Header("�ړI�n��"), ReadOnly]
    int targetNum = 0;
    [SerializeField, Header("�ړI�n�̃C���f�b�N�X"), ReadOnly]
    int targetIndex = 0;

    // �A�j���[�^�[
    Animator animator;

    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        patrol = GetComponent<NavMeshPatrol>();
        animator = GetComponent<Animator>();
        targetNum = patrol.GetTargets().Length;
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
            lastTargetPosition = target.transform.position;
            return;
        }
        if(cnt > waitInterval)
        {
            cnt = 0.0f;
            state = State.Move;
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
        if(FindPlayerAtFOV().isFind)
        {
            cnt = 0.0f;
            rb.velocity =Vector3.zero;
            state = State.Tracking;
            lastTargetPosition = target.transform.position;
            patrol.Stop();
            return;
        }

        if(patrol.GetPatrolState() == NavMeshPatrol.PatrolState.Idle)
        {
            targetIndex++;
            if(targetIndex >= targetNum)
            {
                targetIndex = 0;
            }
            rb.velocity = Vector3.zero;
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
        Vector3 direction = target.transform.position - transform.position;
        Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
        if(Vector3.Dot(direction, transform.forward) > 0.9f)
        {
            transform.LookAt(target.transform.position);
            Vector3 Euler = transform.localEulerAngles;
            Euler.x = 0.0f;
            Euler.z = 0.0f;
            transform.localEulerAngles = Euler;
        }
        if (cnt > attackParam.preTime)
        {
            cnt = 0.0f;
            state = State.AttackA;
        }
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
        if(cnt >= attackParam.continueTime)
        {
            state = State.Idle;
        }
        transform.Translate(Vector3.forward * attackParam.tackleSpeed * Time.deltaTime);
    }

    /*
    * <summary>
    * �ǌ��ˎ��֐�
    * <param>
    * void
    * <return>
    * void
    */
    protected override void UniqueFuncA()
    {
        // �ǒǓˎ��̃N���b�V������
        cnt += Time.deltaTime;
        if(cnt > clashInterval)
        {
            rb.isKinematic = false;
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
    protected override void UniqueFuncB()
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
        state = State.DeathWait;
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
                if (dot < attackParam.clashDot)
                {
                    if(currentHp - clashDamege > 0)
                    {
                        state = State.UniqueA;
                        animator.SetBool("bClash", true);
                        Damage(clashDamege, Vector3.zero);
                        rb.isKinematic = true;
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
