using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeC : Enemy_Mob
{
    [System.Serializable]
    public struct AttackParam
    {
        [SerializeField, Header("遷移距離")]
        public float distance;
        [SerializeField, Header("消費圧力")]
        public float consumePressure;
        [SerializeField, Header("必要最低限の圧力量")]
        public float requiredPressure;
        [SerializeField, Header("攻撃の弾")]
        public GameObject bullet;
        [SerializeField, Header("弾の速度")]
        public float bulletSpeed;
        [SerializeField, Header("インターバル")]
        public float interval;
    }

    [Space(padA), Header("--メインパラメータ--")]
    [SerializeField, Header("汎用カウント"), ReadOnly]
    float cnt;
    [Space(padB), Header("-待機パラメータ-")]
    [SerializeField, Header("待機インターバル")]
    float waitInterval;
    [Space(padB), SerializeField, Header("移動パラメータ")]
    EnemyAgentParam moveParam;
    [Space(padB), SerializeField, Header("追尾パラメータ")]
    EnemyAgentParam trackingParam;
    [SerializeField, Header("近すぎて逃げる距離")]
    float distanceTooClose = 6.0f;
    [Space(padB), SerializeField, Header("逃亡パラメータ")]
    EnemyAgentParam escapeParam;
    [Space(padB), SerializeField, Header("逃亡準備時間"), Range(0.0f, 5.0f)]
    float escapePrepareTime = 1.0f;
    [Space(padB), SerializeField, Header("逃亡時間"), Range(0.0f, 20.0f)]
    float timeToEscape = 2.0f;
    float angle = 0.0f;
    [SerializeField, Header("逃亡に移行ライン")]
    float pressureForEscapeMode = 12.0f;
    [SerializeField, Header("逃亡後攻撃するか"), ReadOnly]
    bool bAttackAfterEscape = false;
    [Space(padB), SerializeField, Header("-攻撃パラメータ-")]
    AttackParam attackParam;
    
    
    [Space(padB), SerializeField, Header("-回復パラメータ-")]
    HealParam healParam;
    
    
    
    
    [SerializeField, Header("プレイヤーの方にどれだけ向くか")]
    float attackLookRate = 0.85f;
    [SerializeField, Header("回転速度")]
    float rotationSpeed = 5.0f;
    Transform attackTransform;
    bool bPrepareAttack = false;

    Animator animator;

    [Space(padA), Header("--パトロール関連--")]
    NavMeshPatrol patrol;
    [SerializeField, Header("目的地数"), ReadOnly]
    int targetNum = 0;
    [SerializeField, Header("目的地のインデックス"), ReadOnly]
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
    * 待機状態関数
    * <param>
    * void
    * <return>
    * void
     */
    protected override void IdleFunc()
    {
        cnt += Time.deltaTime;
        (bool isFind, float distance) = FindPlayerAtFOV();

        // 追尾モードに移行
        if (isFind)
        {
            cnt = 0.0f;
            patrol.SetAgentParam(trackingParam, true);
            state = State.Tracking;
        }
        // 次の目的地へ回転
        //Vector3 targetVector = nextTargetPos - transform.position;
        //Vector3 axis = Vector3.Cross(transform.forward, targetVector);
        //float angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0 ? -1.0f : 1.0f);
        //angle = Mathf.Lerp(0.0f, angle, cnt / waitInterval) * rotationRate;
        //transform.Rotate(0.0f, angle, 0.0f);

        // 移動モードに移行
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
    * 移動状態関数
    * <param>
    * void
    * <return>
    * void
    */
    protected override void MoveFunc()
    {
        cnt += Time.deltaTime;
        // 移動で圧力を使う
        currentPressure -= moveParam.consumePressure * Time.deltaTime;

        // 圧力回復モードに遷移
        if (currentPressure < healParam.startHealLine)
        {
            state = State.Heal;
            return;
        }
        (bool isFind, float distance) = FindPlayerAtFOV();

        // 追尾モードに移行
        if (isFind)
        {
            cnt = 0.0f;
            patrol.SetAgentParam(trackingParam);
            state = State.Tracking;
            return;
        }

        // 待機モードに移行
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
    * 追尾状態関数
    * <param>
    * void
    * <return>
    * void
    */
    protected override void TrackingFunc()
    {
        // 追尾に圧力を使う
        currentPressure -= trackingParam.consumePressure * Time.deltaTime;

        (bool isFind, float distance) = FindPlayerAtFOV();
        // 待機モードに移行
        if (!isFind)
        {
            state = State.Idle;
        }
        else
        {
            // 逃亡後攻撃
            if (distance < distanceTooClose * distanceTooClose && currentPressure > attackParam.requiredPressure + trackingParam.consumePressure * timeToEscape)
            {
                cnt = 0.0f;
                bAttackAfterEscape = true;
                // 回転量の生成
                Vector3 targetVector = transform.position - target.transform.position;
                Vector3 axis = Vector3.Cross(transform.forward, targetVector);
                angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0.0f ? -1.0f : 1.0f);    // 回転方向の計算
                angle /= escapePrepareTime; // 1秒当たりの回転量に直す
                state = State.UniqueA;
                return;
            }
            // 攻撃
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
            // 逃亡準備に移行
            else if (currentPressure < pressureForEscapeMode)
            {
                cnt = 0.0f;
                // 回転量の生成
                Vector3 targetVector = transform.position - target.transform.position;
                Vector3 axis = Vector3.Cross(transform.forward, targetVector);
                angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0.0f ? -1.0f : 1.0f);    // 回転方向の計算
                angle /= escapePrepareTime; // 1秒当たりの回転量に直す
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
    * 逃亡状態関数
    * <param>
    * void
    * <return>
    * void
    */
    protected override void EscapeFunc()
    {
        cnt += Time.deltaTime;
        currentPressure -= escapeParam.consumePressure * Time.deltaTime;

        // 移動
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
    * 攻撃状態関数B
    * <param>
    * void
    * <return>
    * void
    */
    protected override void AttackFuncA()
    {
        cnt += Time.deltaTime;

        // プレイヤーの方向へ少しずつ回転
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
    * 特殊状態関数A
    * <param>
    * void
    * <return>
    * void
    */
    protected override void UniqueFuncA()
    {
        cnt += Time.deltaTime;
        // プレイヤーと逆方向へ少しずつ回転
        transform.Rotate(0.0f, angle * Time.deltaTime, 0.0f);
        if (cnt > escapePrepareTime)
        {
            state = State.Escape;
            cnt = 0.0f;
        }
    }

    /*
    * <summary>
    * 回復状態関数
    * <param>
    * void
    * <return>
    * void
    */
    protected override void HealFunc()
    {
        cnt += Time.deltaTime;
        currentPressure += healPressureVal * Time.deltaTime;
        // 待機モードに移行
        if (currentPressure > healParam.endHealLine)
        {
            cnt = 0.0f;
            state = State.Idle;
        }
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
        // この前に死亡エフェクトを入れたい
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
        //animator.SetBool("bDeath", true);
        //state = State.DeathWait;
        state = State.Death;

    }

    /*
    * <summary>
    * 状態判断関数
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
    * 遠距離攻撃生成関数
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
        // 遠距離攻撃boolをfalseに
        bPrepareAttack = false;
        animator.SetBool("bAttack", bPrepareAttack);
    }

    /*
     * <summary>
     * ダメージ関数
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
