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
            // 自身をインスタンスとする
            instance = this;
        }
        else
        {
            // インスタンスが既に存在していたら自身を消去する
            Destroy(gameObject);
        }

        //CinemachineVirtualCamera
        myCinemachineVirtualCamera = this.GetComponent<CinemachineVirtualCamera>();

        //CinemachinePOV
        myCinemachinePOV = myCinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();

        //初期値を代入
        povAimVerticalMaxSpeed = myCinemachinePOV.m_VerticalAxis.m_MaxSpeed;
        povAimHorizontalMaxSpeed = myCinemachinePOV.m_HorizontalAxis.m_MaxSpeed;
    }

    public void OnAim(float _verticalSpeed, float _horizontalSpeed)
    {
        //Vertical HorizontalそれぞれのAim速度を設定
        myCinemachinePOV.m_VerticalAxis.m_MaxSpeed = _verticalSpeed;
        myCinemachinePOV.m_HorizontalAxis.m_MaxSpeed = _horizontalSpeed;
    }

    public void OnAimReset()
    {
        //Vertical HorizontalそれぞれのAim速度をリセット
        myCinemachinePOV.m_VerticalAxis.m_MaxSpeed = povAimVerticalMaxSpeed;
        myCinemachinePOV.m_HorizontalAxis.m_MaxSpeed = povAimHorizontalMaxSpeed;
    }
}
