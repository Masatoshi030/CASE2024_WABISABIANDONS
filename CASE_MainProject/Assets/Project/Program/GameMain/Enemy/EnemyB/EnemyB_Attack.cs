using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB_Attack : EnemyState
{
    [SerializeField, Header("UÔu")]
    float attackInterval = 3.0f;
    [SerializeField, Header("UÌ¶¬IuWFNg")]
    GameObject attackObject;
    [SerializeField, Header("UIuWFNgÌ¬x")]
    float attackSpeed;
    [SerializeField, Header("UIuWFNgÌp[")]
    float attackPower;
    [SerializeField, Header("ñ]¬x")]
    float rotationSpeed = 0.8f;

    [Space(pad), Header("--JÚæXg--")]
    [SerializeField, Header("ìàÌJÚID")]
    public int elapsedID;
    [SerializeField, Header("íeÌJÚ")]
    public int damagedID;
    [SerializeField, Header("ÕËÌJÚ")]
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

        if (enemy.IsDamaged)
        {
            machine.TransitionTo(damagedID);
            return;
        }
        else if (machine.Cnt >= attackInterval)
        {
            GameObject attackObj = Instantiate(attackObject, enemy.transform.position, Quaternion.identity);
            attackObj.transform.forward = enemy.EyeTransform.forward;
            if(enemy.ToPlayerAngle < 20.0f)
            {
                attackObj.transform.LookAt(PlayerController.instance.transform.position);
            }
            attackObj.GetComponent<DrillComponent>().AttackPower = attackPower;
            attackObj.GetComponent<Rigidbody>().velocity = attackObj.transform.forward * attackSpeed;
            machine.TransitionTo(elapsedID);
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
