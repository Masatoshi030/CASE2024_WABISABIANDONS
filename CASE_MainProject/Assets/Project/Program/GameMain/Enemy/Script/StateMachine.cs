using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [System.Serializable]
    public class StateData
    {
        [SerializeField, Header("ID")]
        public int id;
        [SerializeField, Header("状態名")]
        public string name;
        [SerializeField, Header("ステート")]
        public State state;
    }


    // このコンポーネントを所持してるオブジェクト
    protected GameObject controller;
    public GameObject Controller { get => controller; set => controller = value; }

    [SerializeField, Header("ステートの参照先オブジェクト")]
    protected GameObject stateObject;
    public GameObject StateObject { get => stateObject; }

    [SerializeField, Header("初期状態ID")]
    protected int initStateID = 2;

    [SerializeField, Header("現在の状態"), ReadOnly]
    protected StateData currentState;
    protected StateData nextState;

    // 状態リスト
    [SerializeField, Header("状態データ")]
    protected Dictionary<int,StateData> stateDatas = new Dictionary<int, StateData>();
    public Dictionary<int, StateData> StateDatas { get => stateDatas; }


    [SerializeField, Header("経過時間"), ReadOnly]
    float cnt;
    public float Cnt { get => cnt; }

    [SerializeField, Header("更新の有無")]
    protected bool isUpdate = true;
    public bool IsUpdate { get => isUpdate; set => isUpdate = value; }

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
        controller = gameObject;

        int num = 0;
        // 状態、状態名、辞書を作成
        for (int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            if (stateObject.GetComponentAtIndex(i) is State)
            {
                // ステートの格納
                State state = (State)stateObject.GetComponentAtIndex(i);
                AddState(state);
                num++;
            }
        }
        // 初期ステートを設定
        currentState = stateDatas[initStateID];
        currentState.state.Enter();
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
        if (currentState != null)
        {
            cnt += Time.deltaTime;
            // 現在の状態のメイン処理を呼ぶ
            currentState.state.MainFunc();
        }
    }

    /*
     * <summary>
     * 遷移処理
     * <param>
     * int 遷移先ID
     * <return>
     * bool 遷移の成否
     */
    public virtual bool TransitionTo(int id)
    {
            if(stateDatas.ContainsKey(id))
            {
                nextState = stateDatas[id];
                cnt = 0.0f;
                currentState.state.Exit();
                currentState = nextState;
                nextState = null;
                currentState.state.Enter();
                return true;
            }
            else
            {
                Debug.Log(transform.name + " ステート更新エラー StateID : " + id);
                return false;
            }
    }

    public virtual bool TransitionTo(State state)
    {
        nextState.state = state;
        nextState.id = state.StateID;
        nextState.name = state.name;
        cnt = 0.0f;
        currentState.state.Exit();
        currentState = nextState;
        nextState = null;
        currentState.state.Enter();
        return true;
    }

    public virtual void AddState(State state)
    {
        StateData data = new StateData();
        data.state = state;
        data.id = state.StateID;
        data.name = state.StateName;
        data.state.Machine = this;
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
        if (currentState != null) currentState.state.CollisionEnterSelf(collision);
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
        if (currentState != null) currentState.state.CollisionEnterOpponent(collision);
    }

    public virtual void CollisionStaySelf(Collision collision)
    {
        if (currentState != null) currentState.state.CollisionStaySelf(collision);
    }

    public virtual void CollisionStayOpponent(Collision collision)
    {
        if (currentState != null) currentState.state.CollisionStayOpponent(collision);
    }

    public virtual void CollisionExitSelf(Collision collision)
    {
        if (currentState != null) currentState.state.CollisionExitSelf(collision);
    }

    public virtual void CollisionExitOpponent(Collision collision)
    {
        if (currentState != null) currentState.state.CollisionExitOpponent(collision);
    }

    public virtual void TriggerEnterSelf(Collider other)
    {
        if (currentState != null) currentState.state.TriggerEnterSelf(other);
    }

    public virtual void TriggerEnterOpponent(Collider other)
    {
        if (currentState != null) currentState.state.TriggerEnterOpponent(other);
    }

    public virtual void TriggerStaySelf(Collider other)
    {
        if (currentState != null) currentState.state.TriggerStaySelf(other);
    }

    public virtual void TriggerStayOpponent(Collider other)
    {
        if (currentState != null) currentState.state.TriggerStayOpponent(other);
    }

    public virtual void TriggerExitSelf(Collider other)
    {
        if (currentState != null) currentState.state.TriggerExitSelf(other);
    }

    public virtual void TriggerExitOpponent(Collider other)
    {
        if (currentState != null) currentState.state.TriggerExitOpponent(other);
    }
}
