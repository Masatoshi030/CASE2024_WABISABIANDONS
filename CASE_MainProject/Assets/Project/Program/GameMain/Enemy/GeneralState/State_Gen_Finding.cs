using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class State_Gen_Finding : EnemyState
{
    [SerializeField, Header("回転速度")]
    float rotationSpeed;

    [SerializeField, Header("回転時間")]
    float rotationTime;

    [SerializeField, Header("矢印の長さ")]
    float allowLength = 10.0f;

    [SerializeField, Header("矢印に適用するマテリアル")]
    Material allowMaterial;

    [SerializeField, Header("発見時エフェクト")]
    GameObject foundEffect;

    [SerializeField, Header("溜めエフェクト")]
    GameObject accumulateEffect;
    bool bAccumulate = false;

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("経過時の遷移")]
    StateKey elapsedKey;
    [SerializeField, Header("ダメージ時の遷移")]
    StateKey damagedKey;

    public override void Initialize()
    {
        base.Initialize();
        if(accumulateEffect != null)
        {
            bAccumulate = true;
        }
    }

    public override void Enter()
    {
        base.Enter();
        // 矢印の表示
        enemy.SetAllowActive(true);
        enemy.AllowObject.GetComponent<TargetAllow>().SetMaterial(allowMaterial);

        // 検知の通知
        enemy.SendMsg<int>(0, 0);
        enemy.IsVelocityZero = true;
        // 他からの通知をさえぎるため攻撃中にする
        enemy.IsAttackNow = true;

        //検知時の発見エフェクト生成
        GameObject foundEffectObjectBuf = Instantiate(foundEffect, enemy.EyeTransform.position, Quaternion.LookRotation(-enemy.EyeTransform.forward));
        foundEffectObjectBuf.transform.parent = enemy.EyeTransform;

        if(bAccumulate)
        {
            accumulateEffect.GetComponent<VisualEffect>().SendEvent("OnPlay");
        }
    }

    public override void MainFunc()
    {
        base.MainFunc();
        Quaternion yOnly = enemy.transform.rotation;
        yOnly.x = 0.0f;
        yOnly.z = 0.0f;
        enemy.transform.rotation = yOnly;
        float Distance = Vector3.Distance(enemy.EyeTransform.position, Enemy.Target.transform.position);
        if(Distance < allowLength)
        {
            enemy.AllowObject.GetComponent<TargetAllow>().StartDesignation(enemy.EyeTransform.position, enemy.EyeTransform.position + enemy.EyeTransform.forward * Distance);
        }
        else
        {
            enemy.AllowObject.GetComponent<TargetAllow>().StartDesignation(enemy.EyeTransform.position, enemy.EyeTransform.position + enemy.EyeTransform.forward * allowLength);
        }

        // 向きたい方向
        Vector3 direction = (Enemy.Target.transform.position - enemy.transform.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction, enemy.transform.up);
        rotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);

        enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, rotation, Time.deltaTime * 10);

        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedKey);
        }
        else if(machine.Cnt >= rotationTime)
        {
            machine.TransitionTo(elapsedKey);
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (bAccumulate)
        {
            accumulateEffect.GetComponent<VisualEffect>().SendEvent("OnStop");
        }
        enemy.IsAttackNow = false;
        enemy.SetAllowActive(false);
        enemy.AllowObject.GetComponent<TargetAllow>().EndDesignation();
        enemy.IsVelocityZero = false;
    }
}
