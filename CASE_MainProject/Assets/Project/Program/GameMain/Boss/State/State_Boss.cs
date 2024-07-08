using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Boss : State
{
    protected Boss boss;
    public Boss BossObject { get => boss; set => boss = value; }

    [SerializeField, Header("アニメーションパラメータ"),ReadOnly]
    protected Vector2 animationVector;
    [SerializeField, Header("補間速度")]
    protected float lerpSpeed;
    protected float lerpCnt;

    protected bool isLerp = false;

    Vector2 purposAnimaitonVector;

    public override void Initialize()
    {
        base.Initialize();
        boss = machine.transform.GetComponent<Boss>();
    }

    public override void MainFunc()
    {
        base.MainFunc();
        if(isLerp)
        {
            lerpCnt += Time.deltaTime * lerpSpeed;
            animationVector = Vector2.Lerp(animationVector, purposAnimaitonVector, lerpSpeed);
            boss.EnemyAnimator.SetFloat("BlendX", animationVector.x);
            boss.EnemyAnimator.SetFloat("BlendY", animationVector.y);
        }
    }

    public void SetAnimation(Vector2 newPurpos, float newSpeed)
    {
        purposAnimaitonVector = newPurpos;
        lerpSpeed = 1.0f / newSpeed;
        isLerp = true;
        lerpCnt = 0.0f;
    }

    public void SetAnimation(Vector2 newVector2)
    {
        animationVector = newVector2;
        boss.EnemyAnimator.SetFloat("BlendX", animationVector.x);
        boss.EnemyAnimator.SetFloat("BlendY", animationVector.y);
    }
}
