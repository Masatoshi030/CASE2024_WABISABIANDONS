using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC_IntervalDeath : EnemyState
{

    [SerializeField, Header("�ҋ@����")]
    public float waitInterval;
    public override void Initialize()
    {
        StateName = "���S�ҋ@";
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
