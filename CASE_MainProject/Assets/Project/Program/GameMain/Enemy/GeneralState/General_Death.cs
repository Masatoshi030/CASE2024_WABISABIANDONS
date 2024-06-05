using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Death : EnemyState
{
    public override void Enter()
    {
        base.Enter();
        enemy.EnemyRigidbody.velocity = Vector3.zero;
        Destroy(enemy.gameObject);
        Destroy(gameObject);
    }
}
