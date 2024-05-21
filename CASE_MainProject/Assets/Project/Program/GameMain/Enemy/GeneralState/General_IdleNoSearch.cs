using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_IdleNoSearch : EnemyState
{
    [SerializeField, Header("�ҋ@����")]
    float waitInterval;
    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("���Ԍo�ߌ�̑J��")]
    public string elapsedTransition = "�ړ�";
    [SerializeField, Header("��e���̑J��")]
    public string damagedTransition = "��e";

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
