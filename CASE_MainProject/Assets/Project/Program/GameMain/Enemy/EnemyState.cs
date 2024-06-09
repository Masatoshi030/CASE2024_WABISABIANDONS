using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : State
{
    // Enemyコンポーネント
    protected Enemy enemy;
    public Enemy EnemyObject { get => enemy; set => enemy = value; }
    [SerializeField, Header("アニメーションをするか")]
    bool isAnimation = true;
    [SerializeField, Header("開始時アニメーション")]
    protected string enterAnimation;
    [SerializeField, Header("アニメーション速度")]
    protected float animSpeed = 1.0f;
    [SerializeField, Header("使用蒸気量")]
    protected float pressureAmount;
    [SerializeField, Header("-圧力一定以下の遷移ID-")]
    protected int pressureZeroID = 1;

    public override void Initialize()
    {
        base.Initialize();
    }

    /*
     * <summary>
     * 開始時処理
     * <param>
     * void
     * <return>
     * void
     */
    public override void Enter()
    {
        base.Enter();
        // アニメーションの速度と指定したアニメーションの開始
        if(isAnimation)
        {
            enemy.EnemyAnimator.enabled = true;
            enemy.EnemyAnimator.speed = animSpeed;
            enemy.EnemyAnimator.Play(enterAnimation);
        }
        else
        {
            enemy.EnemyAnimator.enabled = false;
        }
    }

    public override void MainFunc()
    {
        base.MainFunc();

        enemy.CalcPressure(-pressureAmount * Time.deltaTime);
        if(enemy.Pressre <= 0.0)
        {
            machine.IsUpdate = false;
            machine.TransitionTo(pressureZeroID);
        }
    }
}