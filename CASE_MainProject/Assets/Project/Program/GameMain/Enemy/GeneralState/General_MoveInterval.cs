using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_MoveInterval : EnemyState
{
    [SerializeField, Header("移動速度")]
    float moveSpeed;
    [SerializeField, Header("加速度")]
    float acceleration;
    [SerializeField, Header("回転速度")]
    float angularSpeed;
    [SerializeField, Header("移動時間")]
    float moveInterval;

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("経過後の遷移先")]
    public int elapsedID;
    [SerializeField, Header("索敵成功時の遷移先")]
    public int serchSuccessID;
    [SerializeField, Header("被弾時の遷移")]
    public int damagedID;
    [SerializeField, Header("衝突時の遷移")]
    public int collisionID;

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
        base.Enter();
        // 移動処理
        patrol.SetAgentParam(moveSpeed, acceleration, angularSpeed);
        patrol.ExcutePatrol(targetIdx);
        enemy.EnemyRigidbody.velocity = Vector3.zero;
    }

    public override void MainFunc()
    {
        if(enemy.IsFindPlayer)
        {
            machine.TransitionTo(serchSuccessID);
            return;
        }
        else if(enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedID);
            return;
        }
        else if(machine.Cnt >= moveInterval)
        {
            patrol.Stop();
            machine.TransitionTo(elapsedID);
        }
    }

    public override void Exit()
    {
        patrol.Stop();
        targetIdx++;
        if (targetIdx >= targetNum)
        {
            targetIdx = 0;
        }
    }
}
