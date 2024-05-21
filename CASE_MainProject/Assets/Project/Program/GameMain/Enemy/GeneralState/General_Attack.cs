using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Attack : EnemyState
{
    [SerializeField, Header("�U����")]
    float attackPower = 5.0f;

    [SerializeField, Header("�ҋ@�C���^�[�o��")]
    float waitInterval = 2.0f;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("���Ԍo�ߌ�̑J��")]
    string elapsedTransition = "�ړ�";

    [SerializeField, Header("��e���̑J��")]
    string damegedTransition = "��e";


    public override void Enter()
    {
        PlayerController.instance.Damage(attackPower);
    }

    public override void MainFunc()
    {
        if(machine.Cnt >= waitInterval)
        {
            machine.TransitionTo(elapsedTransition);
        }
        else if(enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damegedTransition);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
