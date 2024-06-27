using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupManager_EnemyA : GroupManager
{
    public override void ReceiveMsg<T>(Connection sender, int msgType, T msg)
    {
        // �O���[�v���̓G�ւ̒ʒm�肢���擾
        if (msgType == 0)
        {
            if (isAttachedEnemy)
            {
                Enemy enemy = GetComponent<Enemy>();
                // �ŐV�ŃG�l�~�[�p
                if (enemy.Machine.StateObject.GetComponent<State_A_Tracking>() != null)
                {
                    State_A_Tracking state = enemy.Machine.StateObject.GetComponent<State_A_Tracking>();
                    enemy.Machine.TransitionTo(state.StateID);
                }
                // �����G�l�~�[�p
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
                    // �ŐV�ŃG�l�~�[�p
                    if (enemy.Machine.StateObject.GetComponent<State_A_Tracking>() != null)
                    {
                        State_A_Tracking state = enemy.Machine.StateObject.GetComponent<State_A_Tracking>();
                        enemy.Machine.TransitionTo(state.StateID);
                    }
                    // �����G�l�~�[�p
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
