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

    public virtual void CollisionEnter(Collision collision)
    {
        // 衝突開始処理
    }

    public virtual void CollisionStay(Collision collision)
    {
        // 衝突中処理
    }

    public virtual void CollisionExit(Collision collision)
    {
        // 衝突終了処理
    }

    public virtual void TriggerEnter(Collider collider)
    {
        // 衝突開始処理
    }

    public virtual void TriggerStay(Collider collider)
    {
        // 衝突中処理
    }

    public virtual void TriggerExit(Collider collider)
    {
        // 衝突終了処理
    }
}
