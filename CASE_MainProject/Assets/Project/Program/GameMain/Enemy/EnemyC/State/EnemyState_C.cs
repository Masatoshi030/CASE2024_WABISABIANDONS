using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_C : EnemyState
{
    protected EnemyC enemyC;

    public override void Initialize()
    {
        enemyC = enemy.gameObject.GetComponent<EnemyC>();
    }
}
