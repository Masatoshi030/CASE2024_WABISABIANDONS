using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Damaged : EnemyState
{
    [SerializeField, Header("d¼Ô")]
    float stiffnessTime;
    bool bUpdate = true;

    [Space(pad), Header("--JÚæXg--")]
    [SerializeField, Header("oßãÌJÚ")]
    string elapsedTransition = "Ò@";
    [SerializeField, Header("íeÌJÚ")]
    string damagedTransition = "íe";
    [SerializeField, Header("HPª0ÌJÚ")]
    string noHitPointTransition = "S";

    public override void Enter()
    {
        base.Enter();
        if(enemy.Hp <= 0)
        {
            machine.TransitionTo(noHitPointTransition);
            bUpdate = false;
        }
        enemy.IsInvincible = true;
    }

    public override void MainFunc()
    {
        if(bUpdate) if (machine.Cnt >= stiffnessTime) machine.TransitionTo(elapsedTransition);
    }

    public override void Exit()
    {
        
    }

    public override void CollisionEnterOpponent(Collision collision)
    {
        if (collision.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedTransition);
        }
    }

    public override void TriggerEnterOpponent(Collider other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedTransition);
        }
    }
}
