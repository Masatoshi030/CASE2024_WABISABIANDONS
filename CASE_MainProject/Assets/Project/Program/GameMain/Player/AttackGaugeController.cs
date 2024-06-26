using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackGaugeController : MonoBehaviour
{

    [SerializeField, Header("ゲージImageコンポーネント")]
    Image gageImage;

    [SerializeField, Header("ゲージの値")]
    float gageValue = 0.0f;

    [SerializeField, Header("ゲージの最大値")]
    float gaugeMaxValue = 100.0f;

    public enum ATTACK_GAGE_STEP
    { Step1, Step2, Step3, MaxStep };
    [SerializeField, Header("ゲージの段階"), Toolbar(typeof(ATTACK_GAGE_STEP), "AttackGageStep")]
    public ATTACK_GAGE_STEP attackGageStep = ATTACK_GAGE_STEP.Step1;

    [SerializeField, Header("ゲージの段階マテリアルリスト")]
    Material[] gageStepMaterialList;

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
        gageValue += _value;

        //最大値頭打ち
        if (gageValue > gaugeMaxValue)
        {
            if(attackGageStep < ATTACK_GAGE_STEP.MaxStep)
            {
                //段階を一つ増やす
                attackGageStep++;

                //代入
                gageValue = 0;

                //ゲージの色を変更
                gageImage.material = gageStepMaterialList[((int)attackGageStep)];
            }
            else
            {
                gageValue = gaugeMaxValue;
            }
        }

        //画像の長さに影響させる
        gageImage.fillAmount = gageValue / gaugeMaxValue;
    }

    public void SetValue(float _value)
    {
        //加算
        gageValue = _value;

        //最大値頭打ち
        if (gageValue > gaugeMaxValue)
        {
            //代入
            gageValue = gaugeMaxValue;
        }

        attackGageStep = ATTACK_GAGE_STEP.Step1;

        //ゲージの色を変更
        gageImage.material = gageStepMaterialList[((int)attackGageStep)];

        //画像の長さに影響させる
        gageImage.fillAmount = gageValue / gaugeMaxValue;
    }

    public float GetValue()
    {
        return gageValue;
    }

    public float GetValue_Normalize()
    {
        return gageValue / gaugeMaxValue;
    }
}
