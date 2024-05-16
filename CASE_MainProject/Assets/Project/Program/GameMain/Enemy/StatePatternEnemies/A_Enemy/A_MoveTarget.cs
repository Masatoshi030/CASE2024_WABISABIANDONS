using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_MoveTarget : EnemyState
{
    [SerializeField, Header("ˆÚ“®‘¬“x")]
    float moveSpeed;
    [SerializeField, Header("‰Á‘¬“x")]
    float acceleration;
    [SerializeField, Header("‰ñ“]‘¬“x")]
    float angularSpeed;
    

    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("Œo‰ßŒã‚Ì‘JˆÚ")]
    string elapsedTransition = "‘Ò‹@";
    [SerializeField, Header("õ“G¬Œ÷‚Ì‘JˆÚ")]
    string serchSuccessTransition = "’ÇÕ";
    [SerializeField, Header("”í’e‚Ì‘JˆÚ")]
    string damagedTransition = "”í’e";
    [SerializeField, Header("Õ“Ë‚Ì‘JˆÚ")]
    string collisionTransition = "’ÇÕ";
    

    [SerializeField, Header("ƒpƒgƒ[ƒ‹"), ReadOnly]
    NavMeshPatrol patrol;
    [SerializeField, Header("–Ú“I’n‚Ì”"), ReadOnly]
    int targetNum;
    [SerializeField, Header("–Ú“I’n‚ÌIndex"), ReadOnly]
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
        // ˆÚ“®ˆ—
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
