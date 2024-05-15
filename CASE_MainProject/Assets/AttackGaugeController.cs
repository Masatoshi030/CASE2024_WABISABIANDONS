using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackGaugeController : MonoBehaviour
{

    [SerializeField, Header("ƒQ[ƒWImageƒRƒ“ƒ|[ƒlƒ“ƒg")]
    Image gaugeImage;

    [SerializeField, Header("ƒQ[ƒW‚Ì’l")]
    float gaugeValue = 0.0f;

    [SerializeField, Header("ƒQ[ƒW‚ÌÅ‘å’l")]
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
        //‰ÁŽZ
        gaugeValue += _value;

        //Å‘å’l“ª‘Å‚¿
        if (gaugeValue > gaugeMaxValue)
        {
            //‘ã“ü
            gaugeValue = gaugeMaxValue;
        }

        //‰æ‘œ‚Ì’·‚³‚É‰e‹¿‚³‚¹‚é
        gaugeImage.fillAmount = gaugeValue / gaugeMaxValue;
    }

    public void SetValue(float _value)
    {
        //‰ÁŽZ
        gaugeValue = _value;

        //Å‘å’l“ª‘Å‚¿
        if (gaugeValue > gaugeMaxValue)
        {
            //‘ã“ü
            gaugeValue = gaugeMaxValue;
        }

        //‰æ‘œ‚Ì’·‚³‚É‰e‹¿‚³‚¹‚é
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
