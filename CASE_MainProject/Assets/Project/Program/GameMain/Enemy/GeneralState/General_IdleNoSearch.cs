using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_IdleNoSearch : EnemyState
{
    [SerializeField, Header("待機時間")]
    float waitInterval;
    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("時間経過後の遷移")]
    public string elapsedTransition = "移動";
    [SerializeField, Header("被弾時の遷移")]
    public string damagedTransition = "被弾";

    public override void Enter()
    {
        base.Enter();
        enemy.IsSearchPlayer = false;
    }

    public override void MainFunc()
    {
        if (enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedTransition);
            return;
        }
        else if (machine.Cnt >= waitInterval)
        {
            machine.TransitionTo(elapsedTransition);
        }
    }

    public override void Exit()
    {
        enemy.IsSearchPlayer = true;
    }
}
