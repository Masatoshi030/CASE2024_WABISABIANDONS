using UnityEngine;
using UnityEngine.AI;

public class NavMeshPatrol : MonoBehaviour
{
    public enum PatrolState
    { 
        [InspectorName("�ҋ@")]Idle, 
        [InspectorName("����")]Patrol, 
        [InspectorName("�J�X�^��")]Custom
    };

    [SerializeField, Header("�ړI�n�̐e")]
    Transform targetParent;
    [SerializeField, Header("�ړI�n���X�g")]
    Transform[] patrols;
    [SerializeField, Header("���"), Toolbar(typeof(PatrolState))]
    PatrolState state = PatrolState.Idle;
    [SerializeField, Header("�J�X�^�����ꂽ�ʒu")]
    Vector3 customedPosition;

    NavMeshAgent agent;

    private void Awake()
    {
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
        agent = GetComponent<NavMeshAgent>();
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
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.ResetPath();
            state = PatrolState.Idle;
        }
    }

    void CustomFunc()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.ResetPath();
        }
    }

    /*
     * <summary>
     * �G�[�W�F���g�̃p�����[�^�Z�b�g
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

    /*
     * <summary>
     * ��Ԃ�����ɂ���
     * <param>
     * int index...�ړI�n�̓Y����
     * <return>
     * void
     */
    public void ExcutePatrol(int index)
    {
        if(index >= patrols.Length)
        {
            index = 0;
        }
        state = PatrolState.Patrol;
        agent.SetDestination(patrols[index].position);
    }

    public void ExcuteCustom(Vector3 target)
    {
        state = PatrolState.Custom;
        agent.SetDestination(target);
    }

    /*
     * <summary>
     * ��Ԃ̎擾
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
}