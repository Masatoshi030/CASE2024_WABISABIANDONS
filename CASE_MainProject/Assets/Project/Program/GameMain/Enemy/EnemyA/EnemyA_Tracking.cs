using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyA_Tracking : EnemyState
{
    [SerializeField, Header("NavMeshAgent")]
    NavMeshAgent agent;
    [SerializeField, Header("RigidBody")]
    Rigidbody rb;

    [SerializeField, Header("予備動作距離")]
    float prepareDistance = 2.0f;
    [SerializeField, Header("ジャンプ力")]
    float jumpPower = 15.0f;
    [SerializeField, Header("前方ジャンプ力")]
    float forwardJumpPower = 5.0f;
    Transform eyeTransform;


    public override void Initialize()
    {
        agent = enemy.GetComponent<NavMeshAgent>();
        rb = enemy.GetComponent<Rigidbody>();
        eyeTransform = enemy.EyeTransform;
    }
    public override void MainFunc()
    {
        agent.SetDestination(Enemy.Target.transform.position);
        // 正面にレイを飛ばして壁に衝突しそうか取得する
        RaycastHit[] hits = Physics.RaycastAll(eyeTransform.position, eyeTransform.forward, prepareDistance);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.tag == "Ground")
            {
                Debug.Log("衝突");
                Vector3 normal = hits[i].normal;
                float Dot = Vector3.Dot(eyeTransform.forward, normal);
                Debug.Log(Dot);
                if(Dot < -0.3f)
                {
                    agent.enabled = false;
                    Vector3 forceUp = enemy.transform.up * jumpPower;
                    Vector3 forceForward = enemy.transform.forward * forwardJumpPower;
                    rb.velocity = forceForward;
                    rb.AddForce(forceUp, ForceMode.Impulse);
                }
            }
        }
    }
}
