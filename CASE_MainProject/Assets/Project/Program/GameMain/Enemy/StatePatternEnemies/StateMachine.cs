using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField, Header("ステートの参照先オブジェクト")]
    GameObject stateObject;

    Enemy enemy;
    public Enemy Controller { get => enemy; set => enemy = value; }

    [SerializeField, Header("現在の状態"), ReadOnly]
    EnemyState currentState;

    EnemyState nextState;
    bool bNextState = false;

    [SerializeField, Header("状態一覧")]
    EnemyState[] states;
    [SerializeField, Header("状態名一覧"), ReadOnly]
    string[] stateNames;

    [SerializeField, Header("経過時間"), ReadOnly]
    float cnt;
    public float Cnt { get => cnt; }

    Dictionary<string, EnemyState> stateList = new Dictionary<string, EnemyState>();


    public void Initialize()
    {
        int num = 0;
        for(int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            if (stateObject.GetComponentAtIndex(i) is EnemyState)
            {
                num++;
            }
        }

        states = new EnemyState[num];
        num = 0;
        for (int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            if (stateObject.GetComponentAtIndex(i) is EnemyState)
            {
                states[num] = (EnemyState)stateObject.GetComponentAtIndex(i);
                num++;
            }
        }

        stateNames = new string[states.Length];
        // リストの作成
        for(int i = 0; i < states.Length; i++)
        {
            stateNames[i] = states[i].StateName;
            stateList.Add(stateNames[i] ,states[i]);
            states[i].Controller = enemy;
            states[i].Machine = this;
            // 初期化
            states[i].Initialize();
        }
        // 初期ステートは配列の0番目
        currentState = states[0];
        currentState.Enter();
        enemy.StateName = stateNames[0];
    }

    public void MainFunc()
    {
        if(currentState != null)
        {
            Debug.Log(enemy.name);
            cnt += Time.deltaTime;
            currentState.MainFunc();
        }
        
        if(bNextState)
        {
            cnt = 0.0f;
            currentState.Exit();
            currentState = nextState;
            bNextState = false;
            currentState.Enter();
        }
    }

    public void TransitionTo(string key)
    {
        if(stateList.ContainsKey(key))
        {
            bNextState = true;
            nextState = stateList[key];
            enemy.StateName = key;
        }
        else
        {
            Debug.Log("ステート更新エラー : " + enemy.name + "(stateName : " + nextState + ")");
        }
    }
}
