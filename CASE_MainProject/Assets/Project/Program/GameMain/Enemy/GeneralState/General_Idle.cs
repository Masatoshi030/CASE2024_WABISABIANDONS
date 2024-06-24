using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Idle : EnemyState
{
    [SerializeField, Header("‘Ò‹@ŠÔ")]
    float waitInterval = 3.0f;
    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("ŠÔŒo‰ßŒã‚Ì‘JˆÚ")]
    public int elapsedID;
    [SerializeField, Header("õ“G¬Œ÷‚Ì‘JˆÚ")]
    public int searchSuccessID;
    [SerializeField, Header("”í’e‚Ì‘JˆÚ")]
    public int damagedID;
    [SerializeField, Header("Õ“Ë‚Ì‘JˆÚ")]
    public int collisionID;

    public override void Enter()
    {
        base.Enter();
        enemy.IsVelocityZero = true;
    }

    public override void MainFunc()
    {
        base.MainFunc();

        if(enemy.IsFindPlayer)
        {
            machine.TransitionTo(searchSuccessID);
            enemy.SendMsg<int>(0, 0);
        }
        else if(enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedID);
            enemy.SendMsg<int>(1, 0);
        }
        else if(machine.Cnt >= waitInterval)
        {
            machine.TransitionTo(elapsedID);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void CollisionEnterSelf(Collision collision)
    {
        if(collision.transform.name == "Player")
        {
            machine.TransitionTo(collisionID);
        }
    }

    public override void TriggerEnterSelf(Collider other)
    {
        if (other.name == "Player")
        {
            machine.TransitionTo(collisionID);
        }
    }
}
