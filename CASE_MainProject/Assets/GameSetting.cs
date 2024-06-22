using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class GameSetting : MonoBehaviour
{
    public static GameSetting instance { get; private set; }

    [SerializeField, Header("可燃ガス有効")]
    public bool bCombustibleGasEnable = false;

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
    }
}
