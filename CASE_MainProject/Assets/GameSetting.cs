using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class GameSetting : MonoBehaviour
{
    public static GameSetting instance { get; private set; }

    [SerializeField, Header("�R�K�X�L��")]
    public bool bCombustibleGasEnable = false;

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
    }
}
