using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine : StateMachine
{
    Boss boss;
    public Boss BossComponent { get => boss; set => boss = value; }
    public override void Initialize()
    {
        controller = gameObject;

        int num = 0;
        // 状態、状態名、辞書を作成
        for (int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            if (stateObject.GetComponentAtIndex(i) is State_Boss)
            {
                // ステートの格納
                State_Boss state = (State_Boss)stateObject.GetComponentAtIndex(i);
                AddState(state);
                num++;
            }
        }
        // 初期ステートを設定
        currentState = stateDatas[initStateID];
        currentState.state.Enter();
        boss.StateName = stateDatas[initStateID].name;
    }

    public override void AddState(State state)
    {
        if (!stateDatas.ContainsKey(state.StateID))
        {
            if (state is State_Boss)
            {
                State_Boss es = (State_Boss)state;
                es.BossObject = boss;
                es.Machine = this;
                StateData data = new StateData();
                data.state = state;
                data.id = state.StateID;
                data.name = state.StateName;
                es.Initialize();
                stateDatas.Add(data.id, data);
            }
        }
    }
}
