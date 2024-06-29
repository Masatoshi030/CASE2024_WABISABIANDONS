using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC_IntervalDeath : EnemyState
{

    [SerializeField, Header("待機時間")]
    public float waitInterval;
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void MainFunc()
    {
        base.MainFunc();

        if(machine.Cnt >= waitInterval)
        {
            // Cの死亡メッセージを通知(スポナーにビリヤードのスポーン依頼)
            enemy.SendMsg<int>(1, 0);
            Enemy_Manager.instance.AddDefeatEnemy();

            enemy.DestroyAllow();
            Destroy(enemy.gameObject);
            Destroy(gameObject);
        }
    }
}
