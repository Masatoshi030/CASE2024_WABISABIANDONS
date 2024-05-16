using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Damaged : EnemyState
{
    [SerializeField, Header("d’¼ŠÔ")]
    float stiffnessTime;
    bool bUpdate = true;

    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("Œo‰ßŒã‚Ì‘JˆÚ")]
    string elapsedTransition = "‘Ò‹@";
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
}
