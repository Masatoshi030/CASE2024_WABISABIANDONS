using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupManager_EnemyB : GroupManager
{
    public override void ReceiveMsg<T>(Connection sender, int msgType, T msg)
    {
        if (msgType == 0)
        {
            if (isAttachedEnemy)
            {
                Enemy enemy = GetComponent<Enemy>();
                if (enemy.Machine.StateObject.GetComponent<EnemyB_Attack>())
                {
                    EnemyB_Attack state = enemy.Machine.StateObject.GetComponent<EnemyB_Attack>();
                    enemy.Machine.TransitionTo(state.StateID);
                }
            }
            for (int i = 0; i < subscribers.Count; i++)
            {
                if (subscribers[i] != null && subscribers[i] != sender)
                {
                    Enemy enemy = (Enemy)subscribers[i];
                    if (enemy.Machine.StateObject.GetComponent<EnemyB_Attack>() != null)
                    {
                        EnemyB_Attack state = enemy.Machine.StateObject.GetComponent<EnemyB_Attack>();
                        enemy.Machine.TransitionTo(state.StateID);
                    }
                }
            }
        }
    }
}
