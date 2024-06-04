using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Idle : EnemyState
{
    [SerializeField, Header("‘Ò‹@ŽžŠÔ")]
    float waitInterval = 3.0f;
    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("ŽžŠÔŒo‰ßŒã‚Ì‘JˆÚ")]
    public int elapsedID;
    [SerializeField, Header("õ“G¬Œ÷Žž‚Ì‘JˆÚ")]
    public int searchSuccessID;
    [SerializeField, Header("”í’eŽž‚Ì‘JˆÚ")]
    public int damagedID;
    [SerializeField, Header("Õ“ËŽž‚Ì‘JˆÚ")]
    public int collisionID;

    public override void Enter()
    {
        base.Enter();
        enemy.EnemyRigidbody.velocity = Vector3.zero;
    }

    public override void MainFunc()
    {
        if(enemy.IsFindPlayer)
        {
            machine.TransitionTo(searchSuccessID);
        }
        else if(enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedID);
        }
        else if(machine.Cnt >= waitInterval)
        {
            machine.TransitionTo(elapsedID);
        }
    }

    public override void Exit()
    {
        
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
