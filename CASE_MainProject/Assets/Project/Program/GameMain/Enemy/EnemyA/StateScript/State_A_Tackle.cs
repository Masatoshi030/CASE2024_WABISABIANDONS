using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_A_Tackle : EnemyState
{
    [SerializeField, Header("タックルの強さ(velocity)")]
    float tackleVelocity = 10.0f;
    [SerializeField, Header("タックルの強さ(damage)")]
    float tacklePower = 10.0f;
    [SerializeField, Header("高さをプレイヤーに合わせるか")]
    bool isCalcPlayerHeight = false;
    [SerializeField, Header("タックルの上方向の強さ")]
    float tackleUpVelocity = 1.0f;
    [SerializeField, Header("タックル後の硬直時間")]
    float stiffnessTime = 3.0f;
    [SerializeField, Header("タックル中か"), ReadOnly]
    bool bTackle = false;
    float subCnt = 0.0f;

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("衝突時の遷移")]
    StateKey tackledKey;
    [SerializeField, Header("ダメージ時の遷移")]
    StateKey damagedKey;

    public override void Enter()
    {
        base.Enter();
        enemy.IsVelocityZero = false;
        // y軸以外の成分を消す
        Quaternion yOnly = enemy.transform.rotation;
        yOnly.x = 0.0f;
        yOnly.z = 0.0f;
        enemy.transform.rotation = yOnly;
        // タックル方向
        Vector3 velocity;
        if (isCalcPlayerHeight)
        {
            velocity = (Enemy.Target.transform.position - enemy.transform.position).normalized;
            velocity *= tackleVelocity;
            velocity.y += 3.0f;
        }
        else
        {
            velocity = enemy.transform.forward;
            velocity.x *= tackleVelocity;
            velocity.z *= tackleVelocity;
            velocity.y = tackleUpVelocity;
            
        }
        enemy.EnemyRigidbody.velocity = velocity;
        enemy.EnemyCollider.isTrigger = true;
        bTackle = true;
        subCnt = 0.0f;
    }

    public override void MainFunc()
    {
        base.MainFunc();

        if (!bTackle) subCnt += Time.deltaTime;
        if(!bTackle && enemy.IsDamaged)
        {
            machine.TransitionTo(damagedKey);
        }
        else if(bTackle)
        {
            if (enemy.EnemyRigidbody.velocity.magnitude <= 1.0f)
            {
                bTackle = false;
                enemy.IsVelocityZero = true;
            }
        }

        if(!bTackle)
        {
            Ray ray = new Ray(enemy.EyeTransform.position, Vector3.down);
            RaycastHit[] hits = Physics.RaycastAll(ray, 3.0f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.transform.tag == "Ground")
                {
                    machine.TransitionTo(tackledKey);
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        bTackle = false;
    }

    public override void CollisionStaySelf(Collision collision)
    {
        base.CollisionEnterSelf(collision);

        if (collision.transform.tag == "Ground")
        {
            Ray ray = new Ray(enemy.transform.position, Vector3.down);
            RaycastHit[] hits = Physics.RaycastAll(ray, 2.0f);
            foreach (RaycastHit hit in hits)
            {
                if(hit.collider.gameObject == collision.gameObject)
                {
                    machine.TransitionTo(tackledKey);
                }
            }
        }
    }

    public override void TriggerEnterSelf(Collider other)
    {
        base.TriggerEnterSelf(other);
        if(bTackle)
        {
            if(other.transform.tag == "Player")
            {
                if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                {
                    enemy.EnemyCollider.isTrigger = false;
                    bTackle = false;
                }
                else
                {
                    enemy.EnemyCollider.isTrigger = false;
                    PlayerController.instance.Damage(tacklePower);
                    bTackle = false;
                }
            }
            if (other.transform.tag == "Wall" || other.transform.tag == "Ground")
            {
                bTackle = false;
                enemy.EnemyRigidbody.velocity = Vector3.zero;
                enemy.EnemyCollider.isTrigger = false;
            }
        }
        else
        {
            if (other.transform.tag == "Wall" || other.transform.tag == "Ground")
            {
                enemy.EnemyCollider.isTrigger = false;
                enemy.EnemyRigidbody.velocity = Vector3.zero;
            }
        }
    }
}
