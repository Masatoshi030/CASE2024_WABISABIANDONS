using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Death : EnemyState
{
    public override void Enter()
    {
        enemy.EnemyRigidbody.velocity = Vector3.zero;
        base.Enter();
        Destroy(enemy.gameObject);
        Destroy(gameObject);
    }
}
