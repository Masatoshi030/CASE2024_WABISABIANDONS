using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    protected const float pad = 10.0f;

    protected bool continueProcessing = true;

    GameObject controller;
    public GameObject Controller { get => controller; set => controller = value; }
    // ステートマシンコンポーネント
    protected StateMachine machine;
    public StateMachine Machine { get => machine; set => machine = value; }

    [SerializeField, Header("状態名")]
    string stateName;
    public string StateName { get => stateName; set => stateName = value; }

    [SerializeField, Header("状態ID")]
    int stateID;
    public int StateID { get => stateID; }

    public virtual void Initialize()
    {
        // 初期化処理
    }

    public virtual void Enter()
    {
        continueProcessing = true;
        // 開始処理
    }

    public virtual void MainFunc()
    {
        // 更新処理
    }

    public virtual void Exit()
    {
        continueProcessing = false;
        // 終了時処理
    }

    public virtual void CollisionEnterSelf(Collision collision)
    {
        // 衝突開始処理(セルフ呼び出し)
    }

    public virtual void CollisionEnterOpponent(Collision collision)
    {
        // 衝突開始処理(相手呼び出し)
    }

    public virtual void CollisionStaySelf(Collision collision)
    {
        // 衝突中処理(セルフ呼び出し)
    }

    public virtual void CollisionStayOpponent(Collision collision)
    {
        // 衝突中処理(相手呼び出し)
    }

    public virtual void CollisionExitSelf(Collision collision)
    {
        // 衝突終了処理(セルフ呼び出し)
    }

    public virtual void CollisionExitOpponent(Collision collision)
    {
        // 衝突終了処理(相手呼び出し)
    }

    public virtual void TriggerEnterSelf(Collider other)
    {
        // 衝突開始処理(セルフ呼び出し)
    }

    public virtual void TriggerEnterOpponent(Collider other)
    {
        // 衝突開始処理(相手呼び出し)
    }

    public virtual void TriggerStaySelf(Collider other)
    {
        // 衝突中処理(セルフ呼び出し)
    }

    public virtual void TriggerStayOpponent(Collider other)
    {
        // 衝突中処理(相手呼び出し)
    }

    public virtual void TriggerExitSelf(Collider other)
    {
        // 衝突終了処理(セルフ呼び出し)
    }

    public virtual void TriggerExitOpponent(Collider other)
    {
        // 衝突終了処理(相手呼び出し)
    }
}
