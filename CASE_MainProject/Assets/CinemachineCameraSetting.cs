using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCameraSetting : MonoBehaviour
{

    public static CinemachineCameraSetting instance;

    CinemachineVirtualCamera mainVirtualCamera;

    float mainCameraFieldOfView = 60.0f;

    [SerializeField, Header("少し引いたカメラ視野角")]
    float pullCameraFieldOfView = 90.0f;

    [SerializeField, Header("引いたカメラを有効にしている時間")]
    float pulledCameraTimer = 0.0f;

    private void Awake()
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

        //メインバーチャルカメラを取得
        mainVirtualCamera = this.transform.GetChild(1).GetComponent<CinemachineVirtualCamera>();

        //メインバーチャルカメラの視野角を取得
        mainCameraFieldOfView = mainVirtualCamera.m_Lens.FieldOfView;

        //バーチャルカメラ全てにプレイヤーのインスタンスを強制設定
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<CinemachineVirtualCamera>().Follow = GameObject.Find("Player").transform;
        }
    }

    private void Update()
    {
        if(pulledCameraTimer > 0.0f)
        {
            pulledCameraTimer -= Time.deltaTime;

            if(pulledCameraTimer < 0.0f)
            {
                mainVirtualCamera.m_Lens.FieldOfView = mainCameraFieldOfView;
            }
        }
    }

    public void SetPulledCamera(float _time)
    {
        pulledCameraTimer = _time;
        mainVirtualCamera.m_Lens.FieldOfView = pullCameraFieldOfView;
    }
}
