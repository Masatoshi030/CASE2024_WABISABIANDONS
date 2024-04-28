using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Mob : Enemy_Parent
{
    public enum State
    {
        [InspectorName("待機")] Idle,
        [InspectorName("移動")] Move,
        [InspectorName("追尾")] Tracking,
        [InspectorName("逃走")] Escape,
        [InspectorName("攻撃A")] AttackA,
        [InspectorName("攻撃B")] AttackB,
        [InspectorName("特殊A")]SpecialA,
        [InspectorName("特殊B")]SpecialB,
        [InspectorName("回復")] Heal,
        [InspectorName("破壊")] Death,
    }
    [SerializeField, Header("状態"), Toolbar(typeof(State))]
    State state = State.Idle;
    
    protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
        switch (state)
        {
            case State.Idle:IdleFunc(); break;
            case State.Move: MoveFunc(); break;
            case State.Tracking: TrackingFunc(); break;
            case State.Escape: EscapeFunc(); break;
            case State.AttackA: AttackFunc1(); break;
            case State.AttackB: AttackFunc2(); break;
            case State.SpecialA: SpecialFuncA(); break;
            case State.SpecialB: SpecialFuncB(); break;
            case State.Heal: HealFunc(); break;
            case State.Death: break;
        }
    }

    /*
     * <summary>
     * 待機状態関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void IdleFunc();

    /*
     * <summary>
     * 移動状態関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void MoveFunc();

    /*
     * <summary>
     * 追尾状態関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void TrackingFunc();

    /*
     * <summary>
     * 逃走状態関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void EscapeFunc();

    /*
     * <summary>
     * 攻撃関数パターンA
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void AttackFunc1();

    /*
     * <summary>
     * 攻撃関数パターンB
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void AttackFunc2();

    /*
     * <summary>
     * 攻撃関数パターンB
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void SpecialFuncA();

    /*
     * <summary>
     * 攻撃関数パターンB
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void SpecialFuncB();

    /*
     * <summary>
     * 回復状態関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void HealFunc();

    /*
     * <summary>
     * 死亡状態関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void DeathFunc();

    protected override void DestroyFunc()
    {

    }
}
