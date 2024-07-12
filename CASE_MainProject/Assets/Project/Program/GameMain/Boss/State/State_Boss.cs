using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Boss : State
{
    protected Boss boss;
    public Boss BossObject { get => boss; set => boss = value; }

    protected string nextAnimationBooleanName = "";
    protected bool nextAnimationBoolean = false;
    protected float nextAnimationLateTime = 0.0f;
    protected bool isNextAnimation = false;
    protected float nextAnimationCount = 0.0f;


    public override void Initialize()
    {
        base.Initialize();
        boss = machine.transform.GetComponent<Boss>();
    }

    public override void MainFunc()
    {
        base.MainFunc();
        if(isNextAnimation)
        {
            nextAnimationCount += Time.deltaTime;
            if(nextAnimationCount > nextAnimationLateTime)
            {
                boss.EnemyAnimator.SetBool(nextAnimationBooleanName, nextAnimationBoolean);
                isNextAnimation = false;
            }
        }
    }

    public void SetAnimation(string variableName, bool booleanValue)
    {
        boss.EnemyAnimator.SetBool(variableName, booleanValue);
    }

    public void SetAnimation(string variableName, bool booleanValue, float duration)
    {
        nextAnimationCount = 0.0f;
        nextAnimationLateTime = duration;
        nextAnimationBooleanName = variableName;
        nextAnimationBoolean = booleanValue;
        isNextAnimation = true;
    }

    public void SetAnimationSpeed(float speed)
    {
        boss.EnemyAnimator.speed = speed;
    }
}
