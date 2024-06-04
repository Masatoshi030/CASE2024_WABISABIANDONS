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
                AddState(state);
                num++;
            }
        }
        // �����X�e�[�g��ݒ�
        currentState = stateDatas[initStateID];
        currentState.state.Enter();
        enemy.StateName = stateDatas[0].name;
    }

    public override bool TransitionTo(int key)
    {
        bool b = base.TransitionTo(key);
        // �����ɃL�[���o�^����Ă��邩�`�F�b�N
        if (b) { enemy.StateName = stateDatas[key].name; return true; }
        else return false;
    }

    public override void AddState(State state)
    {
        if(!idDatas.Contains(state.StateID))
        {
            if (state is EnemyState)
            {
                EnemyState es = (EnemyState)state;
                es.EnemyObject = enemy;
                es.Machine = this;
                StateData data = new StateData();
                data.state = state;
                data.id = state.StateID;
                data.name = state.StateName;
                es.Initialize();
                stateDatas.Add(data);
                idDatas.Add(data.id);
            }
        }
    }
}
