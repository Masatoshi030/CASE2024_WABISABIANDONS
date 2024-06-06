using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC_Idle : EnemyState_C
{
    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("”í’e‚Ì‘JˆÚ")]
    int damagedID;
    public override void Enter()
    {
        base.Enter();
    }

    public override void MainFunc()
    {
        base.MainFunc();

        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedID);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
