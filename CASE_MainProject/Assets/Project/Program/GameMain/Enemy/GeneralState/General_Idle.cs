using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Idle : EnemyState
{
    [SerializeField, Header("‘Ò‹@ŽžŠÔ")]
    float waitInterval = 3.0f;
    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("ŽžŠÔŒo‰ßŒã‚Ì‘JˆÚ")]
    public string elapsedTransition = "ˆÚ“®";
    [SerializeField, Header("õ“G¬Œ÷Žž‚Ì‘JˆÚ")]
    public string searchSuccessTransition = "’ÇÕ";
    [SerializeField, Header("”í’eŽž‚Ì‘JˆÚ")]
    public string damagedTransition = "”í’e";
    [SerializeField, Header("Õ“ËŽž‚Ì‘JˆÚ")]
    public string collisionTransition = "’ÇÕ";

    public override void Enter()
    {
        base.Enter();
        enemy.EnemyRigidbody.velocity = Vector3.zero;
    }

    public override void MainFunc()
    {
        if(enemy.IsFindPlayer)
        {
            machine.TransitionTo(searchSuccessTransition);
        }
        else if(enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedTransition);
        }
        else if(machine.Cnt >= waitInterval)
        {
            machine.TransitionTo(elapsedTransition);
        }
    }

    public override void Exit()
    {
        
    }

    public override void CollisionEnterSelf(GameObject other)
    {
        if(other.name == "Player")
        {
            machine.TransitionTo(collisionTransition);
        }
    }

    public override void TriggerEnterSelf(GameObject other)
    {
        if (other.name == "Player")
        {
            machine.TransitionTo(collisionTransition);
        }
    }
}
