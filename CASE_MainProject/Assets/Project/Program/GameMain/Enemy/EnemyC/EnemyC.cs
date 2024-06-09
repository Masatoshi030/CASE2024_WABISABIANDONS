using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : Enemy
{
    static protected GameObject cameraObject;

    public enum PressureState
    { 
        [InspectorName("�ሳ")]Low, 
        [InspectorName("����")]Med,
        [InspectorName("����")]High
    }

    [SerializeField, Header("���͏��"), Toolbar(typeof(PressureState))]
    protected PressureState pressureState = PressureState.Low;
    public PressureState CState { get => pressureState; set => pressureState = value;}

    // �󂯂��_���[�W�̕ۑ�
    float damageValue;
    public float DamageValue { get => damageValue; }


    private void Awake()
    {
        base.Awake();
        if (cameraObject == null)
            cameraObject = GameObject.Find("PlayerCamera_Brain");
    }

    protected void Start()
    {
        base.Start();
    }

    public override bool Damage(float damage, Vector3 direction)
    {
        Transform playerTransform = PlayerController.instance.transform;
        Vector3 dir = transform.position - playerTransform.position;
        damageVector = dir;

        // �J�����̌����ŕ�����ۑ�����p�^�[��
        damageVector = cameraObject.transform.forward;

        damageVector.y = 0.0f;
        damageVector.Normalize();
        damageValue = damage;
        isDamaged = true;
        return false;
    }
}
