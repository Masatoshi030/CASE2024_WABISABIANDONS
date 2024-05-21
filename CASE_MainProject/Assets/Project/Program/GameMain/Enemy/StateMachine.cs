using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // このコンポーネントを所持してるオブジェクト
    protected GameObject controller;
    public GameObject Controller { get => controller; set => controller = value; }

    [SerializeField, Header("ステートの参照先オブジェクト")]
    protected GameObject stateObject;

    [SerializeField, Header("現在の状態"), ReadOnly]
    protected State currentState;

    protected State nextState;

    [SerializeField, Header("状態一覧")]
    protected State[] states;
    [SerializeField, Header("状態名一覧"), ReadOnly]
    protected string[] stateNames;

    protected Dictionary<string, State> stateList = new Dictionary<string, State>();

    [SerializeField, Header("経過時間"), ReadOnly]
    float cnt;
    public float Cnt { get => cnt; }

    public virtual void Initialize()
    {
        // ステート数の取得
        int num = 0;
        for (int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            // エネミーステートの継承ならカウントを増やす
            if (stateObject.GetComponentAtIndex(i) is State)
            {
                num++;
            }
        }

        // 取得に成功したステート分配列を作る
        states = new State[num];
        stateNames = new string[num];
        num = 0;
        // 状態、状態名、辞書を作成
        for (int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            if (stateObject.GetComponentAtIndex(i) is State)
            {
                // ステートの格納
                states[num] = (State)stateObject.GetComponentAtIndex(i);
                stateNames[num] = states[num].StateName;
                states[num].Controller = controller;
                states[num].Machine = this;
                states[num].Initialize();
                stateList.Add(stateNames[num], states[num]);
                num++;
            }
        }
        // 初期ステートは配列の0番目
        currentState = states[0];
        currentState.Enter();
    }

    public virtual void MainFunc()
    {
        if (currentState != null)
        {
            cnt += Time.deltaTime;
            // 現在の状態のメイン処理を呼ぶ
            currentState.MainFunc();
        }
    }

    public virtual bool TransitionTo(string key)
    {
        // 辞書にキーが登録されているかチェック
        if (stateList.ContainsKey(key))
        {
            nextState = stateList[key];
            cnt = 0.0f;
            currentState.Exit();
            currentState = nextState;
            nextState = null;
            currentState.Enter();
            return true;
        }
        else
        {
            Debug.Log("ステート更新エラー : " + controller.name + "( stateName : " + nextState.StateName + " )");
            return false;
        }
    }

    public virtual void TransitionTo(int index)
    {
        if (index >= states.Length)
        {
            index %= states.Length;
        }
        nextState = states[index];
        cnt = 0.0f;
        currentState.Exit();
        currentState = nextState;
        currentState.Enter();
        nextState = null;
    }

    public virtual void CollisionEnterSelf(GameObject other)
    {
        if (currentState != null) currentState.CollisionEnterSelf(other);
    }

    public virtual void CollisionEnterOpponent(GameObject other)
    {
        if (currentState != null) currentState.CollisionEnterOpponent(other);
    }

    public virtual void CollisionStaySelf(GameObject other)
    {
        if (currentState != null) currentState.CollisionStaySelf(other);
    }

    public virtual void CollisionStayOpponent(GameObject other)
    {
        if (currentState != null) currentState.CollisionStayOpponent(other);
    }

    public virtual void CollisionExitSelf(GameObject other)
    {
        if (currentState != null) currentState.CollisionExitSelf(other);
    }

    public virtual void CollisionExitOpponent(GameObject other)
    {
        if (currentState != null) currentState.CollisionExitOpponent(other);
    }

    public virtual void TriggerEnterSelf(GameObject other)
    {
        if (currentState != null) currentState.TriggerEnterSelf(other);
    }

    public virtual void TriggerEnterOpponent(GameObject other)
    {
        if (currentState != null) currentState.TriggerEnterOpponent(other);
    }

    public virtual void TriggerStaySelf(GameObject other)
    {
        if (currentState != null) currentState.TriggerStaySelf(other);
    }

    public virtual void TriggerStayOpponent(GameObject other)
    {
        if (currentState != null) currentState.TriggerStayOpponent(other);
    }

    public virtual void TriggerExitSelf(GameObject other)
    {
        if (currentState != null) currentState.TriggerExitSelf(other);
    }

    public virtual void TriggerExitOpponent(GameObject other)
    {
        if (currentState != null) currentState.TriggerExitOpponent(other);
    }
}
