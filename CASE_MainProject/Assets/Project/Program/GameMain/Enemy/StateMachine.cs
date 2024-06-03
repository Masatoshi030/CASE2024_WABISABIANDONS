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
    public GameObject StateObject { get => stateObject; }

    [SerializeField, Header("現在の状態"), ReadOnly]
    protected State currentState;

    protected State nextState;

    [SerializeField, Header("状態一覧")]
    protected List<State> states;
    [SerializeField, Header("状態名一覧"), ReadOnly]
    protected List<string> stateNames;

    [SerializeField, Header("状態")]
    protected Dictionary<string, State> stateList = new Dictionary<string, State>();
    public Dictionary<string, State> StateList { get => stateList; }

    [SerializeField, Header("経過時間"), ReadOnly]
    float cnt;
    public float Cnt { get => cnt; }

    bool bTransition = false;

    /*
     * <summary>
     * 初期化処理
     * <param>
     * void
     * <return>
     * void
     */
    public virtual void Initialize()
    {
        int num = 0;
        // 状態、状態名、辞書を作成
        for (int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            if (stateObject.GetComponentAtIndex(i) is State)
            {
                // ステートの格納
                states.Add((State)stateObject.GetComponentAtIndex(i));
                stateNames.Add(states[num].StateName);
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

    /*
     * <summary>
     * メイン処理
     * <param>
     * void
     * <return>
     * void
     */
    public virtual void MainFunc()
    {
        if(bTransition)
        {
            cnt = 0.0f;
            currentState.Exit();
            currentState = nextState;
            nextState = null;
            bTransition = false;
            currentState.Enter();
        }
        if (currentState != null)
        {
            cnt += Time.deltaTime;
            // 現在の状態のメイン処理を呼ぶ
            currentState.MainFunc();
        }
    }

    /*
     * <summary>
     * 遷移処理
     * <param>
     * string 遷移先名称
     * <return>
     * bool 遷移の成否
     */
    public virtual bool TransitionTo(string key)
    {
        if(!bTransition)
        {
            // 辞書にキーが登録されているかチェック
            if (stateList.ContainsKey(key))
            {
                nextState = stateList[key];
                // 遷移予約
                bTransition = true;
                return true;
            }
            else
            {
                Debug.Log("ステート更新エラー : " + controller.name + "( stateName : " + key + " )");
                return false;
            }
        }
        return false;
    }

    /*
     * <summary>
     * 遷移処理
     * <param>
     * int 遷移先のインデックス
     * <return>
     * bool 遷移の成否
     */
    public virtual bool TransitionTo(int index)
    {
        if (index >= states.Count || bTransition)
        {
            return false;
        }
        nextState = states[index];
        // 遷移予約
        bTransition = true;
        return true;
    }

    public virtual void AddState(State state)
    {
        state.Machine = this;
        states.Add(state);
        stateNames.Add(state.StateName);
        stateList.Add(state.StateName, state);
        state.Initialize();
    }

    /*
     * <summary>
     * 衝突の処理(自身で呼び出し)
     * <param>
     * GameObject 衝突オブジェクト
     * <return>
     * void
     */
    public virtual void CollisionEnterSelf(Collision collision)
    {
        if (currentState != null) currentState.CollisionEnterSelf(collision);
    }

    /*
     * <summary>
     * 衝突の処理(相手側呼び出し)
     * <param>
     * GameObject 衝突オブジェクト
     * <return>
     * void
     */
    public virtual void CollisionEnterOpponent(Collision collision)
    {
        if (currentState != null) currentState.CollisionEnterOpponent(collision);
    }

    public virtual void CollisionStaySelf(Collision collision)
    {
        if (currentState != null) currentState.CollisionStaySelf(collision);
    }

    public virtual void CollisionStayOpponent(Collision collision)
    {
        if (currentState != null) currentState.CollisionStayOpponent(collision);
    }

    public virtual void CollisionExitSelf(Collision collision)
    {
        if (currentState != null) currentState.CollisionExitSelf(collision);
    }

    public virtual void CollisionExitOpponent(Collision collision)
    {
        if (currentState != null) currentState.CollisionExitOpponent(collision);
    }

    public virtual void TriggerEnterSelf(Collider other)
    {
        if (currentState != null) currentState.TriggerEnterSelf(other);
    }

    public virtual void TriggerEnterOpponent(Collider other)
    {
        if (currentState != null) currentState.TriggerEnterOpponent(other);
    }

    public virtual void TriggerStaySelf(Collider other)
    {
        if (currentState != null) currentState.TriggerStaySelf(other);
    }

    public virtual void TriggerStayOpponent(Collider other)
    {
        if (currentState != null) currentState.TriggerStayOpponent(other);
    }

    public virtual void TriggerExitSelf(Collider other)
    {
        if (currentState != null) currentState.TriggerExitSelf(other);
    }

    public virtual void TriggerExitOpponent(Collider other)
    {
        if (currentState != null) currentState.TriggerExitOpponent(other);
    }
}
