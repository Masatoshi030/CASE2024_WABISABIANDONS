using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    protected const float pad = 10.0f;

    GameObject controller;
    public GameObject Controller { get => controller; set => controller = value; }
    // ステートマシンコンポーネント
    protected StateMachine machine;
    public StateMachine Machine { get => machine; set => machine = value; }

    [SerializeField, Header("状態名")]
    string stateName;
    public string StateName { get => stateName; }

    public virtual void Initialize()
    {
        // 初期化処理
    }

    public virtual void Enter()
    {
        // 開始処理
    }

    public virtual void MainFunc()
    {
        // 更新処理
    }

    public virtual void Exit()
    {
        // 終了時処理
    }

    public virtual void CollisionEnterSelf(GameObject other)
    {
        // 衝突開始処理(セルフ呼び出し)
    }

    public virtual void CollisionEnterOpponent(GameObject other)
    {
        // 衝突開始処理(相手呼び出し)
    }

    public virtual void CollisionStaySelf(GameObject other)
    {
        // 衝突中処理(セルフ呼び出し)
    }

    public virtual void CollisionStayOpponent(GameObject other)
    {
        // 衝突中処理(相手呼び出し)
    }

    public virtual void CollisionExitSelf(GameObject other)
    {
        // 衝突終了処理(セルフ呼び出し)
    }

    public virtual void CollisionExitOpponent(GameObject other)
    {
        // 衝突終了処理(相手呼び出し)
    }

    public virtual void TriggerEnterSelf(GameObject other)
    {
        // 衝突開始処理(セルフ呼び出し)
    }

    public virtual void TriggerEnterOpponent(GameObject other)
    {
        // 衝突開始処理(相手呼び出し)
    }

    public virtual void TriggerStaySelf(GameObject other)
    {
        // 衝突中処理(セルフ呼び出し)
    }

    public virtual void TriggerStayOpponent(GameObject other)
    {
        // 衝突中処理(相手呼び出し)
    }

    public virtual void TriggerExitSelf(GameObject other)
    {
        // 衝突終了処理(セルフ呼び出し)
    }

    public virtual void TriggerExitOpponent(GameObject other)
    {
        // 衝突終了処理(相手呼び出し)
    }
}
