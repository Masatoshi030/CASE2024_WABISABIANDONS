using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Heal : EnemyState
{
    [SerializeField, Header("bÔñÊ")]
    float healAmount;
    [SerializeField, Header("ÊíJÚ³ÍÊ")]
    float transitionAmount;

    [Space(pad), Header("--JÚæXg--")]
    [SerializeField, Header("ÊíJÚID")]
    public int baseID;
    [SerializeField, Header("íeÌJÚID")]
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
