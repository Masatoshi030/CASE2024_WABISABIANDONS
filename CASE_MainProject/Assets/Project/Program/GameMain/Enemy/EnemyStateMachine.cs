using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    Enemy enemy;
    public Enemy EnemyComponent { get => enemy; set => enemy = value; }

    public override void Initialize()
    {
        // ステート数の取得
        int num = 0;
        for(int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            // エネミーステートの継承ならカウントを増やす
            if (stateObject.GetComponentAtIndex(i) is EnemyState)
            {
                num++;
            }
        }

        // 取得に成功したステート分配列を作る
        states = new EnemyState[num];
        stateNames = new string[num];
        num = 0;
        // 状態、状態名、辞書を作成
        for (int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            if (stateObject.GetComponentAtIndex(i) is EnemyState)
            {
                // ステートの格納
                EnemyState state = (EnemyState)stateObject.GetComponentAtIndex(i);
                state.EnemyObject = enemy;
                states[num] = state;
                stateNames[num] = states[num].StateName;
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

    public override bool TransitionTo(string key)
    {
        if(enemy.Hp <= 0.0f)
        {
            base.TransitionTo("死亡");
            return false;
        }

        bool b = base.TransitionTo(key);
        // 辞書にキーが登録されているかチェック
        if (b) { enemy.StateName = key; return true; }
        else return false;
    }
}
