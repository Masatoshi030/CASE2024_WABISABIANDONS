using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Idle : EnemyState
{
    [SerializeField, Header("待機時間")]
    float waitInterval = 3.0f;
    [SerializeField, Header("経過後の遷移先")]
    public string elapsedTransition = "移動";
    [SerializeField, Header("索敵成功時の遷移先")]
    public string serchSuccessTransition = "追跡";

    public override void Enter()
    {
        Debug.Log("待機開始" + enemy.name);
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
        Debug.Log("待機終了" + enemy.name);
    }
}
