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
    int elapesedID;
    [SerializeField, Header("”í’e‚Ì‘JˆÚ")]
    int damagedID;

    public override void Enter()
    {
        
    }

    public override void MainFunc()
    {
        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedID);
            return;
        }

        if(!bCaught)
        {
            machine.TransitionTo(elapesedID);
        }
    }

    public override void Exit()
    {
        bCaught = false;
    }
}
