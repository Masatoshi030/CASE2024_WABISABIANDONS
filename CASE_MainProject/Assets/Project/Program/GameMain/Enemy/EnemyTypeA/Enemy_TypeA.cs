using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_TypeA : Enemy_Mob
{
    [Space(padA), Header("--メインパラメータ--")]
    [SerializeField, Header("汎用カウント"), ReadOnly]
    float cnt = 0.0f;
    [Space(padB), Header("-待機パラメータ-")]
    [SerializeField, Header("待機インターバル")]
    float waitInterval = 3.0f;
    [Space(padB),SerializeField, Header("-移動パラメータ-")]
    EnemyAgentParam moveParam;
    [Space(padB), SerializeField, Header("-追尾パラメータ-")]
    EnemyAgentParam trackingParam;
    [SerializeField, Header("追尾時に維持する距離")]
    float trackingKeepDistance;
    [Space(padB), SerializeField, Header("-逃亡パラメータ-")]
    EnemyAgentParam escapeParam;
    [SerializeField, Header("逃亡時間")]
    float escapeTime;
    [SerializeField, Header("逃亡距離")]
    float escapeDistance;
    [Space(padB), SerializeField, Header("-回復パラメータ-")]
    HealParam healParam;

    

    [Space(padA), Header("--パトロール関連--")]
    NavMeshPatrol patrol;
    [SerializeField, Header("目的地数"), ReadOnly]
    int targetNum;
    [SerializeField, Header("次の目的地の添え字"), ReadOnly]
    int targetIndex = 0;

    // アニメーター
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        // 各種コンポーネント取得
        patrol = GetComponent<NavMeshPatrol>();
        animator = GetComponent<Animator>();
        targetNum = patrol.GetTargets().Length; // 目的地数の取得
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void IdleFunc()
    {
        cnt += Time.deltaTime;
        if (FindPlayerAtFOV().isFind)
        {
            cnt = 0.0f;
            patrol.SetAgentParam(trackingParam, true);
            state = State.Tracking;
            return;
        }

        if (cnt > waitInterval)
        {
            cnt = 0.0f;
            patrol.SetAgentParam(moveParam);
            patrol.ExcutePatrol(targetIndex);
            state = State.Move;
        }
    }

    protected override void MoveFunc()
    {
        SubPressure(moveParam.consumePressure * Time.deltaTime);
        if(currentPressure <= healParam.startHealLine)
        {
            state = State.Heal;
            patrol.Stop();
            return;
        }

        if (FindPlayerAtFOV().isFind)
        {
            state = State.Tracking;
            patrol.SetAgentParam(trackingParam, true);
            return;
        }

        if (patrol.GetPatrolState() == NavMeshPatrol.PatrolState.Idle)
        {
            targetIndex++;
            if(targetIndex >= targetNum)
            {
                targetIndex = 0;
            }
            state = State.Idle;
        }
    }

    protected override void TrackingFunc()
    {
        // 移動距離がまだ残っているなら圧力を減少
        if(patrol.GetRemainingDistance() > 0.0f) SubPressure(trackingParam.consumePressure * Time.deltaTime);

        if (currentPressure <= healParam.startHealLine)
        {
            state = State.Heal;
            patrol.Stop();
            return;
        }

        (bool isFind, float distance) = FindPlayerAtFOV();
        if (!isFind)
        {
            state = State.Idle;
            return;
        }

        // プレイヤーがこちらを向いていた場合、逃亡に移行する
        if(Vector3.Dot(eyeTransform.forward, PlayerController.instance.transform.Find("HorizontalRotationShaft").Find("MoveShaft").transform.forward) < -0.75f)
        {
            state = State.Escape;
            patrol.Stop();
            patrol.SetAgentParam(escapeParam);
            return;
        }

        // 目的地の算出
        Vector3 targetVector = transform.position - target.transform.position;
        targetVector.Normalize();
        Vector3 Position = target.transform.position + targetVector * trackingKeepDistance;
        patrol.ExcuteCustom(Position);
    }

    protected override void EscapeFunc()
    {
        SubPressure(escapeParam.consumePressure * Time.deltaTime);
        cnt += Time.deltaTime;
        if(cnt > escapeTime)
        {
            cnt = 0.0f;
            state = State.Idle;
            return;
        }
        // 目的地の算出
        Vector3 targetVector = transform.position - target.transform.position;
        targetVector.Normalize();
        Vector3 Position = transform.position + targetVector * escapeDistance;
        patrol.ExcuteCustom(Position);
    }

    protected override void HealFunc()
    {
        base.HealFunc();
        if(currentPressure >= healParam.endHealLine)
        {
            state = State.Idle;
        }
    }

    protected override void DeathFunc()
    {
        Destroy(gameObject);
    }

    protected override void DestroyFunc()
    {
        animator.SetBool("bDeath", true);
    }
}
