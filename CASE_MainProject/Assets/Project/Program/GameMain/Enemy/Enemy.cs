using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Subscriber
{
    static protected GameObject target;
    public static GameObject Target { get => target; }

    [SerializeField, Header("���"), ReadOnly]
    protected string stateName;
    public string StateName { get => stateName; set => stateName = value; }

    // �G�X�e�[�g�}�V��
    protected EnemyStateMachine enemyStateMachine;
    public EnemyStateMachine Machine { get => enemyStateMachine; }

    [SerializeField, Header("�ő�HP")]
    float maxHp;
    [SerializeField, Header("HP"), ReadOnly]
    float enemyHp;
    public float Hp { get => enemyHp; }
    [SerializeField, Header("�ő刳��")]
    float maxPressure;
    [SerializeField, Header("����"), ReadOnly]
    float enemyPressure;
    public float Pressre { get => enemyPressure; }
    [SerializeField, Header("���_")]
    protected Transform eyeTransform;
    public Transform EyeTransform { get => eyeTransform; }

    [SerializeField, Header("���F����")]
    protected float viewDistance;
    public float ViewDistance { get => viewDistance; }
    [SerializeField, Header("����p")]
    protected float viewAngle;
    public float ViewAngle { get => viewAngle; }

    // ���G�����̗L��
    protected bool isSearchPlayer = true;
    public bool IsSearchPlayer {set=> isSearchPlayer = value; }

    // �v���C���[�����̗L��
    [SerializeField, Header("��������"), ReadOnly]
    protected bool isFindPlayer = false;
    public bool IsFindPlayer { get => isFindPlayer; }

    // �v���C���[�Ƃ̋���^2
    protected float toPlayerDistance = 0.0f;
    // �v���C���[�Ƃ�2�拗��
    public float ToPlayerDistace { get => toPlayerDistance; }

    protected float toPlayerAngle = 0.0f;
    public float ToPlayerAngle { get => toPlayerAngle; }

    [SerializeField, Header("�U���\��"), ReadOnly]
    protected bool isAttackEnable = false;
    public bool IsAttackEnable { get => isAttackEnable; set => isAttackEnable = value; }

    protected Vector3 damageVector;
    public Vector3 DamageVector { get => damageVector; }

    protected bool isDamaged = false;
    public bool IsDamaged { get => isDamaged; set => isDamaged = value; }

    // ���G���
    protected bool isInvincible = false;
    public bool IsInvincible { set => isInvincible = value; }

    protected bool isVelocityZero = false;

    public bool IsVelocityZero { get => IsVelocityZero; set => isVelocityZero = value; }


    [SerializeField, Header("RigidBody")]
    Rigidbody rb;
    public Rigidbody EnemyRigidbody { get => rb; }

    [SerializeField, Header("�A�j���[�^�[")]
    protected Animator animator;
    public Animator EnemyAnimator { get => animator; }

    [SerializeField, Header("�R���C�_�[")]
    protected Collider enemyCollider;
    public Collider EnemyCollider { get => enemyCollider; }
    [SerializeField, Header("NavMeshAgent")]
    protected NavMeshAgent enemyAgent;

    public NavMeshAgent EnemyAgent { get=> enemyAgent; }

    protected void Awake()
    {
        if(target == null)
        {
            target = GameObject.Find("Player");
        }
        eyeTransform = transform.Find("EyeTransform");
        rb = GetComponent<Rigidbody>();
        if(GetComponent<NavMeshAgent>())
            enemyAgent = GetComponent<NavMeshAgent>();
    }

    protected void Start()
    {
        // �p�����[�^�̏�����
        enemyHp = maxHp;
        enemyPressure = maxPressure;
        // �X�e�[�g�}�V���̐ݒ�
        enemyStateMachine = GetComponent<EnemyStateMachine>();
        enemyStateMachine.EnemyComponent = this;
        enemyStateMachine.Initialize();
    }

    protected void Update()
    {
        if(isVelocityZero)
        {
            rb.velocity = Vector3.zero;
        }
        // ���G���Ȃ���
        isInvincible = false;
        if (isSearchPlayer)
        {
            (bool find, float dis, float degree) = FindPlayerAtFOV();
            isFindPlayer = find;
            toPlayerDistance = dis;
            toPlayerAngle = degree;
        }
        // �X�e�[�g�}�V���̃��C������
        if (enemyStateMachine.IsUpdate)
        {
            enemyStateMachine.MainFunc();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        enemyStateMachine.TriggerEnterSelf(other);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        enemyStateMachine.CollisionEnterSelf(collision);
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        enemyStateMachine.TriggerStaySelf(other);
    }

    protected virtual void OnCollisionStay(Collision collision)
    {
        enemyStateMachine.CollisionStaySelf(collision);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        enemyStateMachine.TriggerExitSelf(other);
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        enemyStateMachine.CollisionExitSelf(collision);
    }
    /*
     * <summary>
     * �v���C���[������p�����ɒT������
     * <param>
     * �Ȃ�
     * <returns>
     * bool isFind, float distance^2
     */
    public (bool isFind, float distance, float angle) FindPlayerAtFOV()
    {
        // �����𑪂�
        Vector3 Diff = target.transform.position - eyeTransform.position;
        float distance = Diff.x * Diff.x + Diff.y * Diff.y + Diff.z * Diff.z;

        // �O�ς�y�����琳�ʂ̍��E�ǂ���ɂ��邩���߂�
        Vector3 axis = Vector3.Cross(eyeTransform.forward, Diff);

        // �p�x��+�Ȃ琳�ʂ��E�ɂ���-�Ȃ獶�ɂ���
        float angle = Vector3.Angle(eyeTransform.forward, Diff) * (axis.y < 0 ? -1.0f : 1.0f);

        if (distance < viewDistance * viewDistance)
        {
            // ����p��������
            if (Mathf.Abs(angle) < viewAngle / 2)
            {
                Vector3 Direction = Diff.normalized;
                // ���C�̍쐬�ƕ\��
                RaycastHit[] hits = Physics.RaycastAll(eyeTransform.position, Direction, viewDistance);
                if(hits.Length > 0)
                {
                    if(hits[0].transform.root.name == "Player")
                    {
                        return (true, distance, angle);
                    }
                }
            }
        }
        return (false, distance, angle);
    }

    /*
     * <summary>
     * �_���[�W�֐�
     * <param>
     * float �_���[�W��, Vector3 �U���x�N�g��
     * <return>
     * bool HP��0�ȉ���
     */
    public virtual bool Damage(float damage, Vector3 direction)
    {
        if(!isDamaged)
        {
            damageVector = direction;
            if (!isInvincible) enemyHp -= damage;
            isDamaged = true;

            if (enemyHp <= 0)
            {
                return true;
            }
        }
        return false;
    }

    public void CalcPressure(float value)
    {
        enemyPressure += value;
        if (enemyPressure > maxPressure) enemyPressure = maxPressure;
        else if (enemyPressure < 0.0f) enemyPressure = 0.0f;
    }
}
