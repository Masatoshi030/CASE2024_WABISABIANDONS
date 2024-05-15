using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackGaugeController : MonoBehaviour
{

    [SerializeField, Header("�Q�[�WImage�R���|�[�l���g")]
    Image gaugeImage;

    [SerializeField, Header("�Q�[�W�̒l")]
    float gaugeValue = 0.0f;

    [SerializeField, Header("�Q�[�W�̍ő�l")]
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
        //���Z
        gaugeValue += _value;

        //�ő�l���ł�
        if (gaugeValue > gaugeMaxValue)
        {
            //���
            gaugeValue = gaugeMaxValue;
        }

        //�摜�̒����ɉe��������
        gaugeImage.fillAmount = gaugeValue / gaugeMaxValue;
    }

    public void SetValue(float _value)
    {
        //���Z
        gaugeValue = _value;

        //�ő�l���ł�
        if (gaugeValue > gaugeMaxValue)
        {
            //���
            gaugeValue = gaugeMaxValue;
        }

        //�摜�̒����ɉe��������
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
