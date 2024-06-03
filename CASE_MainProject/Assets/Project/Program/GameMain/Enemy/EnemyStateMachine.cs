using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    Enemy enemy;
    public Enemy EnemyComponent { get => enemy; set => enemy = value; }

    /*
     * <summary>
     * ����������
     * <param>
     * void
     * <return>
     * void
     */
    public override void Initialize()
    {
        int num = 0;
        // ��ԁA��Ԗ��A�������쐬
        for (int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            if (stateObject.GetComponentAtIndex(i) is EnemyState)
            {
                // �X�e�[�g�̊i�[
                EnemyState state = (EnemyState)stateObject.GetComponentAtIndex(i);
                state.EnemyObject = enemy;
                states.Add(state);
                stateNames.Add(states[num].StateName);
                states[num].Machine = this;
                states[num].Initialize();
                stateList.Add(stateNames[num], states[num]);
                num++;
            }
        }
        // �����X�e�[�g�͔z���0�Ԗ�
        currentState = states[0];
        currentState.Enter();
        enemy.StateName = stateNames[0];
    }

    /*
     * <summary>
     * �J�ڏ���
     * <param>
     * string �J�ڐ於��
     * <return>
     * bool �J�ڂ̐���
     */
    public override bool TransitionTo(string key)
    {
        bool b = base.TransitionTo(key);
        // �����ɃL�[���o�^����Ă��邩�`�F�b�N
        if (b) { enemy.StateName = key; return true; }
        else return false;
    }

    public override void AddState(State state)
    {
        if(!stateList.ContainsKey(state.StateName))
        {
            if (state is EnemyState)
            {
                EnemyState es = (EnemyState)state;
                es.EnemyObject = enemy;
                es.Machine = this;
                es.Initialize();
                states.Add(state);
                stateNames.Add(state.StateName);
                stateList.Add(es.StateName, es);
            }
        }
    }
}
