using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : State
{
    // Enemyコンポーネント
    protected Enemy enemy;
    public Enemy EnemyObject { get => enemy; set => enemy = value; }

    [SerializeField, Header("アニメーション速度")]
    protected float animSpeed = 1.0f;
    [SerializeField, Header("開始時アニメーション")]
    protected string enterAnimation;

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
        // アニメーションの速度と指定したアニメーションの開始
        enemy.EnemyAnimator.speed = animSpeed;
        enemy.EnemyAnimator.Play(enterAnimation);
    }
}