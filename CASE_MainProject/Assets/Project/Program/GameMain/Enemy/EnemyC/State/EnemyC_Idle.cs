using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC_Idle : EnemyState_C
{
    [SerializeField, Header("�ҋ@����")]
    float waitInterval;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("���Ԍo�ߌ�̑J��ID")]
    int elapsedID;
    [SerializeField, Header("��e���̑J��")]
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
