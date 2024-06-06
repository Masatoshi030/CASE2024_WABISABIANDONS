using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : Enemy
{
    static protected GameObject cameraObject;

    public enum PressureState
    { 
        [InspectorName("��")]Empty, 
        [InspectorName("����")]Medium,
        [InspectorName("�p���p��")]Full
    }

    [SerializeField, Header("���͏��"), Toolbar(typeof(PressureState))]
    protected PressureState pressureState = PressureState.Empty;
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

    protected void Update()
    {
        if(isSearchPlayer)
        {
            (bool find, float dis) = FindPlayerAtFOV();
            isFindPlayer = find;
            toPlayerDistance = dis;
        }
        enemyStateMachine.MainFunc();
    }

    public override bool Damage(float damage, Vector3 direction)
    {
        // �v���C���[�Ƃ̃|�W�V�����̍����ŕ�����ۑ�����p�^�[��

        // �V���v���Ƀ|�W�V�����̍����v�Z
        Transform tra = PlayerController.instance.transform;
        Vector3 dir = transform.position - tra.position;
        damageVector = dir;

        //// �Փ˓_�̌��m
        //Ray ray = new Ray(tra.position, tra.forward);
        //RaycastHit[] hits = Physics.RaycastAll(ray, 10.0f);
        //for(int i = 0; i < hits.Length;i++)
        //{
        //    if (hits[i].collider.gameObject == gameObject)
        //    {
        //        Debug.Log("player");
        //        Vector3 hitPoint = hits[i].point;
        //        damageVector = transform.position - hitPoint;
        //        damageVector.y = 0.0f;
        //        break;
        //    }
        //    if (i== hits.Length - 1)
        //    {
        //        damageVector = dir;
        //    }
        //}

        // �J�����̌����ŕ�����ۑ�����p�^�[��
        //damageVector = cameraObject.transform.forward;
        //damageVector.y = 0.0f;
        damageVector.Normalize();
        damageValue = damage;
        isDamaged = true;
        return false;
    }
}
