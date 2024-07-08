using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Idle                      : x = 0, y = 0
 * SweepAttack       : x = 1, y = 0
 * SlamAttack         : x = -1, y = 0
 * HatchAttack       : x = 0, y = 1
 * FlameThrow2      : x = 0, y = -1
 * FlameThrow4      : x = 1, y = -1
 * Drumming            : x = 1, y = 1
 */

public class State_Boss_Fase1 : State_Boss
{
    public enum SubState
    {
        [InspectorName("着地")]Landing,
        [InspectorName("浮遊")]Floating,
    }

    public enum MotionState
    {
        [InspectorName("判断")]Judge,
        [InspectorName("待機")]Idle,
        [InspectorName("サブステ移動")]SubStateMove,
        [InspectorName("移動回転")] Trans,
        [InspectorName("ハッチ攻撃")]Hatch,
        [InspectorName("火炎放射")]FireBeam,
        [InspectorName("薙ぎ払い")]ArmSlash,
        [InspectorName("叩きつけ")]ArmSlamming,
        [InspectorName("空")]Empty,
    }
    [SerializeField, Header("サブステート")]
    SubState subState;
    [SerializeField, Header("モーションステート")]
    MotionState motionState;
    MotionState nextMotionState = MotionState.Empty;
    bool isTranslate = false;
    bool isRotate = false;

    [SerializeField, Header("着地状態の高さ")]
    float landingHeight;
    [SerializeField, Header("浮遊状態の高さ")]
    float floatingHeight;

    // ステート変更時の移動距離
    float remaingDistance = 0.0f;
    const float subStateChangeSpeed = 5.0f;

    /* ----着地状態---- */
    // ジャッジ
    const float land_JudgeCloseMaxDistance = 17.5f;
    const float land_JudgeCloseMinDistance = 3.0f;
    const float land_JudgeRangeDistance = 25.0f;
    // 待機
    const float land_waitInterval = 3.0f;

    // 移動回転
    float land_moveSpeed = 5.0f;
    const float land_rotateSpeed = 360.0f;



    public override void Enter()
    {
        base.Enter();

        // 着地状態からスタート
        subState = SubState.Landing;
        motionState = MotionState.Judge;

        SetAnimation(new Vector2(0.0f, 0.0f));
        //ChangeSubState();
    }

    public override void MainFunc()
    {
        base.MainFunc();

        switch (subState)
        {
            case SubState.Landing:LandingFunc();break;
            case SubState.Floating: FloatingFunc();break;
        }
    }

    void LandingFunc()
    {
        switch (motionState)
        {
            case MotionState.Judge:
                Func_LandingJudge();
                break;
            case MotionState.SubStateMove:
                Func_SubStateMove();
                break;
            case MotionState.Idle:
                Func_LandingIdle();
                break;
            case MotionState.Trans:
                Func_LandingTrans();
                break;
            case MotionState.ArmSlash:

                break;
            case MotionState.Hatch:

                break;
            case MotionState.FireBeam:

                break;
        }
    }

    void FloatingFunc()
    {
        switch (motionState)
        {
            case MotionState.Judge:
                Func_FloatingJudge();
                break;
            case MotionState.SubStateMove:
                Func_SubStateMove();
                break;
            case MotionState.Idle:

                break;
            case MotionState.Trans:
                
                break;
            case MotionState.ArmSlash: 
                
                break;
            case MotionState.Hatch:
                
                break;
            case MotionState.FireBeam: 
                
                break;
        }
    }

    public void ChangeSubState()
    {
        if (subState == SubState.Landing)
        {
            subState = SubState.Floating;
            remaingDistance = Mathf.Abs(floatingHeight - boss.transform.position.y);
        }
        else
        {
            subState = SubState.Landing;
            remaingDistance = Mathf.Abs(boss.transform.position.y - landingHeight);
        }
        motionState = MotionState.SubStateMove;
    }

