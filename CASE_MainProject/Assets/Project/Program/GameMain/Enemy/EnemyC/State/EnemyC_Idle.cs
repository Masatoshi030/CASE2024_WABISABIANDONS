using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC_Idle : EnemyState_C
{
    [SerializeField, Header("‘Ò‹@ŽžŠÔ")]
    float waitInterval;

    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("ŽžŠÔŒo‰ßŒã‚Ì‘JˆÚID")]
    int elapsedID;
    [SerializeField, Header("”í’eŽž‚Ì‘JˆÚ")]
    int damagedID;

    public override void Enter()
    {
        base.Enter();
    }

    public override void MainFunc()
    {
        base.MainFunc();

        if (enemy.IsDamaged)
        {
            machine.TransitionTo(damagedID);
            return;
        }
        else if(machine.Cnt >= waitInterval)
        {
            machine.TransitionTo(elapsedID);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
