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
    int elapsedID;

    [SerializeField, Header("��e���̑J��")]
    int damegedID;


    public override void Enter()
    {
        PlayerController.instance.Damage(attackPower);
    }

    public override void MainFunc()
    {
        if(machine.Cnt >= waitInterval)
        {
            machine.TransitionTo(elapsedID);
        }
        else if(enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damegedID);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
