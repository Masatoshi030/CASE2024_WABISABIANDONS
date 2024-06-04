using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_IdleNoSearch : EnemyState
{
    [SerializeField, Header("�ҋ@����")]
    float waitInterval;
    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("���Ԍo�ߌ�̑J��")]
    public int elapsedID;
    [SerializeField, Header("��e���̑J��")]
    public int damagedID;

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
            machine.TransitionTo(damagedID);
            return;
        }
        else if (machine.Cnt >= waitInterval)
        {
            machine.TransitionTo(elapsedID);
        }
    }

    public override void Exit()
    {
        enemy.IsSearchPlayer = true;
    }
}
