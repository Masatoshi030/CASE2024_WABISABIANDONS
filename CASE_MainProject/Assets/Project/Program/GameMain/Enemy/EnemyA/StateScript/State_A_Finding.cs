using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_A_Finding : EnemyState
{
    [SerializeField, Header("‰ñ“]‘¬“x")]
    float rotationSpeed;

    [SerializeField, Header("‰ñ“]ŠÔ")]
    float rotationTime;

    [SerializeField, Header("–îˆó‚Ì’·‚³")]
    float allowLength = 10.0f;

    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("Œo‰ß‚Ì‘JˆÚ")]
    StateKey elapsedKey;
    [SerializeField, Header("ƒ_ƒ[ƒW‚Ì‘JˆÚ")]
    StateKey damagedKey;

    public override void Enter()
    {
        base.Enter();
        enemy.SetAllowActive(true);
        enemy.IsVelocityZero = true;
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

        // Œü‚«‚½‚¢•ûŒü
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
        enemy.SetAllowActive(false);
        enemy.AllowObject.GetComponent<TargetAllow>().EndDesignation();
        enemy.IsVelocityZero = false;
    }
}
