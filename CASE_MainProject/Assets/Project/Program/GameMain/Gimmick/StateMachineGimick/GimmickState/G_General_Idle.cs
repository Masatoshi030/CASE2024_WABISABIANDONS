using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_General_Idle : GimmickState
{
    [SerializeField, Header("�ҋ@����")]
    float waitInterval;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("���Ԍo�ߌ�̑J��")]
    string elapsedTransition = "�쓮";

    public override void Enter()
    {
        base.Enter();
    }

    public override void MainFunc()
    {
        if (machine.Cnt >= waitInterval) machine.TransitionTo(elapsedTransition);
    }
}
