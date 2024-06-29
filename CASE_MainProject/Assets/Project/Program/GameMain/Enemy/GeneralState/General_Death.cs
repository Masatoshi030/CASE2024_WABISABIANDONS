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
        if (enemy.IsDropValves)
        {
            DropValveManager.instance.CreateValves(enemy.DropValveNum, enemy.transform.position, enemy.IsAutoGet);
        }
        Enemy_Manager.instance.AddDefeatEnemy();
        enemy.DestroyAllow();
        Destroy(enemy.gameObject);
        Destroy(gameObject);
    }
}
