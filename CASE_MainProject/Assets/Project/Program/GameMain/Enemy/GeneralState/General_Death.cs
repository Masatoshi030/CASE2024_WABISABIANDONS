using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Death : EnemyState
{
    public override void Enter()
    {
        base.Enter();
        enemy.EnemyRigidbody.velocity = Vector3.zero;
        enemy.SendMsg<int>(0, 0);
        Destroy(enemy.gameObject);
        Destroy(gameObject);
    }
}
