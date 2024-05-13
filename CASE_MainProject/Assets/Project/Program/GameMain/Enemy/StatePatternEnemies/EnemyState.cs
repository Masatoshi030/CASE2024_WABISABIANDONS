using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    // Enemyコンポーネント
    protected Enemy enemy;
    public Enemy Controller { get => enemy; set => enemy = value; }

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
}