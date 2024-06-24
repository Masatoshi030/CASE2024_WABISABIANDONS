using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupManager_EnemyA : Publisher
{
    public override void ReceiveMsg<T>(Connection sender, int msgType, T msg)
    {
        // ���S�ʒm���擾
        if (msgType == 0)
        {
            if (GetComponent<Enemy>() != null)
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
