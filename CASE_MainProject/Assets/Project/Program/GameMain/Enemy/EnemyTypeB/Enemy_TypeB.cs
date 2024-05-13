using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeB : Enemy_Mob
{
    [System.Serializable]
    public struct AttackParam
    {
        [SerializeField, Header("攻撃準備時間")]
        public float preTime;
        [SerializeField, Header("突進速度")]
        public float tackleSpeed;
        [SerializeField, Header("突進攻撃力")]
        public float tacklePower;
        [SerializeField, Header("攻撃継続時間")]
        public float continueTime;
        [SerializeField, Header("壁衝突時の追突処理を起こす内積"), Range(-1.0f, 0.0f)]
        public float clashDot;
        [SerializeField, Header("近距離攻撃インターバル")]
        public float interval;
    }

    [Space(padA), Header("--メインパラメータ--")]
    [SerializeField, Header("汎用カウント"), ReadOnly]
    float cnt;
    [Space(padB), Header("-待機パラメータ-")]
    [SerializeField, Header("待機インターバル")]
    float waitInterval = 2.0f;
    [Space(padB), SerializeField, Header("-移動パラメータ-")]
    EnemyAgentParam moveParam;
    [Space(padB), Header("-追尾パラメータ-")]
    [SerializeField, Header("回転速度")]
    float rotateSpeed;
    [SerializeField, Header("補間速度")]
    float interpolationSpeed;
    [SerializeField, Header("ターゲット前回位置"), ReadOnly]
    Vector3 lastTargetPosition;
    [Space(padB), SerializeField, Header("-攻撃パラメータ-")]
    AttackParam attackParam;
    [Space(padB), Header("-壁激突パラメータ-")]
    [SerializeField, Header("壁激突時のダメージ")]
    float clashDamege;
    [SerializeField, Header("壁激突時インターバル")]
    float clashInterval;

    [Space(padA), Header("--パトロール関連--")]
    NavMeshPatrol patrol;
    [SerializeField, Header("目的地数"), ReadOnly]
    int targetNum = 0;
    [SerializeField, Header("目的地のインデックス"), ReadOnly]
    int targetIndex = 0;

    // アニメーター
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
    * 待機状態関数
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
    * 移動状態関数
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
    * 追尾状態関数
    * <param>
    * void
    * <return>
    * void
    */
    protected override void TrackingFunc()
    {
        cnt += Time.deltaTime;
        // プレイヤーの方向に少しずつ向く
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
    * 攻撃関数A
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
    * 壁激突時関数
    * <param>
    * void
    * <return>
    * void
    */
    protected override void UniqueFuncA()
    {
        // 壁追突時のクラッシュ処理
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
    * 特殊関数B
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
    * 回復関数
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
    * 死亡状態関数
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
    * 破壊時呼び出し関数
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
