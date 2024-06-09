using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC_Caught : EnemyState
{
    [SerializeField, Header("�������ݒ���"), ReadOnly]
    bool bCaught = false;
    public bool Caught { get => bCaught; set => bCaught = value; }

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("��ԏI�����̑J��")]
    int elapesedID;
    [SerializeField, Header("��e���̑J��")]
    int damagedID;

    public override void Enter()
    {
        base.Enter();

        enemy.IsVelocityZero = true;
    }

    public override void MainFunc()
    {
        base.MainFunc();

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
        base.Exit();

        bCaught = false;
    }
}
