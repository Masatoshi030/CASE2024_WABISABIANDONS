using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyB_Attack : EnemyState
{
    [SerializeField, Header("攻撃間隔")]
    float attackInterval = 3.0f;
    [SerializeField, Header("攻撃時の生成オブジェクト")]
    GameObject attackObject;
    [SerializeField, Header("攻撃オブジェクトの速度")]
    float attackSpeed;
    [SerializeField, Header("攻撃オブジェクトのパワー")]
    float attackPower;
    [SerializeField, Header("回転速度")]
    float rotationSpeed = 0.8f;

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("視野内の遷移ID")]
    public int elapsedID;
    [SerializeField, Header("被弾時の遷移")]
    public int damagedID;
    [SerializeField, Header("衝突時の遷移")]
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
            if(Mathf.Abs(enemy.ToPlayerAngle) < 20.0f)
            {
                attackObj.transform.LookAt(PlayerController.instance.transform.position);
            }
            // 上下のVelocityだけ合わせる
            float verticalVelocity = PlayerController.instance.transform.position.y - attackObj.transform.position.y;
            attackObj.GetComponent<DrillComponent>().AttackPower = attackPower;
            Vector3 velocity = attackObj.transform.forward * attackSpeed;
            velocity.y = verticalVelocity;
            attackObj.GetComponent<Rigidbody>().velocity = velocity;
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
