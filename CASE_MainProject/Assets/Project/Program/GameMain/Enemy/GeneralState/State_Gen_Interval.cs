using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class State_Gen_Interval : EnemyState
{
    [SerializeField, Header("‘Ò‚¿ŽžŠÔ")]
    float waitTime = 3.0f;

    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("Œo‰ßŽž‚Ì‘JˆÚ")]
    StateKey elapsedKey;
    [SerializeField, Header("ƒ_ƒ[ƒWŽž‚Ì‘JˆÚ")]
    StateKey damagedKey;

    public override void Enter()
    {
        base.Enter();

        enemy.IsVelocityZero = true;
        enemy.IsSearchPlayer = false;
    }

    public override void MainFunc()
    {
        base.MainFunc();

        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedKey);
        }
        else if(machine.Cnt >= waitTime)
        {
            machine.TransitionTo(elapsedKey);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.IsSearchPlayer = true;
    }
}
