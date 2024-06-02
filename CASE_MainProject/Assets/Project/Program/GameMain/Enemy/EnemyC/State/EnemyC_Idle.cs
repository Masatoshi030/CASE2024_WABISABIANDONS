using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC_Idle : EnemyState_C
{
    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("��e���̑J��")]
    string damagedTransition;
    public override void Enter()
    {
        base.Enter();
    }

    public override void MainFunc()
    {
        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedTransition);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
