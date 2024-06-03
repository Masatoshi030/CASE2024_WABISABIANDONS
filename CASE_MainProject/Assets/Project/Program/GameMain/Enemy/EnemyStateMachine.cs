using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    Enemy enemy;
    public Enemy EnemyComponent { get => enemy; set => enemy = value; }

    /*
     * <summary>
     * 初期化処理
     * <param>
     * void
     * <return>
     * void
     */
    public override void Initialize()
    {
        int num = 0;
        // 状態、状態名、辞書を作成
        for (int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            if (stateObject.GetComponentAtIndex(i) is EnemyState)
            {
                // ステートの格納
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
        // 初期ステートは配列の0番目
        currentState = states[0];
        currentState.Enter();
        enemy.StateName = stateNames[0];
    }

    /*
     * <summary>
     * 遷移処理
     * <param>
     * string 遷移先名称
     * <return>
     * bool 遷移の成否
     */
    public override bool TransitionTo(string key)
    {
        bool b = base.TransitionTo(key);
        // 辞書にキーが登録されているかチェック
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
