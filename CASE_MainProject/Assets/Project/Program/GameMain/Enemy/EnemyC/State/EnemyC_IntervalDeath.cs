using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC_IntervalDeath : EnemyState
{

    [SerializeField, Header("‘Ò‹@ŽžŠÔ")]
    public float waitInterval;
    public override void Initialize()
    {
        StateName = "Ž€–S‘Ò‹@";
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
