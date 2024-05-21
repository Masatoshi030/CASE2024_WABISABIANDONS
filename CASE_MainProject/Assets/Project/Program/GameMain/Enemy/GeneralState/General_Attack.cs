using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Attack : EnemyState
{
    [SerializeField, Header("攻撃力")]
    float attackPower = 5.0f;

    [SerializeField, Header("待機インターバル")]
    float waitInterval = 2.0f;

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("時間経過後の遷移")]
    string elapsedTransition = "移動";

    [SerializeField, Header("被弾時の遷移")]
    string damegedTransition = "被弾";


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
