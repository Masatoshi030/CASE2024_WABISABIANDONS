using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Attack : EnemyState
{
    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("ŠÔŒo‰ßŒã‚Ì‘JˆÚ")]
    int elapsedID;

    [SerializeField, Header("”í’e‚Ì‘JˆÚ")]
    int damegedID;


    public override void Enter()
    {
        base.Enter();
    }

    public override void MainFunc()
    {
        base.MainFunc();
        if (!machine.IsUpdate) return;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
