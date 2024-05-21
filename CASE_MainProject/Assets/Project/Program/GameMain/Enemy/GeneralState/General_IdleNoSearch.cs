using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_IdleNoSearch : EnemyState
{
    [SerializeField, Header("‘Ò‹@ŽžŠÔ")]
    float waitInterval;
    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("ŽžŠÔŒo‰ßŒã‚Ì‘JˆÚ")]
    public string elapsedTransition = "ˆÚ“®";
    [SerializeField, Header("”í’eŽž‚Ì‘JˆÚ")]
    public string damagedTransition = "”í’e";

    public override void Enter()
    {
        base.Enter();
        enemy.IsSearchPlayer = false;
    }

    public override void MainFunc()
    {
        if (enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedTransition);
            return;
        }
        else if (machine.Cnt >= waitInterval)
        {
            machine.TransitionTo(elapsedTransition);
        }
    }

    public override void Exit()
    {
        enemy.IsSearchPlayer = true;
    }
}
