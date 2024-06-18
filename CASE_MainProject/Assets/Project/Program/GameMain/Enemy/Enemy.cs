using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Subscriber
{
    static protected GameObject target;
    public static GameObject Target { get => target; }

    [SerializeField, Header("状態"), ReadOnly]
    protected string stateName;
    public string StateName { get => stateName; set => stateName = value; }

    // 敵ステートマシン
    protected EnemyStateMachine enemyStateMachine;
    public EnemyStateMachine Machine { get => enemyStateMachine; }

    [SerializeField, Header("最大HP")]
    float maxHp;
    [SerializeField, Header("HP"), ReadOnly]
    float enemyHp;
    public float Hp { get => enemyHp; }
    [SerializeField, Header("最大圧力")]
    float maxPressure;
    [SerializeField, Header("圧力"), ReadOnly]
    float enemyPressure;
    public float Pressre { get => enemyPressure; }
    [SerializeField, Header("視点")]
    protected Transform eyeTransform;
    public Transform EyeTransform { get => eyeTransform; }

    [SerializeField, Header("視認距離")]
    protected float viewDistance;
    public float ViewDistance { get => viewDistance; }
    [SerializeField, Header("視野角")]
    protected float viewAngle;
    public float ViewAngle { get => viewAngle; }

    // 索敵処理の有無
    protected bool isSearchPlayer = true;
    public bool IsSearchPlayer {set=> isSearchPlayer = value; }

    // プレイヤー発見の有無
    [SerializeField, Header("発見した"), ReadOnly]
    protected bool isFindPlayer = false;
    public bool IsFindPlayer { get => isFindPlayer; }

    // プレイヤーとの距離^2
    protected float toPlayerDistance = 0.0f;
    // プレイヤーとの2乗距離
    public float ToPlayerDistace { get => toPlayerDistance; }

    protected float toPlayerAngle = 0.0f;
    public float ToPlayerAngle { get => toPlayerAngle; }

    [SerializeField, Header("攻撃可能か"), ReadOnly]
    protected bool isAttackEnable = false;
    public bool IsAttackEnable { get => isAttackEnable; set => isAttackEnable = value; }

    protected Vector3 damageVector;
    public Vector3 DamageVector { get => damageVector; }

    protected bool isDamaged = false;
    public bool IsDamaged { get => isDamaged; set => isDamaged = value; }

    // 無敵状態
    protected bool isInvincible = false;
    public bool IsInvincible { set => isInvincible = value; }

    protected bool isVelocityZero = false;

    public bool IsVelocityZero { get => IsVelocityZero; set => isVelocityZero = value; }


    [SerializeField, Header("RigidBody")]
    Rigidbody rb;
    public Rigidbody EnemyRigidbody { get => rb; }

    [SerializeField, Header("アニメーター")]
    protected Animator animator;
    public Animator EnemyAnimator { get => animator; }

    [SerializeField, Header("コライダー")]
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
        // パラメータの初期化
        enemyHp = maxHp;
        enemyPressure = maxPressure;
        // ステートマシンの設定
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
        // 無敵をなくす
        isInvincible = false;
        if (isSearchPlayer)
        {
            (bool find, float dis, float degree) = FindPlayerAtFOV();
            isFindPlayer = find;
            toPlayerDistance = dis;
            toPlayerAngle = degree;
        }
        // ステートマシンのメイン処理
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
     * プレイヤーを視野角を元に探す処理
     * <param>
     * なし
     * <returns>
     * bool isFind, float distance^2
     */
    public (bool isFind, float distance, float angle) FindPlayerAtFOV()
    {
        // 距離を測る
        Vector3 Diff = target.transform.position - eyeTransform.position;
        float distance = Diff.x * Diff.x + Diff.y * Diff.y + Diff.z * Diff.z;

        // 外積でy軸から正面の左右どちらにあるか求める
        Vector3 axis = Vector3.Cross(eyeTransform.forward, Diff);

        // 角度が+なら正面より右にいる-なら左にいる
        float angle = Vector3.Angle(eyeTransform.forward, Diff) * (axis.y < 0 ? -1.0f : 1.0f);

        if (distance < viewDistance * viewDistance)
        {
            // 視野角内か判定
            if (Mathf.Abs(angle) < viewAngle / 2)
            {
                Vector3 Direction = Diff.normalized;
                // レイの作成と表示
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
     * ダメージ関数
     * <param>
     * float ダメージ量, Vector3 攻撃ベクトル
     * <return>
     * bool HPが0以下か
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
