using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Attack : EnemyState
{
    [SerializeField, Header("UŒ‚—Í")]
    float attackPower = 5.0f;

    [SerializeField, Header("‘Ò‹@ƒCƒ“ƒ^[ƒoƒ‹")]
    float waitInterval = 2.0f;

    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("ŽžŠÔŒo‰ßŒã‚Ì‘JˆÚ")]
    string elapsedTransition = "ˆÚ“®";

    [SerializeField, Header("”í’eŽž‚Ì‘JˆÚ")]
    string damegedTransition = "”í’e";


    public override void Enter()
    {
        PlayerController.instance.Damage(attackPower);
    }

    public override void MainFunc()
    {
        if(machine.Cnt >= waitInterval)
        {
            machine.TransitionTo(elapsedTransition);
        }
        else if(enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damegedTransition);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
