using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupManager_EnemyA : GroupManager
{
    public override void ReceiveMsg<T>(Connection sender, int msgType, T msg)
    {
        // グループ内の敵への通知願いを取得
        if (msgType == 0)
        {
            if (isAttachedEnemy)
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
                }
            }
        }
    }
}
