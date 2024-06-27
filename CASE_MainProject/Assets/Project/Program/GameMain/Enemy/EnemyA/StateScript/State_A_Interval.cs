using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 
public class State_A_Interval : EnemyState
{
    [SerializeField, Header("‘Ò‚¿ŽžŠÔ")]
    float waitTime = 3.0f;

    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("Œo‰ßŽž‚Ì‘JˆÚ")]
    StateKey elapsedKey;
    [SerializeField, Header("ƒ_ƒ[ƒWŽž‚Ì‘JˆÚ")]
    StateKey damagedKey;

    public override void Enter()
    {
        base.Enter();
    }

    public override void MainFunc()
    {
        base.MainFunc();

        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedKey);
        }
        else if(machine.Cnt >= waitTime)
        {
            machine.TransitionTo(elapsedKey);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.IsVelocityZero = false;
    }

    public override void CollisionEnterSelf(Collision collision)
    {
        base.CollisionEnterSelf(collision);
        if(collision.transform.tag == "Ground")
        {
            
        }
        if (collision.transform.tag == "Enemy")
        {
            Vector3 direction = (collision.transform.position - enemy.transform.position).normalized;
            direction *= 10.0f;
            enemy.EnemyRigidbody.velocity = direction;
        }
    }
    public override void TriggerEnterSelf(Collider other)
    {
        base.TriggerEnterSelf(other);
        if (other.transform.tag == "Enemy")
        {
            enemy.EnemyRigidbody.velocity = other.transform.GetComponent<Enemy>().EnemyRigidbody.velocity * 2;
        }
    }
}
