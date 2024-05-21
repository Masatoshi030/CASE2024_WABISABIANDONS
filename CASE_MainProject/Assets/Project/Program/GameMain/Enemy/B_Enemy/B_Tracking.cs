using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Tracking : EnemyState
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
    [SerializeField, Header("UŒ‚‘JˆÚ‹——£")]
    float attackDistance;

    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("’ÇÕ¸”s‚Ì‘JˆÚ")]
    public string failedTransition = "‘Ò‹@";
    [SerializeField, Header("‹——£ˆê’èˆÈ“à‚Ì‘JˆÚ")]
    public string successfulTransition = "UŒ‚";
    [SerializeField, Header("”í’e‚Ì‘JˆÚ")]
    public string damagedTransition = "”í’e";
    [SerializeField, Header("Õ“Ë‚Ì‘JˆÚ")]
    public string collisionTransition = "‘Ò‹@";

    public override void Initialize()
    {
        patrol = enemy.GetComponent<NavMeshPatrol>();
    }

    public override void Enter()
    {
        base.Enter();
        patrol.ExcuteCustom(Enemy.Target.transform.position);
        enemy.EnemyRigidbody.velocity = Vector3.zero;
    }

    public override void MainFunc()
    {
        if(machine.Cnt>= trackingInterval)
        {
            machine.TransitionTo(failedTransition);
            return;
        }
        if(enemy.IsFindPlayer)
        {
            patrol.ExcuteCustom(Enemy.Target.transform.position);
            if(enemy.ToPlayerDistace <= attackDistance * attackDistance)
            {
                machine.TransitionTo(successfulTransition);
            }
        }
        else
        {
            // ¸”s‚Æ‚İ‚È‚·
            machine.TransitionTo(failedTransition);
        }
    }

    public override void Exit()
    {
        patrol.Stop();
    }

    public override void CollisionEnterOpponent(GameObject other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedTransition);
            else
                machine.TransitionTo(collisionTransition);
        }
    }

    public override void TriggerEnterOpponent(GameObject other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedTransition);
            else
                machine.TransitionTo(collisionTransition);
        }
    }
}
