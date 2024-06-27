using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupManager_EnemyAB : GroupManager
{
    public override void ReceiveMsg<T>(Connection sender, int msgType, T msg)
    {
        if (msgType == 0)
        {
            if(isAttachedEnemy)
            {
                Enemy enemy = GetComponent<Enemy>();
                // 最新版エネミー用
                if (enemy.Machine.StateObject.GetComponent<State_A_Tracking>() != null)
                {
                    State_A_Tracking state = enemy.Machine.StateObject.GetComponent<State_A_Tracking>();
                    enemy.Machine.TransitionTo(state.StateID);
                }
                // 旧式エネミー用
                else if (enemy.Machine.StateObject.GetComponent<EnemyA_Tracking>() != null)
                {
                    EnemyA_Tracking state = enemy.Machine.StateObject.GetComponent<EnemyA_Tracking>();
                    enemy.Machine.TransitionTo(state.StateID);
                }
                // エネミーB用
                else if(enemy.Machine.StateObject.GetComponent<EnemyB_Attack>())
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
                    // 最新版エネミー用
                    if (enemy.Machine.StateObject.GetComponent<State_A_Tracking>() != null)
                    {
                        State_A_Tracking state = enemy.Machine.StateObject.GetComponent<State_A_Tracking>();
                        enemy.Machine.TransitionTo(state.StateID);
                    }
                    // 旧式エネミー用
                    else if (enemy.Machine.StateObject.GetComponent<EnemyA_Tracking>() != null)
                    {
                        EnemyA_Tracking state = enemy.Machine.StateObject.GetComponent<EnemyA_Tracking>();
                        enemy.Machine.TransitionTo(state.StateID);
                    }
                    else if(enemy.Machine.StateObject.GetComponent<EnemyB_Attack>() != null)
                    {
                        EnemyB_Attack state = enemy.Machine.StateObject.GetComponent<EnemyB_Attack>();
                        enemy.Machine.TransitionTo(state.StateID);
                    }
                }
            }
        }
    }
}
