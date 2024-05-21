using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_MoveTarget : EnemyState
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
    string searchSuccessTransition = "’ÇÕ";
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
        enemy.EnemyRigidbody.velocity = Vector3.zero;
    }

    public override void MainFunc()
    {
        if(enemy.IsFindPlayer)
        {
            Machine.TransitionTo(searchSuccessTransition);
            return;
        }
        else if(enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedTransition);
        }
        if (patrol.GetPatrolState() == NavMeshPatrol.PatrolState.Idle)
        {          
             Machine.TransitionTo(elapsedTransition);
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

    public override void CollisionEnterSelf(GameObject other)
    {
        if (other.name == "Player")
        {
            machine.TransitionTo(collisionTransition);
        }
    }
    public override void TriggerEnterSelf(GameObject other)
    {
        if (other.name == "Player")
        {
            machine.TransitionTo(collisionTransition);
        }
    }
}
