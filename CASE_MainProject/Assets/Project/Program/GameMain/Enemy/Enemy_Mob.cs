using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Mob : Enemy_Parent
{
    public enum State
    {
        [InspectorName("�ҋ@")] Idle,
        [InspectorName("�ړ�")] Move,
        [InspectorName("�ǔ�")] Tracking,
        [InspectorName("����")] Escape,
        [InspectorName("�U��A")] AttackA,
        [InspectorName("�U��B")] AttackB,
        [InspectorName("����A")]SpecialA,
        [InspectorName("����B")]SpecialB,
        [InspectorName("��")] Heal,
        [InspectorName("�j��")] Death,
    }

    [Space(padA), Header("--���F�p�����[�^--")]
    [SerializeField, Header("�G���F����")]
    protected float viewingDistance = 20.0f;

    [SerializeField, Header("����p")]
    protected float viewingAngle = 80.0f;

    [SerializeField, Header("���_�ʒu")]
    protected Transform eyeTransform;

    [Space(padB), SerializeField, Header("���"), Toolbar(typeof(State))]
    protected State state = State.Idle;
    
    protected void Start()
    {
        if(eyeTransform == null)
        {
            eyeTransform = transform.Find("EyeTransform");
        }
        base.Start();
    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
        switch (state)
        {
            case State.Idle:IdleFunc(); break;
            case State.Move: MoveFunc(); break;
            case State.Tracking: TrackingFunc(); break;
            case State.Escape: EscapeFunc(); break;
            case State.AttackA: AttackFuncA(); break;
            case State.AttackB: AttackFuncB(); break;
            case State.SpecialA: SpecialFuncA(); break;
            case State.SpecialB: SpecialFuncB(); break;
            case State.Heal: HealFunc(); break;
            case State.Death: DeathFunc(); break;
        }
    }

    /*
     * <summary>
     * �ҋ@��Ԋ֐�
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected virtual void IdleFunc() { }

    /*
     * <summary>
     * �ړ���Ԋ֐�
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected virtual void MoveFunc() { }

    /*
     * <summary>
     * �ǔ���Ԋ֐�
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected virtual void TrackingFunc() { }

    /*
     * <summary>
     * ������Ԋ֐�
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected virtual void EscapeFunc() { }

    /*
     * <summary>
     * �U���֐��p�^�[��A
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected virtual void AttackFuncA() { }

    /*
     * <summary>
     * �U���֐��p�^�[��B
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected virtual void AttackFuncB() { }

    /*
     * <summary>
     * ����֐��p�^�[��A
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected virtual void SpecialFuncA() { }

    /*
     * <summary>
     * ����֐��p�^�[��B
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected virtual void SpecialFuncB() { }

    /*
     * <summary>
     * �񕜏�Ԋ֐�
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected virtual void HealFunc() { }

    /*
     * <summary>
     * ���S��Ԋ֐�
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected virtual void DeathFunc() { }

    protected override void DestroyFunc()
    {

    }

    /*
     * <summary>
     * �v���C���[������p�����ɒT������
     * <param>
     * �Ȃ�
     * <returns>
     * bool isFind, float distance
     */
    protected (bool isFind, float distance) FindPlayerAtFOV()
    {
        // �����𑪂�
        Vector3 Diff = target.transform.position - eyeTransform.position;
        float distance = Diff.x * Diff.x + Diff.y * Diff.y + Diff.z * Diff.z;

        if (distance < viewingDistance * viewingDistance)
        {
            // �O�ς�y�����琳�ʂ̍��E�ǂ���ɂ��邩���߂�
            Vector3 axis = Vector3.Cross(eyeTransform.forward, Diff);

            // �p�x��+�Ȃ琳�ʂ��E�ɂ���-�Ȃ獶�ɂ���
            float angle = Vector3.Angle(eyeTransform.forward, Diff) * (axis.y < 0 ? -1.0f : 1.0f);

            if (Mathf.Abs(angle) < viewingAngle / 2)
            {
                Vector3 Direction = Diff.normalized;
                // ���C�̍쐬�ƕ\��
                Ray ray = new Ray(eyeTransform.position, Direction);
                Debug.DrawRay(eyeTransform.position, Direction, Color.red);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, viewingDistance))
                {
                    Debug.Log(hit.transform.name);

                    if (hit.transform.root.name == "Player")
                    {
                        return (true, distance);
                    }
                }
            }
        }
        return (false, distance);
    }

    /*
    * <summary>
    * �I�u�W�F�N�g�T���֐�
    * <param>
    * GameObject[] objects
    * <return>
    * bool isFind, float distance, GameObject findObject
    */
    protected (bool isFind, float distance, GameObject findObject) FindObjectAtFOV(GameObject[] objects)
    {
        // �����𑪂�
        Vector3 Diff = target.transform.position - eyeTransform.position;
        float distance = Diff.x * Diff.x + Diff.y * Diff.y + Diff.z * Diff.z;

        if (distance < viewingDistance)
        {
            // �O�ς�y�����琳�ʂ̍��E�ǂ���ɂ��邩���߂�
            Vector3 axis = Vector3.Cross(eyeTransform.forward, Diff);

            // �p�x��+�Ȃ琳�ʂ��E�ɂ���-�Ȃ獶�ɂ���
            float angle = Vector3.Angle(eyeTransform.forward, Diff) * (axis.y < 0 ? -1.0f : 1.0f);

            if (Mathf.Abs(angle) < viewingAngle / 2)
            {
                Vector3 Direction = Diff.normalized;
                // ���C�̍쐬�ƕ\��
                Ray ray = new Ray(eyeTransform.position, Direction);
                Debug.DrawRay(eyeTransform.position, Direction, Color.red);
                RaycastHit[] hits = Physics.RaycastAll(eyeTransform.position, Direction, viewingDistance);
                if (hits.Length > 0)
                {
                    for(int i = 0; i < objects.Length; i++)
                    {
                        if (hits[0].transform.gameObject == objects[i])
                        {
                            return (true, distance, objects[i]);
                        }
                    }
                }
            }
        }
        return (false, distance, null);
    }

    /*
    * <summary>
    * �I�u�W�F�N�g�T���֐�
    * <param>
    * string[] tags
    * <return>
    * bool isFind, float distance, string findTag
    */
    protected (bool isFind, float distance, string findTag) FindObjectAtFOV(string[] tags)
    {
        // �����𑪂�
        Vector3 Diff = target.transform.position - eyeTransform.position;
        float distance = Diff.x * Diff.x + Diff.y * Diff.y + Diff.z * Diff.z;

        if (distance < viewingDistance)
        {
            // �O�ς�y�����琳�ʂ̍��E�ǂ���ɂ��邩���߂�
            Vector3 axis = Vector3.Cross(eyeTransform.forward, Diff);

            // �p�x��+�Ȃ琳�ʂ��E�ɂ���-�Ȃ獶�ɂ���
            float angle = Vector3.Angle(eyeTransform.forward, Diff) * (axis.y < 0 ? -1.0f : 1.0f);

            if (Mathf.Abs(angle) < viewingAngle / 2)
            {
                Vector3 Direction = Diff.normalized;
                // ���C�̍쐬�ƕ\��
                Ray ray = new Ray(eyeTransform.position, Direction);
                Debug.DrawRay(eyeTransform.position, Direction, Color.red);
                RaycastHit[] hits = Physics.RaycastAll(eyeTransform.position, Direction, viewingDistance);
                if (hits.Length > 0)
                {
                    for (int i = 0; i < tags.Length; i++)
                    {
                        if (hits[0].transform.tag == tags[i])
                        {
                            return (true, distance, tags[i]);
                        }
                    }
                }
            }
        }
        return (false, distance, null);
    }

    /*
    * <summary>
    * ��Ԕ��f�֐�(���z)
    * <param>
    * void
    * <return>
    * void
    */
    protected virtual void JudgeState()
    {
        // �X�e�[�g�̃W���b�W(�A�j���[�V�����̏I�����ɒ@������)
    }

    /*
    * <summary>
    * ��Ԑݒ�֐�
    * <param>
    * Enemy_Mob.State state
    * <return>
    * void
    */
    public void SetState(State state)
    {
        this.state = state;
    }
}
