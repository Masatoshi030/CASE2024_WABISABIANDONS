using System;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshPatrol : MonoBehaviour
{
    public enum PatrolState
    { 
        [InspectorName("待機")]Idle, 
        [InspectorName("巡回")]Patrol, 
        [InspectorName("カスタム")]Custom
    };

    [SerializeField, Header("目的地の親")]
    Transform targetParent;
    [SerializeField, Header("目的地リスト")]
    Transform[] patrols;
    [SerializeField, Header("状態"), Toolbar(typeof(PatrolState))]
    PatrolState state = PatrolState.Idle;
    [SerializeField, Header("カスタムされた位置")]
    Vector3 customedPosition;

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (targetParent)
        {
            patrols = new Transform[targetParent.childCount];
            for (int i = 0; i < targetParent.childCount; i++)
            {
                Transform trs = targetParent.GetChild(i);
                patrols[i] = trs;
            }
        }
    }

    void Start()
    {
        
    }

    private void Update()
    {
        switch (state)
        {
            case PatrolState.Idle: break;
            case PatrolState.Patrol: PatrolFunc(); break;
            case PatrolState.Custom: CustomFunc(); break;
        }
    }

    void PatrolFunc()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Stop();
        }
    }

    void CustomFunc()
    {
        if (agent.remainingDistance <= agent.stoppingDistance &&!agent.pathPending)
        {
            Stop();
        }
    }

    /*
     * <summary>
     * エージェントのパラメータセット
     * <param>
     * float speed, float acceleraion, float angular, bool autoBrake
     * <return>
     * void
     */
    public void SetAgentParam(float speed, float acceleration, bool autoBrake = false)
    {
        agent.speed = speed > 0.1f ? speed : 0.1f;
        agent.acceleration = acceleration > 0.1f ? acceleration : 0.1f;
        agent.autoBraking = autoBrake;
    }

    public void SetAgentParam(float speed, float acceleration, float angularSpeed, bool autoBrake = false)
    {
        agent.speed = speed > 0.1f ? speed : 0.1f;
        agent.acceleration = acceleration > 0.1f ? acceleration : 0.1f;
        agent.angularSpeed = angularSpeed;
        agent.autoBraking = autoBrake;
    }

    /*
     * <summary>
     * 状態を巡回にする
     * <param>
     * int index...目的地の添え字
     * <return>
     * void
     */
    public bool ExcutePatrol(int index)
    {
        if(index >= patrols.Length)
        {
            index = 0;
        }
        state = PatrolState.Patrol;
        return agent.SetDestination(patrols[index].position);
    }

    public bool ExcuteCustom(Vector3 target)
    {
        state = PatrolState.Custom;
        return agent.SetDestination(target);
    }

    /*
     * <summary>
     * 状態の取得
     * <param>
     * void
     * <return>
     * PatrolState
     */
    public PatrolState GetPatrolState()
    {
        return state;
    }

    public Transform[] GetTargets()
    {
        return patrols;
    }

    public void Stop()
    {
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        state = PatrolState.Idle;
    }

    public float GetRemainingDistance()
    {
        return agent.remainingDistance;
    }
}