using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyA_Tracking : EnemyState
{
    [SerializeField, Header("ƒpƒgƒ[ƒ‹"), ReadOnly]
    NavMeshPatrol patrol;
    [SerializeField, Header("ˆÚ“®‘¬“x")]
    float moveSpeed;
    [SerializeField, Header("‰Á‘¬“x")]
    float acceleration;
    [SerializeField, Header("‰ñ“]‘¬“x")]
    float angularSpeed;
    [SerializeField, Header("’ÇÕŠÔ")]
    float trackingInterval;
    [SerializeField, Header("ˆÚ“®‹——£")]
    float moveDistance;
    [SerializeField, Header("UŒ‚—Í")]
    float attackPower = 5.0f;


    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("’ÇÕ¸”s‚Ì‘JˆÚ")]
    public int failedID;
    [SerializeField, Header("”í’e‚Ì‘JˆÚ")]
    public int damagedID;
    [SerializeField, Header("Õ“Ë‚Ì‘JˆÚ")]
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
        patrol.Agent.enabled = true;
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
        if (!continueProcessing) return;

        if (machine.Cnt >= trackingInterval)
        {
            machine.TransitionTo(failedID);
            return;
        }
        else if (enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedID);
            return;
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
        patrol.Agent.enabled = false;
    }

    public override void CollisionEnterSelf(Collision collision)
    {
        if (collision.transform.name == "Player" && !enemy.IsDamaged)
        {
            PlayerController.instance.Damage(attackPower);
            machine.TransitionTo(collisionID);
        }
    }

    public override void TriggerEnterSelf(Collider other)
    {
        if (other.transform.name == "Player" && !enemy.IsDamaged)
        {
            PlayerController.instance.Damage(attackPower);
            machine.TransitionTo(collisionID);
        }
    }
}
