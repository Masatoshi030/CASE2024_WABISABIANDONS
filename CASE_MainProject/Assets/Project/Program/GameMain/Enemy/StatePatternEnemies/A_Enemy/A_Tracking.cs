using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Tracking : EnemyState
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
    [SerializeField, Header("’ÇÕ¸”s‚Ì‘JˆÚæ")]
    public string failedTransition = "‘Ò‹@";
    [SerializeField, Header("’ÇÕ¬Œ÷‚Ì‘JˆÚæ")]
    public string successfulTransition = "UŒ‚";


    public override void Initialize()
    {
        patrol = enemy.GetComponent<NavMeshPatrol>();
    }

    public override void Enter()
    {
        patrol.SetAgentParam(moveSpeed, acceleration, angularSpeed);
        patrol.ExcuteCustom(Enemy.Target.transform.position);
    }

    public override void MainFunc()
    {
        patrol.ExcuteCustom(Enemy.Target.transform.position);
        if (machine.Cnt >= trackingInterval)
        {
            Machine.TransitionTo(failedTransition);
        }
    }

    public override void Exit()
    {
        
    }
}
