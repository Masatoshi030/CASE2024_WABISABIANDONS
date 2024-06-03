using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC_IntervalDeath : EnemyState
{

    [SerializeField, Header("待機時間")]
    public float waitInterval;
    public override void Initialize()
    {
        StateName = "死亡待機";
    }

    public override void Enter()
    {
        
    }

    public override void MainFunc()
    {
        if(machine.Cnt >= waitInterval)
        {
            Destroy(enemy.gameObject);
        }
    }
}
