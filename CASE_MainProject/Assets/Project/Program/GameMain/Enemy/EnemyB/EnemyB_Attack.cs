using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB_Attack : EnemyState
{
    [SerializeField, Header("UŒ‚ŠÔŠu")]
    float attackDuration = 3.0f;
    [SerializeField, Header("UŒ‚‚Ì¶¬ƒIƒuƒWƒFƒNƒg")]
    GameObject attackObject;
    [SerializeField, Header("UŒ‚ƒIƒuƒWƒFƒNƒg‚Ì‘¬“x")]
    float attackPower;

    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("’ÇÕ¸”s‚Ì‘JˆÚ")]
    public int failedID;
    [SerializeField, Header("‹——£ˆê’èˆÈ“à‚Ì‘JˆÚ")]
    public int successfuID;
    [SerializeField, Header("”í’e‚Ì‘JˆÚ")]
    public int damagedID;
    [SerializeField, Header("Õ“Ë‚Ì‘JˆÚ")]
    public int collisionID;

    public override void Initialize()
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        enemy.EnemyRigidbody.velocity = Vector3.zero;
    }

    public override void MainFunc()
    {
        
    }

    public override void Exit()
    {

    }

    public override void CollisionEnterOpponent(Collision collision)
    {
        if (collision.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedID);
            else
                machine.TransitionTo(collisionID);
        }
    }

    public override void TriggerEnterOpponent(Collider other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedID);
            else
                machine.TransitionTo(collisionID);
        }
    }
}
