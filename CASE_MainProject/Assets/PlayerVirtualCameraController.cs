using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVirtualCameraController : MonoBehaviour
{
    public static PlayerVirtualCameraController instance;

    CinemachineVirtualCamera myCinemachineVirtualCamera;

    CinemachinePOV myCinemachinePOV;

    float povAimVerticalMaxSpeed;
    float povAimHorizontalMaxSpeed;

    private void Start()
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

        //CinemachineVirtualCamera
        myCinemachineVirtualCamera = this.GetComponent<CinemachineVirtualCamera>();

        //CinemachinePOV
        myCinemachinePOV = myCinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();

        //�����l����
        povAimVerticalMaxSpeed = myCinemachinePOV.m_VerticalAxis.m_MaxSpeed;
        povAimHorizontalMaxSpeed = myCinemachinePOV.m_HorizontalAxis.m_MaxSpeed;
    }

    public void OnAim(float _verticalSpeed, float _horizontalSpeed)
    {
        //Vertical Horizontal���ꂼ���Aim���x��ݒ�
        myCinemachinePOV.m_VerticalAxis.m_MaxSpeed = _verticalSpeed;
        myCinemachinePOV.m_HorizontalAxis.m_MaxSpeed = _horizontalSpeed;
    }

    public void OnAimReset()
    {
        //Vertical Horizontal���ꂼ���Aim���x�����Z�b�g
        myCinemachinePOV.m_VerticalAxis.m_MaxSpeed = povAimVerticalMaxSpeed;
        myCinemachinePOV.m_HorizontalAxis.m_MaxSpeed = povAimHorizontalMaxSpeed;
    }
}
