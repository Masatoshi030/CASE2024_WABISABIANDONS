using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_A_Move : EnemyState
{
    [SerializeField, Header("移動速度")]
    float moveSpeed;
    [SerializeField, Header("加速度")]
    float accelerationSpeed;
    [SerializeField, Header("回転速度")]
    float angularSpeed;

    NavMeshPatrol patrol;
    int targetNum;
    int targetIdx;

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("完了時の遷移")]
    StateKey reachedKey;
    [SerializeField, Header("発見時の遷移")]
    StateKey findingKey;
    [SerializeField, Header("ダメージ時の遷移")]
    StateKey damagedKey;

    public override void Initialize()
    {
        base.Initialize();

        // パトロール用コンポーネントを取得
        patrol = enemy.GetComponent<NavMeshPatrol>();
        targetNum = patrol.GetTargets().Length;
        targetIdx = 0;
    }

    public override void Enter()
    {
        base.Enter();

        // エージェント自体のenabled
        patrol.Agent.enabled = true;
        patrol.enabled = true;
        patrol.Agent.velocity = Vector3.zero;
        // パラメータの設定
        patrol.SetAgentParam(moveSpeed, accelerationSpeed, angularSpeed);
        // パスの設定
        patrol.ExcutePatrol(targetIdx);
        enemy.IsVelocityZero = true;
    }

    public override void MainFunc()
    {
        base.MainFunc();

        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedKey);
        }
        else if(enemy.IsFindPlayer)
        {
            machine.TransitionTo(findingKey);
        }
        else if(patrol.GetPatrolState() == NavMeshPatrol.PatrolState.Idle)
        {
            machine.TransitionTo(reachedKey);
        }
    }

    public override void Exit()
    {
        base.Exit();
        // パトロールの終了
        patrol.Stop();
        patrol.Agent.enabled = false;
        // インデックスだけ進める
        targetIdx++;
        if (targetIdx >= targetNum)
        {
            targetIdx = 0;
        }
    }
}
