using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Gen_Idle : EnemyState
{
    [SerializeField, Header("�ҋ@����")]
    float waitTime = 3.0f;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�o�ߎ��̑J��")]
    StateKey elapsedKey;
    [SerializeField, Header("�������̑J��")]
    StateKey findingKey;
    [SerializeField, Header("�_���[�W���̑J��")]
    StateKey damagedKey;

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
            machine.TransitionTo(damagedKey);
        }
        else if(enemy.IsFindPlayer)
        {
            machine.TransitionTo(findingKey);
        }
        else if(machine.Cnt >= waitTime)
        {
            machine.TransitionTo(elapsedKey);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.IsVelocityZero = false;
    }
}
