using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Heal : EnemyState
{
    [SerializeField, Header("•bŠÔ‰ñ•œ—Ê")]
    float healAmount;
    [SerializeField, Header("’Êí‘JˆÚˆ³—Í—Ê")]
    float transitionAmount;

    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("’Êí‘JˆÚID")]
    public int baseID;
    [SerializeField, Header("”í’eŽž‚Ì‘JˆÚID")]
    public int damagedID;

    public override void Enter()
    {
        base.Enter();
        machine.IsUpdate = true;
        enemy.CalcPressure(healAmount * Time.deltaTime);
    }

    public override void MainFunc()
    {
        base.MainFunc();
        enemy.CalcPressure(healAmount * Time.deltaTime);

        if(enemy.Pressre >= transitionAmount)
        {
            machine.TransitionTo(baseID);
        }
        else if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedID);
        }
    }

    public override void Exit()
    {
        base.Exit();
        machine.IsUpdate = true;
    }
}
