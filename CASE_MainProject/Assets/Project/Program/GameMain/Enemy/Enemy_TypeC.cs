using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeC : Enemy_Mob
{
    const float pad = 20.0f;

    [Space(pad), Header("--インターバル--")]
    [SerializeField, Header("待機インターバル")]
    float waitInterval;
    [SerializeField, Header("移動インターバル")]
    float moveInterval;
    [SerializeField, Header("近距離攻撃インターバル")]
    float closeInterval;
    [SerializeField, Header("遠距離攻撃インターバル")]
    float rangedInterval;

    [Space(pad), Header("--メインパラメータ--")]

    [SerializeField, Header("汎用カウント"), ReadOnly]
    float cnt;
    [SerializeField, Header("回転速度"), Range(0.0f, 1.0f)]
    float rotationRate = 0.3f;
    [SerializeField, Header("移動速度"), Range(0.0f, 30.0f)]
    float moveSpeed = 5.0f;
    [SerializeField, Header("移動時の消費圧力"), Range(0.0f, 20.0f)]
    float pressureForMove = 2.0f;
    [SerializeField, Header("追撃時の移動倍率"), Range(1.0f, 10.0f)]
    float trackingSpeedRate = 1.5f;
    [SerializeField, Header("回復に移行する圧力量")]
    float pressureForHealMode = 15.0f;
    [SerializeField, Header("1秒当たりの圧力回復量")]
    float pressurePerHeal = 5.0f;
    [SerializeField, Header("回復から復帰する圧力量")]
    float pressureForStoppedHeal = 50.0f;

    [Space(pad), Header("--遠距離攻撃--")]

    [SerializeField, Header("遷移距離")]
    float distanceForRangedAttack = 12.0f;
    [SerializeField, Header("消費圧力")]
    float pressureForRangedAttack = 5.0f;
    [SerializeField, Header("最低必要圧力")]
    float lineForRagnedAttack = 10.0f;
    [SerializeField, Header("遠距離攻撃の弾")]
    GameObject objectForRangedAttack;
    [SerializeField, Header("遠距離攻撃の速度")]
    float rangedAttackSpeed;

    [Space(pad), Header("--近距離攻撃--")]

    [SerializeField, Header("遷移距離")]
    float distanceForCloseAttack = 5.0f;
    [SerializeField, Header("消費圧力")]
    float pressureForCloseAttack = 2.0f;
    [SerializeField, Header("最低必要圧力")]
    float lineForCloseAttack = 7.0f;

    [Space(pad), Header("--パトロール関連--")]

    [SerializeField, Header("パトロールコンポーネント"), ReadOnly]
    Patrol patrol;
    [SerializeField, Header("目的地数"),ReadOnly]
    int targetNum = 0;
    [SerializeField, Header("目的地のインデックス"), ReadOnly]
    int targetIndex = 0;
    [SerializeField, Header("次の目的地"), ReadOnly]
    Vector3 nextTargetPos;

    void Start()
    {
        base.Start();
        patrol = GetComponent<Patrol>();
        targetNum = patrol.GetTargets().Length;
        nextTargetPos = patrol.GetTargets()[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

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
        angle = Mathf.Lerp(0.0f, angle, cnt / waitInterval) * 0.1f;
        transform.Rotate(0.0f, angle, 0.0f);

        // 移動モードに移行
        if (cnt > waitInterval)
        {
            cnt = 0.0f;
            state = State.Move;
            patrol.SetInitPosition(transform.position, moveSpeed, targetIndex);
        }
    }

    protected override void MoveFunc()
    {
        cnt += Time.deltaTime;
        currentPressure -= pressureForMove * Time.deltaTime;
        // 圧力回復モードに遷移
        if(currentPressure < pressureForHealMode)
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
            if(targetIndex >= targetNum)
            {
                targetIndex = 0;
            }
            nextTargetPos = patrol.GetTargets()[targetIndex].position;
        }
    }

    protected override void TrackingFunc()
    {
        // 待機モードに移行
        if(!FindPlayerAtFOV().isFind)
        {
            state = State.Idle;
        }
        else
        {
            transform.LookAt(target.transform.position);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * trackingSpeedRate);

            // 攻撃モードへの遷移
            Vector3 Diff = target.transform.position - transform.position;
            float dis2 = Diff.x * Diff.x + Diff.y * Diff.y + Diff.z * Diff.z;
            float range1 = distanceForCloseAttack * distanceForCloseAttack;
            float range2 = distanceForRangedAttack * distanceForRangedAttack;

            // 遠距離攻撃に重きを置く
            if (dis2 < range2 && currentPressure > lineForRagnedAttack)
            {
                Debug.Log("遠距離攻撃モードに移行");
                currentPressure -= pressureForRangedAttack;
                // 遠距離攻撃を行うベクトル
                Diff.Normalize();
                GameObject bullet = Instantiate(objectForRangedAttack, transform.position + transform.forward * 1.5f, Quaternion.identity);
                bullet.GetComponent<Rigidbody>().AddForce(Diff * rangedAttackSpeed, ForceMode.Impulse);

                state = State.AttackB;
                return;
            }
            else if (dis2 < range1 && currentPressure > lineForCloseAttack)
            {
                Debug.Log("近距離攻撃モードに移行");
                currentPressure -= pressureForCloseAttack;
                state = State.AttackA;
                return;
            }
        }
    }

    protected override void EscapeFunc()
    {
        throw new System.NotImplementedException();
    }

    protected override void AttackFunc1()
    {
        Debug.Log("近距離攻撃");
    }

    protected override void AttackFunc2()
    {
        cnt += Time.deltaTime;
        // アニメーション終了待ち

        if(cnt >= rangedInterval)
        {
            // 待機を行うか
            state = State.Idle;
            cnt = 0.0f;
            // 再度遠距離攻撃か

            // 近距離攻撃か

            // 回復か
        }
    }

    protected override void SpecialFuncA()
    {
        throw new System.NotImplementedException();
    }

    protected override void SpecialFuncB()
    {
        throw new System.NotImplementedException();
    }

    protected override void HealFunc()
    {
        cnt += Time.deltaTime;
        currentPressure += pressurePerHeal * Time.deltaTime;
        // 待機モードに移行
        if(currentPressure > pressureForStoppedHeal)
        {
            cnt = 0.0f;
            state = State.Idle;
        }
    }

    protected override void DeathFunc()
    {
        throw new System.NotImplementedException();
    }

    protected override void DestroyFunc()
    {
        base.DestroyFunc();
    }
}