    void Func_SubStateMove()
    {
        // 今回のフレームの移動量を計算
        float currentFrameMoveAmount = subStateChangeSpeed * Time.deltaTime;
        bool isCompletion = false;
        remaingDistance -= currentFrameMoveAmount;
        if(remaingDistance <= 0.0f)
        {
            currentFrameMoveAmount -= remaingDistance;
            isCompletion = true;
        }
        switch (subState)
        {
            case SubState.Landing: boss.transform.Translate(0.0f, -currentFrameMoveAmount, 0.0f); break;
            case SubState.Floating: boss.transform.Translate(0.0f, currentFrameMoveAmount, 0.0f); break;
        }

        if(isCompletion)
        {
            motionState = MotionState.Judge;
        }
    }
    /* ----着地状態---- */
    // ジャッジ
    void Func_LandingJudge()
    {
        if(boss.ToPlayerDistace < land_JudgeCloseMaxDistance && boss.ToPlayerDistace >= land_JudgeCloseMinDistance)
        {
            Boss_Manager.Emotion emotion = boss.Manager.CalcEmotion();
            if(emotion == Boss_Manager.Emotion.Calm)
            {
                // 次のモーションを薙ぎ払いに設定
                nextMotionState = MotionState.ArmSlash;
                motionState = MotionState.Trans;
                // 回転のみ有効化
                isRotate = true;
                machine.ResetCount();
                boss.IsFinishAnimation = false;
            }
            else if(emotion == Boss_Manager.Emotion.Anger)
            {
                // 次のモーションを叩きつけに設定
                nextMotionState = MotionState.ArmSlamming;
                SetAnimation(new Vector2(-1.0f, 0.0f), 0.5f);
                // 移動と回転の有効化
                isTranslate = true;
                isRotate = true;
                machine.ResetCount();
                boss.IsFinishAnimation = false;
            }
        }
        else if(boss.ToPlayerDistace < land_JudgeRangeDistance && boss.ToPlayerDistace >= land_JudgeCloseMaxDistance)
        {

        }
    }

    // 待機
    void Func_LandingIdle()
    {
        if(machine.Cnt >= land_waitInterval)
        {
            // 地上状態(物理攻撃が多くなる)
            // 冷静さが50%を超えているかのチェック
            if(boss.Manager.GetEmotions()[(int)Boss_Manager.Emotion.Calm] >= 50.0f)
            {

            }
        }
    }

    void Func_LandingTrans()
    {
        switch (nextMotionState)
        {
            case MotionState.ArmSlamming:
                if(machine.Cnt >= 2.0f)
                {
                    boss.EnemyAnimator.speed = 0.0f;
                }
                if (boss.ToPlayerAngle < 10.0f && boss.ToPlayerDistace < land_JudgeCloseMaxDistance)
                {
                    boss.EnemyAnimator.speed = 1.0f;
                    motionState = nextMotionState;
                    nextMotionState = MotionState.Empty;
                }
                else
                {
                    if (isRotate)
                    {
                        Quaternion quat = boss.transform.rotation;
                        quat *= Quaternion.Euler(0.0f, land_rotateSpeed * Time.deltaTime, 0.0f);
                        boss.transform.rotation = Quaternion.Lerp(boss.transform.rotation, quat, Time.deltaTime);
                    }
                    if (isTranslate)
                    {
                        Vector3 direction = (Enemy.Target.transform.position - boss.transform.position).normalized;
                        float distance = Vector3.Distance(Enemy.Target.transform.position, boss.transform.position);
                        if (distance >= land_JudgeCloseMaxDistance)
                        {
                            boss.transform.Translate(direction * land_moveSpeed * Time.deltaTime);
                        }
                    }
                }
                break;

            case MotionState.ArmSlash:
                // 1.5秒かけて向きを整える
                if (machine.Cnt >= 1.5f)
                {
                    SetAnimation(new Vector2(1.0f, 0.0f), 0.25f);
                    motionState = nextMotionState;
                    nextMotionState = MotionState.Empty;
                    machine.ResetCount();
                }
                else
                {
                    if (isRotate)
                    {
                        Vector3 Direction = Enemy.Target.transform.position - boss.transform.position;
                        Direction.Normalize();
                        Transform trans = boss.transform;
                        trans.LookAt(Enemy.Target.transform.position);
                        trans.rotation = new Quaternion(-trans.rotation.x, -trans.rotation.y, -trans.rotation.z, trans.rotation.w);
                        boss.transform.localRotation = Quaternion.Lerp(boss.transform.localRotation, trans.rotation, Time.deltaTime);
                    }
                }
                break;
        }
    }

    void Func_ArmSlash()
    {
        if(boss.IsFinishAnimation)
        {
            boss.IsFinishAnimation = false;
            motionState = MotionState.Judge;
        }
    }


    void Func_FloatingJudge()
    {
        if (boss.ToPlayerDistace < land_JudgeCloseMaxDistance && boss.ToPlayerDistace >= land_JudgeCloseMinDistance)
        {
            Boss_Manager.Emotion emotion = boss.Manager.CalcEmotion();
            if (emotion == Boss_Manager.Emotion.Calm)
            {
                // 薙ぎ払い
                SetAnimation(new Vector2(1.0f, 0.0f), 0.25f);
                machine.ResetCount();
            }
            else if (emotion == Boss_Manager.Emotion.Anger)
            {
                SetAnimation(new Vector2(-1.0f, 0.0f), 0.25f);
                machine.ResetCount();
            }
        }
        else if (boss.ToPlayerDistace < land_JudgeRangeDistance && boss.ToPlayerDistace >= land_JudgeCloseMaxDistance)
        {

        }
    }

    void Func_FloatingIdle()
    {

    }
}
