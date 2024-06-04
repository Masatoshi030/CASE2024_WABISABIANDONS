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
    int elapsedID;
    [SerializeField, Header("íeÌJÚ")]
    int damagedID;
    [SerializeField, Header("HPª0ÌJÚ")]
    int noHitPointID;

    public override void Enter()
    {
        base.Enter();
        if(enemy.Hp <= 0)
        {
            machine.TransitionTo(noHitPointID);
            bUpdate = false;
        }
        enemy.IsInvincible = true;
    }

    public override void MainFunc()
    {
        if(bUpdate) if (machine.Cnt >= stiffnessTime) machine.TransitionTo(elapsedID);
    }

    public override void Exit()
    {
        
    }

    public override void CollisionEnterOpponent(Collision collision)
    {
        if (collision.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedID);
        }
    }

    public override void TriggerEnterOpponent(Collider other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedID);
        }
    }
}
