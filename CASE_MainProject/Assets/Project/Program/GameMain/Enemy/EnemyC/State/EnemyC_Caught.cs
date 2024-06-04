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
    string elapesedTransition = "�ҋ@";
    [SerializeField, Header("��e���̑J��")]
    string damagedTransition = "��e";

    public override void Initialize()
    {
        StateName = "��������";
    }

    public override void Enter()
    {
        
    }

    public override void MainFunc()
    {
        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedTransition);
            return;
        }

        if(!bCaught)
        {
            machine.TransitionTo(elapesedTransition);
        }
    }

    public override void Exit()
    {
        bCaught = false;
    }
}
