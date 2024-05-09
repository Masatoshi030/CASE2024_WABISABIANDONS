using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeB : Enemy_Mob
{
    [Space(padA), Header("--メインパラメータ--")]
    [SerializeField, Header("汎用カウント"), ReadOnly]
    float cnt;
    [SerializeField, Header("移動速度")]
    float moveSpeed = 3.0f;
    [SerializeField, Header("壁激突時のダメージ")]
    float clashDamege = 1.0f;
    

    [Space(padB), Header("--攻撃関連--")]
    [SerializeField, Header("攻撃準備時間")]
    float attackPrepareTime = 1.2f;
    [SerializeField, Header("攻撃準備時の向き補間の割合"), Range(0.0f, 3.0f)]
    float attackPrepareRotationRate = 1.5f;
    [SerializeField, Header("突進時速度")]
    float tackleSpeed = 8.0f;
    [SerializeField, Header("突進攻撃力")]
    float tacklePower = 3.0f;
    [SerializeField, Header("攻撃継続時間")]
    float attackContinueTime = 2.0f;
    [SerializeField, Header("壁衝突時の追突処理を起こす内積"), Range(-1.0f, 0.0f)]
    float clashDot = -0.7f;

    [Space(padA), Header("--インターバル--")]
    [SerializeField, Header("待機インターバル")]
    float waitInterval = 2.0f;
    [SerializeField, Header("近距離攻撃インターバル")]
    float closedInterval = 5.0f;
    [SerializeField, Header("壁追突時インターバル")]
    float clashInterval = 2.0f;

    [Space(padA), Header("--パトロール関連--")]
    [SerializeField, Header("パトロールコンポーネント"), ReadOnly]
    Patrol patrol;
    [SerializeField, Header("目的地数"), ReadOnly]
    int targetNum = 0;
    [SerializeField, Header("目的地のインデックス"), ReadOnly]
    int targetIndex = 0;
    [SerializeField, Header("次の目的地"), ReadOnly]
    Vector3 nextTargetPos;
    bool bInitialize;

    [Space(padA), Header("--アニメーション関連--")]
    [SerializeField, Header("アニメーター")]
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
    * 逃亡状態関数
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
    * 攻撃関数A
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
    * 攻撃関数B
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
    * 特殊関数A
    * <param>
    * void
    * <return>
    * void
    */
    protected override void SpecialFuncA()
    {
        // 壁追突時のクラッシュ処理
        cnt += Time.deltaTime;
        if(cnt > clashInterval)
        {
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
    protected override void SpecialFuncB()
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
