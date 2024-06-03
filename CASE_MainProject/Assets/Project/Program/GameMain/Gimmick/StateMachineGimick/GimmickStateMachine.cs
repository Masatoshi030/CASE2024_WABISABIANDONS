using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickStateMachine : StateMachine
{
    Gimmick gimmick;

    public Gimmick GimmickTarget { get => gimmick; set => gimmick = value; }
    public override void Initialize()
    {
        int num = 0;
        // 状態、状態名、辞書を作成
        for (int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            if (stateObject.GetComponentAtIndex(i) is GimmickState)
            {
                // ステートの格納
                GimmickState state = (GimmickState)stateObject.GetComponentAtIndex(i);
                state.GimmickObject = gimmick;
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
        gimmick.StateName = stateNames[0];
    }
}
