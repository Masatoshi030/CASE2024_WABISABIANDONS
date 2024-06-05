using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC_IntervalDeath : EnemyState
{

    [SerializeField, Header("‘Ò‹@ŽžŠÔ")]
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
            Destroy(enemy.gameObject);
        }
    }
}
