using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Damaged : EnemyState
{
    [SerializeField, Header("d’¼ŠÔ")]
    float stiffnessTime;
    bool bUpdate = true;

    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("Œo‰ßŒã‚Ì‘JˆÚ")]
    string elapsedTransition = "‘Ò‹@";
    [SerializeField, Header("”í’e‚Ì‘JˆÚ")]
    string damagedTransition = "”í’e";
    [SerializeField, Header("HP‚ª0‚Ì‘JˆÚ")]
    string noHitPointTransition = "€–S";

    public override void Enter()
    {
        base.Enter();
        enemy.Damage(10.0f, Vector3.zero);
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

    public override void CollisionEnter(Collision collision)
    {
        if (collision.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedTransition);
        }
    }

    public override void TriggerEnter(Collider collider)
    {
        if (collider.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedTransition);
        }
    }
}
