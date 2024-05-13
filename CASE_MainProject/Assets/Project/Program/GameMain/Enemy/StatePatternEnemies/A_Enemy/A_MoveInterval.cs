using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_MoveInterval : EnemyState
{
    [SerializeField, Header("移動速度")]
    float moveSpeed;
    [SerializeField, Header("加速度")]
    float acceleration;
    [SerializeField, Header("回転速度")]
    float angularSpeed;
    [SerializeField, Header("移動時間")]
    float moveInterval;
    [SerializeField, Header("経過後の遷移先")]
    string elapsedTransition = "待機";
    [SerializeField, Header("索敵成功時の遷移先")]
    string serchSuccessTransition = "追跡";

    [SerializeField, Header("パトロール"), ReadOnly]
    NavMeshPatrol patrol;
    [SerializeField, Header("目的地の数"), ReadOnly]
    int targetNum;
    [SerializeField, Header("目的地のIndex"), ReadOnly]
    int targetIdx;

    public override void Initialize()
    {
        patrol = enemy.GetComponent<NavMeshPatrol>();
        targetNum = patrol.GetTargets().Length;
        targetIdx = 0;
    }

    public override void Enter()
    {
        // 移動処理
        patrol.SetAgentParam(moveSpeed, acceleration, angularSpeed);
        patrol.ExcutePatrol(targetIdx);
    }

    public override void MainFunc()
    {
        if(enemy.IsFindPlayer)
        {
            patrol.Stop();
            Machine.TransitionTo(serchSuccessTransition);
        }
        if(machine.Cnt >= moveInterval)
        {
            patrol.Stop();
            Machine.TransitionTo(elapsedTransition);
        }
    }

    public override void Exit()
    {
        targetIdx++;
        if (targetIdx >= targetNum)
        {
            targetIdx = 0;
        }
    }
}
