using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_IdleNoSearch : EnemyState
{
    [SerializeField, Header("‘Ò‹@ŽžŠÔ")]
    float waitInterval;
    [SerializeField, Header("Œo‰ßŒã‚Ì‘JˆÚ")]
    public string elapsedTransition = "ˆÚ“®";
    [SerializeField, Header("”í’eŽž‚Ì‘JˆÚ")]
    public string damagedTransition = "”í’e";

    public override void Enter()
    {
        base.Enter();
        enemy.IsSearchPlayer = false;
    }

    public override void MainFunc()
    {
        if (machine.Cnt >= waitInterval) machine.TransitionTo(elapsedTransition);
    }

    public override void Exit()
    {
        enemy.IsSearchPlayer = true;
    }

    public override void CollisionEnter(Collision collision)
    {
        if (collision.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack) machine.TransitionTo(damagedTransition);
        }
    }

    public override void TriggerEnter(Collider collider)
    {
        if (collider.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack) machine.TransitionTo(damagedTransition);
        }
    }
}
