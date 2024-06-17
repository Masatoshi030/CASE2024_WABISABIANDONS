using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCameraSetting : MonoBehaviour
{
    private void Awake()
    {
        //�o�[�`�����J�����S�ĂɃv���C���[�̃C���X�^���X�������ݒ�
        for(int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<CinemachineVirtualCamera>().Follow = GameObject.Find("Player").transform;
        }
    }
}
