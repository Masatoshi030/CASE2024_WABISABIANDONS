using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCameraSetting : MonoBehaviour
{

    public static CinemachineCameraSetting instance;

    [SerializeField, Header("少し引いたカメラ")]
    CinemachineVirtualCamera littlePulledCamera;

    [SerializeField, Header("引いたカメラを有効にしている時間")]
    float littlePulledCameraTimer = 0.0f;

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

        //バーチャルカメラ全てにプレイヤーのインスタンスを強制設定
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
