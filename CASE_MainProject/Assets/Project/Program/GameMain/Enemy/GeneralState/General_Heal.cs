using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Heal : EnemyState
{
    [SerializeField, Header("�b�ԉ񕜗�")]
    float healAmount;
    [SerializeField, Header("�ʏ�J�ڈ��͗�")]
    float transitionAmount;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�ʏ�J��ID")]
    public int baseID;
    [SerializeField, Header("��e���̑J��ID")]
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
