using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeC : Enemy_Mob
{
    enum MainState
    {[InspectorName("共通")] Common, [InspectorName("固有")] Unique };

    [Space(padA), Header("--メインパラメータ--")]
    [SerializeField, Header("ステート"), Toolbar(typeof(MainState))]
    MainState subState = MainState.Common;
    [SerializeField, Header("汎用カウント"), ReadOnly]
    float cnt;
    [SerializeField, Header("回転レート"), Range(0.0f, 3.0f)]
    float rotationRate = 0.6f;
    [SerializeField, Header("移動速度"), Range(0.0f, 30.0f)]
    float moveSpeed = 5.0f;
    [SerializeField, Header("移動時の消費圧力"), Range(0.0f, 20.0f)]
    float pressureForMove = 2.0f;
    [SerializeField, Header("追撃時の移動倍率"), Range(1.0f, 10.0f)]
    float trackingSpeedRate = 1.5f;
    [SerializeField, Header("1秒当たりの圧力回復量")]
    float pressurePerHeal = 5.0f;
    [SerializeField, Header("逃亡準備時間"), Range(0.0f, 5.0f)]
    float timeToPrepareEscape = 1.0f;
    [SerializeField, Header("逃亡時回転レート"), Range(0.0f, 2.0f)]
    float rotationRateForEscape = 0.9f;
    [SerializeField, Header("逃亡時間"), Range(0.0f, 20.0f)]
    float timeToEscape = 2.0f;

    [Space(padA), Header("--インターバル--")]
    [SerializeField, Header("待機インターバル")]
    float waitInterval;
    [SerializeField, Header("遠距離攻撃インターバル")]
    float rangedInterval;

    [Space(padA), Header("--状態遷移パラメータ--")]
    [SerializeField, Header("逃亡に移行する圧力量")]
    float pressureForEscapeMode = 12.0f;
    [SerializeField, Header("回復に移行する圧力量")]
    float pressureForHealMode = 15.0f;
    [SerializeField, Header("回復から復帰する圧力量")]
    float pressureForStoppedHeal = 50.0f;
    [SerializeField, Header("逃亡後攻撃するか"), ReadOnly]
    bool bAttackAfterEscape = false;

    [Space(padA), Header("--攻撃関連--")]

    [SerializeField, Header("遷移距離")]
    float distanceForAttack = 12.0f;
    [SerializeField, Header("逃亡後攻撃遷移距離")]
    float distanceForEscapeAfterAttack = 6.0f;
    [SerializeField, Header("消費圧力")]
    float pressureForAttack = 5.0f;
    [SerializeField, Header("最低必要圧力")]
    float lineForAttack = 10.0f;
    [SerializeField, Header("遠距離攻撃の弾")]
    GameObject objectForAttack;
    [SerializeField, Header("遠距離攻撃の速度")]
    float rangedAttackSpeed;
    [SerializeField, Header("攻撃オブジェクト生成位置")]
    Transform attackTransform;
    [SerializeField, Header("攻撃準備中か")]
    bool bPrepareAttack = false;

    [Space(padA), Header("--アニメーション関連--")]
    [SerializeField, Header("アニメーターコンポーネント"), ReadOnly]
    Animator animator;

    [Space(padA), Header("--パトロール関連--")]

    [SerializeField, Header("パトロールコンポーネント"), ReadOnly]
    Patrol patrol;
    [SerializeField, Header("目的地数"), ReadOnly]
    int targetNum = 0;
    [SerializeField, Header("目的地のインデックス"), ReadOnly]
    int targetIndex = 0;
    [SerializeField, Header("次の目的地"), ReadOnly]
    Vector3 nextTargetPos;

    Rigidbody rb;

    void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        patrol = GetComponent<Patrol>();
        animator = GetComponent<Animator>();
        targetNum = patrol.GetTargets().Length;
        attackTransform = transform.Find("AttackTransform");
        nextTargetPos = patrol.GetTargets()[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (subState)
        {
            case MainState.Common: base.Update(); break;
            case MainState.Unique: PrepareEscapeFunc(); break;
        }
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
            state = State.Tracking;
        }

        // プレイヤーの方向へ少しずつ回転
        //Vector3 targetVector = target.transform.position - transform.position;
        //Vector3 axis = Vector3.Cross(transform.forward, targetVector);
        //float angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0 ? -1.0f : 1.0f);
        //angle = Mathf.Lerp(0.0f, angle, rotationRate);
        //transform.Rotate(0.0f, angle * Time.deltaTime, 0.0f);


        // 次の目的地へ回転
        Vector3 targetVector = nextTargetPos - transform.position;
        Vector3 axis = Vector3.Cross(transform.forward, targetVector);
        float angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0 ? -1.0f : 1.0f);
        angle = Mathf.Lerp(0.0f, angle, cnt / waitInterval) * rotationRate;
        transform.Rotate(0.0f, angle, 0.0f);

        // 移動モードに移行
        if (cnt > waitInterval)
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
        cnt += Time.deltaTime;
        // 移動で圧力を使う
        currentPressure -= pressureForMove * Time.deltaTime;
        // 圧力回復モードに遷移
        if (currentPressure < pressureForHealMode)
        {
            state = State.Heal;
            return;
        }
        (bool isFind, float distance) = FindPlayerAtFOV();

        // 追尾モードに移行
        if (isFind)
        {
            cnt = 0.0f;
            state = State.Tracking;
            return;
        }
        bool b = patrol.ExcutePatrol(targetIndex, moveSpeed);

        // 待機モードに移行
        if (b)
        {
            cnt = 0.0f;
            state = State.Idle;
            targetIndex++;
            if (targetIndex >= targetNum)
            {
                targetIndex = 0;
            }
            nextTargetPos = patrol.GetTargets()[targetIndex].position;
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
        currentPressure -= pressureForMove * trackingSpeedRate * Time.deltaTime;

        (bool isFind, float distance) = FindPlayerAtFOV();
        // 待機モードに移行
        if (!isFind)
        {
            state = State.Idle;
        }
        else
        {
            // 距離が一定以下且つ現在圧力が遠距離最低ラインと逃亡に必要な圧力以上あるとき
            if (distance < distanceForEscapeAfterAttack * distanceForEscapeAfterAttack && currentPressure > lineForAttack + pressureForMove * trackingSpeedRate * timeToEscape)
            {
                cnt = 0.0f;
                state = State.Escape;
                bAttackAfterEscape = true;
                // ステートを固有ステート化
                subState = MainState.Unique;
                return;
            }
            else if (distance < distanceForAttack * distanceForAttack && currentPressure > lineForAttack)
            {
                cnt = 0.0f;
                currentPressure -= pressureForAttack;
                bPrepareAttack = true;
                animator.SetBool("bRanged", bPrepareAttack);
                state = State.AttackB;
                return;
            }
            else if (currentPressure < pressureForEscapeMode)
            {
                cnt = 0.0f;
                state = State.Escape;
                subState = MainState.Unique;
                return;
            }

            transform.LookAt(target.transform.position);
            Vector3 Euler = transform.localEulerAngles;
            Euler.x = 0.0f;
            Euler.z = 0.0f;
            transform.localEulerAngles = Euler;
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * trackingSpeedRate);
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
        currentPressure -= pressureForMove * trackingSpeedRate * Time.deltaTime;

        transform.Translate(Vector3.forward * moveSpeed * trackingSpeedRate * Time.deltaTime);

        if (cnt >= timeToEscape)
        {
            cnt = 0.0f;
            if (bAttackAfterEscape)
            {
                cnt = 0.0f;
                currentPressure -= pressureForAttack;
                bPrepareAttack = true;
                animator.SetBool("bRanged", bPrepareAttack);
                state = State.AttackB;
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
    * 攻撃状態関数A
    * <param>
    * void
    * <return>
    * void
    */
    protected override void AttackFuncA()
    {
        // 近距離攻撃無し
    }

    /*
    * <summary>
    * 攻撃状態関数B
    * <param>
    * void
    * <return>
    * void
    */
    protected override void AttackFuncB()
    {
        cnt += Time.deltaTime;

        // プレイヤーの方向へ少しずつ回転
        if (bPrepareAttack)
        {
            Vector3 targetVector = target.transform.position - transform.position;
            Vector3 axis = Vector3.Cross(transform.forward, targetVector);
            float angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0 ? -1.0f : 1.0f);
            if(Mathf.Abs(angle) > 10.0f)
            {
                angle = Mathf.Lerp(0.0f, angle, cnt / waitInterval) * rotationRate;
                transform.Rotate(0.0f, angle, 0.0f);
            }
            else
            {
                transform.LookAt(target.transform.position);
                Vector3 Euler = transform.localEulerAngles;
                Euler.x = 0.0f;
                Euler.z = 0.0f;
                transform.localEulerAngles = Euler;
            }
        }

        if (cnt > rangedInterval)
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
    protected override void SpecialFuncA()
    {
    }

    /*
    * <summary>
    * 特殊状態関数B
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
    * 回復状態関数
    * <param>
    * void
    * <return>
    * void
    */
    protected override void HealFunc()
    {
        cnt += Time.deltaTime;
        currentPressure += pressurePerHeal * Time.deltaTime;
        // 待機モードに移行
        if (currentPressure > pressureForStoppedHeal)
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
        base.DestroyFunc();

        animator.SetBool("bDeath", true);
    }

    /*
    * <summary>
    * 逃亡準備関数
    * 逃亡状態に入る前の準備関数
    * <param>
    * void
    * <return>
    * void
    */
    void PrepareEscapeFunc()
    {
        cnt += Time.deltaTime;
        // プレイヤーと逆方向へ少しずつ回転
        Vector3 targetVector = transform.position - target.transform.position;
        Vector3 axis = Vector3.Cross(transform.forward, targetVector);
        float angle = Vector3.Angle(transform.forward, targetVector) * (axis.y < 0 ? -1.0f : 1.0f);
        angle = Mathf.Lerp(0.0f, angle, rotationRateForEscape);
        transform.Rotate(0.0f, angle * Time.deltaTime, 0.0f);
        if (cnt > timeToPrepareEscape)
        {
            cnt = 0.0f;
            subState = MainState.Common;
        }
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
        GameObject bullet = Instantiate(objectForAttack, attackTransform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(AttackVector * rangedAttackSpeed, ForceMode.Impulse);
        // 遠距離攻撃boolをfalseに
        bPrepareAttack = false;
        animator.SetBool("bRanged", bPrepareAttack);
    }

    /*
     * <summary>
     * ダメージ関数
     * <param>
     * float damageValue, Vector3 direction
     * <return>
     * void
     */
    public override void Damage(float val, Vector3 direction)
    {
        rb.AddForce(direction * val, ForceMode.Impulse);
        base.Damage(val, direction);
    }
}
