using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackGaugeController : MonoBehaviour
{

    [SerializeField, Header("ゲージImageコンポーネント")]
    Image gaugeImage;

    [SerializeField, Header("ゲージの値")]
    float gaugeValue = 0.0f;

    [SerializeField, Header("ゲージの最大値")]
    float gaugeMaxValue = 100.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddValue(float _value)
    {
        //加算
        gaugeValue += _value;

        //最大値頭打ち
        if (gaugeValue > gaugeMaxValue)
        {
            //代入
            gaugeValue = gaugeMaxValue;
        }

        //画像の長さに影響させる
        gaugeImage.fillAmount = gaugeValue / gaugeMaxValue;
    }

    public void SetValue(float _value)
    {
        //加算
        gaugeValue = _value;

        //最大値頭打ち
        if (gaugeValue > gaugeMaxValue)
        {
            //代入
            gaugeValue = gaugeMaxValue;
        }

        //画像の長さに影響させる
        gaugeImage.fillAmount = gaugeValue / gaugeMaxValue;
    }

    public float GetValue()
    {
        return gaugeValue;
    }

    public float GetValue_Normalize()
    {
        return gaugeValue / gaugeMaxValue;
    }
}
