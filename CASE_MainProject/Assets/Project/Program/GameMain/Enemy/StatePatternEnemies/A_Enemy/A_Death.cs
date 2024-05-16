using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Death : EnemyState
{
    public override void Enter()
    {
        base.Enter();
        Destroy(enemy.gameObject);
        Destroy(gameObject);
    }
}
