using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_MoveTarget : EnemyState
{
    [SerializeField, Header("移動速度")]
    float moveSpeed;
    [SerializeField, Header("加速度")]
    float acceleration;
    [SerializeField, Header("回転速度")]
    float angularSpeed;
    

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("経過後の遷移")]
    string elapsedTransition = "待機";
    [SerializeField, Header("索敵成功時の遷移")]
    string serchSuccessTransition = "追跡";
    [SerializeField, Header("被弾時の遷移")]
    string damagedTransition = "被弾";
    [SerializeField, Header("衝突時の遷移")]
    string collisionTransition = "追跡";
    

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
    }

    public override void MainFunc()
    {
        if(enemy.IsFindPlayer)
        {
            Machine.TransitionTo(serchSuccessTransition);
        }
        if (patrol.GetPatrolState() == NavMeshPatrol.PatrolState.Idle)
        {          
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

    public override void CollisionEnter(Collision collision)
    {
        if(collision.transform.root.name == "Player")
        {
            patrol.Stop();
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedTransition);
            else
                machine.TransitionTo(collisionTransition);
        }
    }

    public override void TriggerEnter(Collider collider)
    {
        if (collider.transform.root.name == "Player")
        {
            patrol.Stop();
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedTransition);
            else
                machine.TransitionTo(collisionTransition);
        }
    }
}
