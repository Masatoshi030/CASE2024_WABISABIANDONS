using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Subscriber
{
    static GameObject target;
    public static GameObject Target { get => target; }

    [SerializeField, Header("状態"), ReadOnly]
    string stateName;
    public string StateName { get => stateName; set => stateName = value; }

    // 敵ステートマシン
    EnemyStateMachine enemyStateMachine;
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
    Transform eyeTransform;
    public Transform EyeTransform { get => eyeTransform; }

    [SerializeField, Header("視認距離")]
    float viewDistance;
    public float ViewDistance { get => viewDistance; }
    [SerializeField, Header("視野角")]
    float viewAngle;
    public float ViewAngle { get => viewAngle; }

    // 索敵処理の有無
    bool isSearchPlayer = true;
    public bool IsSearchPlayer {set=> isSearchPlayer = value; }

    // プレイヤー発見の有無
    [SerializeField, Header("発見した"), ReadOnly]
    bool isFindPlayer = false;
    public bool IsFindPlayer { get => isFindPlayer; }

    // プレイヤーとの距離^2
    float toPlayerDistance = 0.0f;
    // プレイヤーとの2乗距離
    public float ToPlayerDistace { get => toPlayerDistance; }

    [SerializeField, Header("攻撃可能か"), ReadOnly]
    bool isAttackEnable = false;
    public bool IsAttackEnable { get => isAttackEnable; set => isAttackEnable = value; }

    Vector3 damageVector;
    public Vector3 DamageVector { get => damageVector; }

    bool isDamaged = false;
    public bool IsDamaged { get => isDamaged; set => isDamaged = value; }

    // 無敵状態
    bool isInvincible = false;
    public bool IsInvincible { set => isInvincible = value; }

    [SerializeField, Header("RigidBody")]
    Rigidbody rb;
    public Rigidbody EnemyRigidbody { get => rb; }

    [SerializeField, Header("アニメーター")]
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
        // パラメータの初期化
        enemyHp = maxHp;
        enemyPressure = maxPressure;
        // ステートマシンの設定
        enemyStateMachine = GetComponent<EnemyStateMachine>();
        enemyStateMachine.EnemyComponent = this;
        enemyStateMachine.Initialize();
    }

    void Update()
    {
        // 無敵をなくす
        isInvincible = false;
        rb.velocity = Vector3.zero;
        if (isSearchPlayer)
        {
            (bool find, float dis) = FindPlayerAtFOV();
            isFindPlayer = find;
            toPlayerDistance = dis;
        }
        // ステートマシンのメイン処理を呼ぶ
        enemyStateMachine.MainFunc();
    }

    /*
     * <summary>
     * プレイヤーを視野角を元に探す処理
     * <param>
     * なし
     * <returns>
     * bool isFind, float distance^2
     */
    public (bool isFind, float distance) FindPlayerAtFOV()
    {
        // 距離を測る
        Vector3 Diff = target.transform.position - eyeTransform.position;
        float distance = Diff.x * Diff.x + Diff.y * Diff.y + Diff.z * Diff.z;

        if (distance < viewDistance * viewDistance)
        {
            // 外積でy軸から正面の左右どちらにあるか求める
            Vector3 axis = Vector3.Cross(eyeTransform.forward, Diff);

            // 角度が+なら正面より右にいる-なら左にいる
            float angle = Vector3.Angle(eyeTransform.forward, Diff) * (axis.y < 0 ? -1.0f : 1.0f);

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
                        return (true, distance);
                    }
                }
            }
        }
        return (false, distance);
    }

    /*
     * <summary>
     * ダメージ関数
     * <param>
     * float ダメージ量, Vector3 攻撃ベクトル
     * <return>
     * bool HPが0以下か
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
