using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_TypeA : Enemy_Mob
{
    [Space(padA), Header("--メインパラメータ--")]
    [SerializeField, Header("汎用カウント"), ReadOnly]
    float cnt = 0.0f;
    [Space(padB), Header("-移動-")]
    [SerializeField, Header("最大移動速度")]
    float moveSpeed = 5.0f;
    [SerializeField, Header("加速度")]
    float moveAcceleration = 2.0f;
    [Space(padB), Header("-追尾-")]
    [SerializeField, Header("追尾時の速度")]
    float trackingSpeed = 8.0f;
    [SerializeField, Header("追尾時加速度")]
    float trackingAcceleration = 2.0f;
    [SerializeField, Header("維持する距離")]
    float trackingKeepDistance = 3.0f;
    [Space(padB), Header("-逃亡-")]
    [SerializeField, Header("逃亡時の速度")]
    float escapeSpeed = 8.0f;
    [SerializeField, Header("逃亡時加速度")]
    float escapeAcceleration = 2.0f;
    [SerializeField, Header("逃亡時間")]
    float escapeTime = 1.2f;
    [SerializeField, Header("逃亡距離")]
    float escapeDistance = 3.0f;
    

    [Space(padA), Header("--インターバル--")]
    [SerializeField, Header("待機インターバル")]
    float waitInterval = 3.0f;

    [Space(padA), Header("--パトロール関連--")]
    [SerializeField, Header("コンポーネント")]
    NavMeshPatrol patrol;
    [SerializeField, Header("目的地数"), ReadOnly]
    int targetNum;
    [SerializeField, Header("次の目的地の添え字"), ReadOnly]
    int targetIndex = 0;

    [SerializeField, Header("アニメーター")]
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        patrol = GetComponent<NavMeshPatrol>();
        animator = GetComponent<Animator>();
        targetNum = patrol.GetTargets().Length;
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
            patrol.SetAgentParam(trackingSpeed, trackingAcceleration, true);
            state = State.Tracking;
            return;
        }

        if (cnt > waitInterval)
        {
            cnt = 0.0f;
            patrol.SetAgentParam(moveSpeed, moveAcceleration);
            patrol.ExcutePatrol(targetIndex);
            Debug.Log("移動" + patrol.GetRemaingDistance());
            state = State.Move;
        }
    }

    protected override void MoveFunc()
    {
        if (FindPlayerAtFOV().isFind)
        {
            state = State.Tracking;
            patrol.SetAgentParam(trackingSpeed, trackingAcceleration, true);
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
        (bool isFind, float distance) = FindPlayerAtFOV();
        if (!isFind)
        {
            state = State.Idle;
            return;
        }

        if(Vector3.Dot(eyeTransform.forward, PlayerController.instance.transform.Find("HorizontalRotationShaft").Find("MoveShaft").transform.forward) < -0.75f)
        {
            state = State.Escape;
            patrol.SetAgentParam(escapeSpeed, escapeAcceleration);
            return;
        }

        // 目的地の算出
        Vector3 targetVector = transform.position - target.transform.position;
        targetVector.Normalize();
        Vector3 Position = target.transform.position + targetVector * trackingKeepDistance;
        patrol.ExcuteCustom(Position);
        Debug.Log("追尾" + patrol.GetRemaingDistance());
    }

    protected override void EscapeFunc()
    {
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

    protected override void DeathFunc()
    {
        Destroy(gameObject);
    }

    protected override void DestroyFunc()
    {
        animator.SetBool("bDeath", true);
    }
}
