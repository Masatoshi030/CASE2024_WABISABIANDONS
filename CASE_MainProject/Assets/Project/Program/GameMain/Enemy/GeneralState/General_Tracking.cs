using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Tracking : EnemyState
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

    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("’ÇÕ¸”s‚Ì‘JˆÚ")]
    public string failedTransition = "‘Ò‹@";
    [SerializeField, Header("”í’e‚Ì‘JˆÚ")]
    public string damagedTransition = "”í’e";
    [SerializeField, Header("Õ“Ë‚Ì‘JˆÚ")]
    public string collisionTransition = "UŒ‚";

    public override void Initialize()
    {
        patrol = enemy.GetComponent<NavMeshPatrol>();
    }

    public override void Enter()
    {
        base.Enter();
        patrol.SetAgentParam(moveSpeed, acceleration, angularSpeed);
        Vector3 Direction = Enemy.Target.transform.position - enemy.gameObject.transform.position;
        Direction.Normalize();
        Direction *= moveDistance;
        patrol.ExcuteCustom(enemy.gameObject.transform.position + Direction);
    }

    public override void MainFunc()
    {
        if (machine.Cnt >= trackingInterval)
        {
            machine.TransitionTo(failedTransition);
        }
        else if(enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedTransition);
        }
        Vector3 Direction = Enemy.Target.transform.position - enemy.gameObject.transform.position;
        Direction.Normalize();
        Direction *= moveDistance;
        patrol.ExcuteCustom(enemy.gameObject.transform.position + Direction);
        
    }

    public override void Exit()
    {
        patrol.Stop();
    }

    public override void CollisionEnterSelf(Collision collision)
    {
        if (collision.transform.name == "Player" && !enemy.IsDamaged)
        {
            Vector3 direction = collision.transform.position - transform.position;
            direction.Normalize();
            direction.y = 0.5f;
            PlayerController.instance.KnockBack(moveSpeed, direction);
            machine.TransitionTo(collisionTransition);
        }
    }
    public override void TriggerEnterSelf(Collider other)
    {
        if (other.name == "Player" && !enemy.IsDamaged)
        {
            Vector3 direction = other.transform.position - transform.position;
            direction.Normalize();
            direction.y = 0.5f;
            PlayerController.instance.KnockBack(moveSpeed, direction);
            machine.TransitionTo(collisionTransition);
        }
    }
}
