using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Subscriber
{
    static GameObject target;
    public static GameObject Target { get => target; }

    [SerializeField, Header("���"), ReadOnly]
    string stateName;
    public string StateName { get => stateName; set => stateName = value; }

    // �G�X�e�[�g�}�V��
    EnemyStateMachine enemyStateMachine;
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
    Transform eyeTransform;
    public Transform EyeTransform { get => eyeTransform; }

    [SerializeField, Header("���F����")]
    float viewDistance;
    public float ViewDistance { get => viewDistance; }
    [SerializeField, Header("����p")]
    float viewAngle;
    public float ViewAngle { get => viewAngle; }

    // ���G�����̗L��
    bool isSearchPlayer = true;
    public bool IsSearchPlayer {set=> isSearchPlayer = value; }

    // �v���C���[�����̗L��
    [SerializeField, Header("��������"), ReadOnly]
    bool isFindPlayer = false;
    public bool IsFindPlayer { get => isFindPlayer; }

    // �v���C���[�Ƃ̋���^2
    float toPlayerDistance = 0.0f;
    // �v���C���[�Ƃ�2�拗��
    public float ToPlayerDistace { get => toPlayerDistance; }

    [SerializeField, Header("�U���\��"), ReadOnly]
    bool isAttackEnable = false;
    public bool IsAttackEnable { get => isAttackEnable; set => isAttackEnable = value; }

    Vector3 damageVector;
    public Vector3 DamageVector { get => damageVector; }

    bool isDamaged = false;
    public bool IsDamaged { get => isDamaged; set => isDamaged = value; }

    // ���G���
    bool isInvincible = false;
    public bool IsInvincible { set => isInvincible = value; }

    [SerializeField, Header("RigidBody")]
    Rigidbody rb;
    public Rigidbody EnemyRigidbody { get => rb; }

    [SerializeField, Header("�A�j���[�^�[")]
    Animator animator;
    public Animator EnemyAnimator { get => animator; }

    private void Awake()
    {
        if(target == null)
        {
            target = GameObject.Find("Player");
        }
        eyeTransform = transform.Find("EyeTransform");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // �p�����[�^�̏�����
        enemyHp = maxHp;
        enemyPressure = maxPressure;
        // �X�e�[�g�}�V���̐ݒ�
        enemyStateMachine = GetComponent<EnemyStateMachine>();
        enemyStateMachine.EnemyComponent = this;
        enemyStateMachine.Initialize();
    }

    void Update()
    {
        // ���G���Ȃ���
        isInvincible = false;
        rb.velocity = Vector3.zero;
        if (isSearchPlayer)
        {
            (bool find, float dis) = FindPlayerAtFOV();
            isFindPlayer = find;
            toPlayerDistance = dis;
        }
        // �X�e�[�g�}�V���̃��C���������Ă�
        enemyStateMachine.MainFunc();
    }

    /*
     * <summary>
     * �v���C���[������p�����ɒT������
     * <param>
     * �Ȃ�
     * <returns>
     * bool isFind, float distance^2
     */
    public (bool isFind, float distance) FindPlayerAtFOV()
    {
        // �����𑪂�
        Vector3 Diff = target.transform.position - eyeTransform.position;
        float distance = Diff.x * Diff.x + Diff.y * Diff.y + Diff.z * Diff.z;

        if (distance < viewDistance * viewDistance)
        {
            // �O�ς�y�����琳�ʂ̍��E�ǂ���ɂ��邩���߂�
            Vector3 axis = Vector3.Cross(eyeTransform.forward, Diff);

            // �p�x��+�Ȃ琳�ʂ��E�ɂ���-�Ȃ獶�ɂ���
            float angle = Vector3.Angle(eyeTransform.forward, Diff) * (axis.y < 0 ? -1.0f : 1.0f);

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
                        return (true, distance);
                    }
                }
            }
        }
        return (false, distance);
    }

    /*
     * <summary>
     * �_���[�W�֐�
     * <param>
     * float �_���[�W��, Vector3 �U���x�N�g��
     * <return>
     * bool HP��0�ȉ���
     */
    public bool Damage(float damage, Vector3 direction)
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
}
