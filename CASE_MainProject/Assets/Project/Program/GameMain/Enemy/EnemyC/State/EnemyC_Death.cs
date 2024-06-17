using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC_Death : EnemyState_C
{
    public override void Enter()
    {
        enemy.SendMsg<int>(0, 0);
        Destroy(enemy.gameObject);
        Destroy(gameObject);
    }
}
