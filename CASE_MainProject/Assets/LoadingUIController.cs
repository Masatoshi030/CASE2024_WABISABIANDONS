using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUIController : MonoBehaviour
{
    [SerializeField, Header("マークのスプライトリスト")]
    Sprite[] markSpriteList;

    [SerializeField, Header("マークのImageコンポーネントリスト")]
    Image[] markImageComponentList;

    private void Awake()
    {
        for (int i = 0; i < markImageComponentList.Length; i++)
        {
            //非表示
            markImageComponentList[i].enabled = false;
        }
    }

    public void SetActiveMark(int _idx)
    {
        if (_idx >= 0 && _idx < markImageComponentList.Length)
        {
            for (int i = 0; i <= _idx; i++)
            {
                if (markImageComponentList[i].enabled == false)
                {
                    //ランダムなスプライト変更
                    markImageComponentList[i].sprite = markSpriteList[Random.Range(0, markSpriteList.Length - 1)];

                    //ランダムな回転
                    markImageComponentList[i].rectTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 180.0f));

                    //表示
                    markImageComponentList[i].enabled = true;
                }
            }
        }
    }

}
