using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC_Caught : EnemyState
{
    [SerializeField, Header("Šª‚«‚İ’†‚©"), ReadOnly]
    bool bCaught = false;
    public bool Caught { get => bCaught; set => bCaught = value; }

    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("ó‘ÔI—¹‚Ì‘JˆÚ")]
    string elapesedTransition = "‘Ò‹@";
    [SerializeField, Header("”í’e‚Ì‘JˆÚ")]
    string damagedTransition = "”í’e";

    public override void Initialize()
    {
        StateName = "Šª‚«‚İ";
    }

    public override void Enter()
    {
        
    }

    public override void MainFunc()
    {
        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedTransition);
            return;
        }

        if(!bCaught)
        {
            machine.TransitionTo(elapesedTransition);
        }
    }

    public override void Exit()
    {
        bCaught = false;
    }
}
