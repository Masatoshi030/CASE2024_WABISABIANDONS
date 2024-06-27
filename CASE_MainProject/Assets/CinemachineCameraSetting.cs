using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCameraSetting : MonoBehaviour
{

    public static CinemachineCameraSetting instance;

    [SerializeField, Header("�����������J����")]
    CinemachineVirtualCamera littlePulledCamera;

    [SerializeField, Header("�������J������L���ɂ��Ă��鎞��")]
    float littlePulledCameraTimer = 0.0f;

    private void Awake()
    {
        if (instance == null)
        {
            // ���g���C���X�^���X�Ƃ���
            instance = this;
        }
        else
        {
            // �C���X�^���X�����ɑ��݂��Ă����玩�g����������
            Destroy(gameObject);
        }

        //�o�[�`�����J�����S�ĂɃv���C���[�̃C���X�^���X�������ݒ�
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<CinemachineVirtualCamera>().Follow = GameObject.Find("Player").transform;
        }
    }

    private void Update()
    {
        if(littlePulledCameraTimer > 0.0f)
        {
            littlePulledCameraTimer -= Time.deltaTime;

            if(littlePulledCameraTimer < 0.0f)
            {
                littlePulledCameraTimer = 0.0f;
                littlePulledCamera.Priority = -1;
            }
        }
    }

    public void SetlittlePulledCamera(float _time)
    {
        littlePulledCameraTimer = _time;
        littlePulledCamera.Priority = 10;
    }
}
