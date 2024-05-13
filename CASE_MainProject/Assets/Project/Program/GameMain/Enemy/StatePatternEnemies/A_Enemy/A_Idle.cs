using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Idle : EnemyState
{
    [SerializeField, Header("�ҋ@����")]
    float waitInterval = 3.0f;
    [SerializeField, Header("�o�ߌ�̑J�ڐ�")]
    public string elapsedTransition = "�ړ�";
    [SerializeField, Header("���G�������̑J�ڐ�")]
    public string serchSuccessTransition = "�ǐ�";

    public override void Enter()
    {
        Debug.Log("�ҋ@�J�n" + enemy.name);
    }

    public override void MainFunc()
    {
        if(enemy.IsFindPlayer)
        {
            Machine.TransitionTo(serchSuccessTransition);
        }

        if(machine.Cnt >= waitInterval)
        {
            Machine.TransitionTo(elapsedTransition);
        }
    }

    public override void Exit()
    {
        Debug.Log("�ҋ@�I��" + enemy.name);
    }
}
