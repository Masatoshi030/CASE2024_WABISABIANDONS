using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Tracking : EnemyState
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
    [SerializeField, Header("移動距離")]
    float moveDistance;

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("追跡失敗時の遷移")]
    public int failedID;
    [SerializeField, Header("被弾時の遷移")]
    public int damagedID;
    [SerializeField, Header("衝突時の遷移")]
    public int collisionID;

    public override void Initialize()
    {
        base.Initialize();

        patrol = enemy.GetComponent<NavMeshPatrol>();
    }

    public override void Enter()
    {
        base.Enter();

        enemy.IsVelocityZero = true;
        patrol.enabled = true;
        patrol.Agent.velocity = Vector3.zero;
        patrol.SetAgentParam(moveSpeed, acceleration, angularSpeed);
        Vector3 Direction = Enemy.Target.transform.position - enemy.gameObject.transform.position;
        Direction.Normalize();
        Direction *= moveDistance;
        patrol.ExcuteCustom(enemy.gameObject.transform.position + Direction);
    }

    public override void MainFunc()
    {
        base.MainFunc();
        if (!machine.IsUpdate) return;

        if (machine.Cnt >= trackingInterval)
        {
            machine.TransitionTo(failedID);
        }
        else if(enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedID);
        }
        Vector3 Direction = Enemy.Target.transform.position - enemy.gameObject.transform.position;
        Direction.Normalize();
        Direction *= moveDistance;
        patrol.ExcuteCustom(enemy.gameObject.transform.position + Direction);
        
    }

    public override void Exit()
    {
        base.Exit();

        patrol.Stop();
    }

    public override void CollisionEnterSelf(Collision collision)
    {
        if (collision.transform.name == "Player" && !enemy.IsDamaged)
        {
            machine.TransitionTo(collisionID);
        }
    }
    public override void TriggerEnterSelf(Collider other)
    {
        if (other.transform.name == "Player" && !enemy.IsDamaged)
        {
            machine.TransitionTo(collisionID);
        }
    }
}
