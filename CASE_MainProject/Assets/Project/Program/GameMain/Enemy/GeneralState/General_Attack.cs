using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Attack : EnemyState
{
    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("���Ԍo�ߌ�̑J��")]
    int elapsedID;

    [SerializeField, Header("��e���̑J��")]
    int damegedID;


    public override void Enter()
    {
        base.Enter();
    }

    public override void MainFunc()
    {
        base.MainFunc();
        if (!machine.IsUpdate) return;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
