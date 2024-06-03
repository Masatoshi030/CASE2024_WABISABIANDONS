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

    [SerializeField, Header("�\�����싗��")]
    float prepareDistance = 2.0f;
    [SerializeField, Header("�W�����v��")]
    float jumpPower = 15.0f;
    [SerializeField, Header("�O���W�����v��")]
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
        // ���ʂɃ��C���΂��ĕǂɏՓ˂��������擾����
        RaycastHit[] hits = Physics.RaycastAll(eyeTransform.position, eyeTransform.forward, prepareDistance);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.tag == "Ground")
            {
                Debug.Log("�Փ�");
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
