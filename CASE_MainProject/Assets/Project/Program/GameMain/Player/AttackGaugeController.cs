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

    [SerializeField, Header("ゲージ溜まり速度の１ステップ減衰量")]
    float gaugeAddSpeed_Attenuation = 0.75f;

    public enum ATTACK_GAGE_STEP
    { Step1, Step2, Step3, MaxStep };
    [SerializeField, Header("ゲージの段階"), Toolbar(typeof(ATTACK_GAGE_STEP), "AttackGageStep")]
    public ATTACK_GAGE_STEP attackGageStep = ATTACK_GAGE_STEP.Step1;

    [SerializeField, Header("ゲージの段階マテリアルリスト")]
    Material[] gageStepMaterialList;

    AudioSource au_Gage;

    [SerializeField, Header("ゲージ溜め効果音")]
    AudioClip gageSound;

    float au_Gage_startPitch;

    // Start is called before the first frame update
    void Start()
    {
        au_Gage = this.GetComponent<AudioSource>();
        au_Gage_startPitch = au_Gage.pitch;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddValue(float _value)
    {
        //効果音
        if(gageValue == 0.0f)
        {
            //ピッチを変える
            au_Gage.pitch = au_Gage_startPitch + ((int)attackGageStep) * 0.5f;

            //効果音を再生
            au_Gage.PlayOneShot(gageSound);
        }

        //加算
        gageValue += _value / ((int)attackGageStep);

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

        au_Gage.pitch = au_Gage_startPitch;

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

    public float GetGageStepValue()
    {
        return ((int)attackGageStep) + 1;
    }
}
