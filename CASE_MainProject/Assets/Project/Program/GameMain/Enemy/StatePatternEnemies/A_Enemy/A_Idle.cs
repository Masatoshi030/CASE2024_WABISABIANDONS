using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Idle : EnemyState
{
    [SerializeField, Header("‘Ò‹@ŽžŠÔ")]
    float waitInterval = 3.0f;
    [SerializeField, Header("Œo‰ßŒã‚Ì‘JˆÚ")]
    public string elapsedTransition = "ˆÚ“®";
    [SerializeField, Header("õ“G¬Œ÷Žž‚Ì‘JˆÚ")]
    public string serchSuccessTransition = "’ÇÕ";
    [SerializeField, Header("”í’eŽž‚Ì‘JˆÚ")]
    public string damagedTransition = "”í’e";
    [SerializeField, Header("Õ“ËŽž‚Ì‘JˆÚ")]
    public string collisionTransition = "’ÇÕ";

    public override void Enter()
    {
        base.Enter();
    }

    public override void MainFunc()
    {
        if(enemy.IsFindPlayer)
        {
            Machine.TransitionTo(serchSuccessTransition);
        }

        if(machine.Cnt >= waitInterval)
        {
            Machine.TransitionTo(elapsedTransition);
        }
    }

    public override void Exit()
    {
        
    }

    public override void CollisionEnter(Collision collision)
    {
        if (collision.transform.root.name == "Player")
        {
            machine.TransitionTo(collisionTransition);
        }
    }

    public override void TriggerEnter(Collider collider)
    {
        if (collider.transform.root.name == "Player")
        {
            machine.TransitionTo(collisionTransition);
        }
    }
}
