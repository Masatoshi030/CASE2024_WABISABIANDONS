using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC_Move : EnemyState_C
{
    [SerializeField, Header("移動速度")]
    float moveSpeed;
    [SerializeField, Header("加速度")]
    float acceleration;
    [SerializeField, Header("回転速度")]
    float angularSpeed;


    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("経過後の遷移")]
    public int elapsedID;
    [SerializeField, Header("被弾時の遷移")]
    public int damagedID;

    [SerializeField, Header("パトロール"), ReadOnly]
    NavMeshPatrol patrol;
    [SerializeField, Header("目的地の数"), ReadOnly]
    int targetNum;
    [SerializeField, Header("目的地のIndex"), ReadOnly]
    int targetIdx;

    public override void Initialize()
    {
        base.Initialize();

        patrol = enemy.GetComponent<NavMeshPatrol>();
        targetNum = patrol.GetTargets().Length;
        targetIdx = 0;
    }

    public override void Enter()
    {
        base.Enter();

        // 移動処理
        patrol.enabled = true;
        patrol.Agent.velocity = Vector3.zero;
        patrol.SetAgentParam(moveSpeed, acceleration, angularSpeed);
        patrol.ExcutePatrol(targetIdx);
        enemy.IsVelocityZero = true;
    }

    public override void MainFunc()
    {
        base.MainFunc();
        if (!machine.IsUpdate) return;

        if (enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedID);
        }
        else if (patrol.GetPatrolState() == NavMeshPatrol.PatrolState.Idle)
        {
            Machine.TransitionTo(elapsedID);
        }
    }

    public override void Exit()
    {
        base.Exit();

        patrol.Stop();
        targetIdx++;
        if (targetIdx >= targetNum)
        {
            targetIdx = 0;
        }
    }
}
