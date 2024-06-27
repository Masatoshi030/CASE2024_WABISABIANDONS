using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackGaugeController : MonoBehaviour
{

    [SerializeField, Header("�Q�[�WImage�R���|�[�l���g")]
    Image gageImage;

    [SerializeField, Header("�Q�[�W�̒l")]
    float gageValue = 0.0f;

    [SerializeField, Header("�Q�[�W�̍ő�l")]
    float gaugeMaxValue = 100.0f;

    [SerializeField, Header("�Q�[�W���܂葬�x�̂P�X�e�b�v������")]
    float gaugeAddSpeed_Attenuation = 0.75f;

    public enum ATTACK_GAGE_STEP
    { Step1, Step2, Step3, MaxStep };
    [SerializeField, Header("�Q�[�W�̒i�K"), Toolbar(typeof(ATTACK_GAGE_STEP), "AttackGageStep")]
    public ATTACK_GAGE_STEP attackGageStep = ATTACK_GAGE_STEP.Step1;

    [SerializeField, Header("�Q�[�W�̒i�K�}�e���A�����X�g")]
    Material[] gageStepMaterialList;

    AudioSource au_Gage;

    [SerializeField, Header("�Q�[�W���ߌ��ʉ�")]
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
        //���ʉ�
        if(gageValue == 0.0f)
        {
            //�s�b�`��ς���
            au_Gage.pitch = au_Gage_startPitch + ((int)attackGageStep) * 0.5f;

            //���ʉ����Đ�
            au_Gage.PlayOneShot(gageSound);
        }

        //���Z
        gageValue += _value / ((int)attackGageStep);

        //�ő�l���ł�
        if (gageValue > gaugeMaxValue)
        {
            if(attackGageStep < ATTACK_GAGE_STEP.MaxStep)
            {
                //�i�K������₷
                attackGageStep++;

                //���
                gageValue = 0;

                //�Q�[�W�̐F��ύX
                gageImage.material = gageStepMaterialList[((int)attackGageStep)];
            }
            else
            {
                gageValue = gaugeMaxValue;
            }
        }

        //�摜�̒����ɉe��������
        gageImage.fillAmount = gageValue / gaugeMaxValue;
    }

    public void SetValue(float _value)
    {
        //���Z
        gageValue = _value;

        //�ő�l���ł�
        if (gageValue > gaugeMaxValue)
        {
            //���
            gageValue = gaugeMaxValue;
        }

        attackGageStep = ATTACK_GAGE_STEP.Step1;

        //�Q�[�W�̐F��ύX
        gageImage.material = gageStepMaterialList[((int)attackGageStep)];

        au_Gage.pitch = au_Gage_startPitch;

        //�摜�̒����ɉe��������
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
