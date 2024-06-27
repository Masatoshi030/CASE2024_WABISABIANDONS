using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Gen_Damage : EnemyState
{
    [SerializeField, Header("�d������")]
    float stiffnessTime = 3.0f;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("HP0���̑J��")]
    StateKey hp0Key;
    [SerializeField, Header("�_���[�W���̑J��")]
    StateKey damagedKey;
    [SerializeField, Header("�o�ߎ��̑J��")]
    StateKey elapsedKey;

    public override void Enter()
    {
        base.Enter();
        enemy.IsDamaged = false;
        // �_���[�W���󂯂��Ƃ����ʒm
        enemy.SendMsg<int>(0, 0);
        if(enemy.Hp <= 0)
        {
            machine.TransitionTo(hp0Key);
        }
    }

    public override void MainFunc()
    {
        base.MainFunc();
        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedKey);
        }
        else if(machine.Cnt >= stiffnessTime)
        {
            machine.TransitionTo(elapsedKey);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
