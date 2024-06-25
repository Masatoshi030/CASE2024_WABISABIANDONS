using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupManager_EnemyA : GroupManager
{
    public override void ReceiveMsg<T>(Connection sender, int msgType, T msg)
    {
        // Ž€–S’Ê’m‚ðŽæ“¾
        if (msgType == 0)
        {
            if (isAttachedEnemy)
            {
                Enemy enemy = GetComponent<Enemy>();
                if (enemy.Machine.StateObject.GetComponent<EnemyA_Tracking>() != null)
                {
                    EnemyA_Tracking state = enemy.Machine.StateObject.GetComponent<EnemyA_Tracking>();
                    enemy.Machine.TransitionTo(state.StateID);
                }
            }
            for (int i = 0; i < subscribers.Count; i++)
            {
                if (subscribers[i] != null && subscribers[i] != sender)
                {
                    Enemy enemy = (Enemy)subscribers[i];
                    if(enemy.Machine.StateObject.GetComponent<EnemyA_Tracking>()!= null)
                    {
                        EnemyA_Tracking state = enemy.Machine.StateObject.GetComponent<EnemyA_Tracking>();
                        enemy.Machine.TransitionTo(state.StateID);
                    }
                }
            }
        }
    }
}
