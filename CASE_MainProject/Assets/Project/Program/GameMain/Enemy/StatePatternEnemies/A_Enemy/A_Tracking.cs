using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Tracking : EnemyState
{
    [SerializeField, Header("パトロール"), ReadOnly]
    NavMeshPatrol patrol;
    [SerializeField, Header("移動速度")]
    float moveSpeed;
    [SerializeField, Header("加速度")]
    float acceleration;
    [SerializeField, Header("回転速度")]
    float angularSpeed;
    [SerializeField, Header("追跡時間")]
    float trackingInterval;
    [SerializeField, Header("追跡失敗時の遷移先")]
    public string failedTransition = "待機";
    [SerializeField, Header("追跡成功時の遷移先")]
    public string successfulTransition = "攻撃";


    public override void Initialize()
    {
        patrol = enemy.GetComponent<NavMeshPatrol>();
    }

    public override void Enter()
    {
        patrol.SetAgentParam(moveSpeed, acceleration, angularSpeed);
        patrol.ExcuteCustom(Enemy.Target.transform.position);
    }

    public override void MainFunc()
    {
        patrol.ExcuteCustom(Enemy.Target.transform.position);
        if (machine.Cnt >= trackingInterval)
        {
            Machine.TransitionTo(failedTransition);
        }
    }

    public override void Exit()
    {
        
    }
}
