using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCameraSetting : MonoBehaviour
{
    private void Awake()
    {
        //バーチャルカメラ全てにプレイヤーのインスタンスを強制設定
        for(int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<CinemachineVirtualCamera>().Follow = GameObject.Find("Player").transform;
        }
    }
}
