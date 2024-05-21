using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_ValveRotate : GimmickState
{
    [SerializeField, Header("��]������I�u�W�F�N�g")]
    GameObject rotateObj;
    [SerializeField, Header("�ړ�������I�u�W�F�N�g")]
    GameObject moveObj;
    [SerializeField, Header("��]��"), VectorRange(-1.0f, 1.0f)]
    Vector3 rotateAxis;
    [SerializeField, Header("�������x")]
    float initSpeed;
    [SerializeField, Header("�쓮����")]
    float runningTime;
    [SerializeField, Header("�b�Ԍ����l"), ReadOnly]
    float brakeValue;
    [SerializeField, Header("���ݑ��x"), ReadOnly]
    float currentSpeed;
    [SerializeField, Header("�ړ�����I�u�W�F�N�g�̏����ʒu"), ReadOnly]
    Vector3 initObjectPosition;
    [SerializeField, Header("�ړ�����"), VectorRange(-1.0f, 1.0f)]
    Vector3 moveVelocity;
    [SerializeField, Header("�I�u�W�F�N�g�̈ړ��l")]
    float moveValue;
    [SerializeField, Header("�ړ�����~�܂łɂ����鎞��")]
    float initTime;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("���Ԍo�ߌ�̑J��")]
    string elapsedTransition;

    public override void Enter()
    {
        base.Enter();
        currentSpeed = initSpeed;
        brakeValue = initSpeed / runningTime;
    }

    public override void MainFunc()
    {
        if(currentSpeed <= 0)
        {
            machine.TransitionTo(elapsedTransition);
        }
        rotateObj.transform.Rotate(rotateAxis, currentSpeed * Time.deltaTime);
        currentSpeed = currentSpeed - brakeValue * Time.deltaTime;
    }
}
