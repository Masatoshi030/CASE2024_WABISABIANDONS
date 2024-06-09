using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB_Attack : EnemyState
{
    [SerializeField, Header("UŒ‚ŠÔŠu")]
    float attackInterval = 3.0f;
    [SerializeField, Header("UŒ‚Žž‚Ì¶¬ƒIƒuƒWƒFƒNƒg")]
    GameObject attackObject;
    [SerializeField, Header("UŒ‚ƒIƒuƒWƒFƒNƒg‚Ì‘¬“x")]
    float attackPower;
    [SerializeField, Header("‰ñ“]‘¬“x")]
    float rotationSpeed = 0.8f;

    [Space(pad), Header("--‘JˆÚæƒŠƒXƒg--")]
    [SerializeField, Header("Ž‹–ì“à‚Ì‘JˆÚID")]
    public int successID;
    [SerializeField, Header("Ž‹–ìŠO‚Ì‘JˆÚID")]
    public int failedID;
    [SerializeField, Header("”í’eŽž‚Ì‘JˆÚ")]
    public int damagedID;
    [SerializeField, Header("Õ“ËŽž‚Ì‘JˆÚ")]
    public int collisionID;

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Enter()
    {
        base.Enter();

        enemy.EnemyRigidbody.velocity = Vector3.zero;
    }

    public override void MainFunc()
    {
        base.MainFunc();

        float angleY = enemy.transform.rotation.y;
        float angle = Mathf.Lerp(0.0f, enemy.ToPlayerAngle, rotationSpeed);

        enemy.transform.Rotate(0.0f, angle * Time.deltaTime, 0.0f);

        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedID);
            return;
        }

        if(machine.Cnt >= attackInterval)
        {
            if(enemy.IsFindPlayer)
            {
                machine.TransitionTo(successID);
            }
            else
            {
                machine.TransitionTo(failedID);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void CollisionEnterOpponent(Collision collision)
    {
        if (collision.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState != PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(collisionID);
        }
    }

    public override void TriggerEnterOpponent(Collider other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState != PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(collisionID);
        }
    }
}
