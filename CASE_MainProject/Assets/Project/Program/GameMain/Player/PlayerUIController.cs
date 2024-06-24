using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{

    [SerializeField, Header("���C�Q�[�W")]
    Image steamGauge;

    [SerializeField, Header("�U���̍ŏ�Intensity")]
    float minVibrationIntensity = 0.25f;

    [SerializeField, Header("�U���̍ŏ�Intensity")]
    float maxVibrationIntensity = 1.0f;

    [SerializeField, Header("�Q�[�W�ő勖�e�͈�")]
    float gageMaxToleranceSteamValue = 10.0f;

    [SerializeField, Header("�Q�[�WSpriteRenderer")]
    Image gageImage;

    [SerializeField, Header("�Q�[�W�̐F���X�g")]
    Material[] gageColorMaterialList;

    SinVibration mySinVibration;

    // Start is called before the first frame update
    void Start()
    {
        mySinVibration = this.GetComponent<SinVibration>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        steamGauge.fillAmount = PlayerController.instance.heldSteam / (PlayerController.instance.maxHeldSteam - gageMaxToleranceSteamValue);

        mySinVibration.Intensity = Mathf.Lerp(minVibrationIntensity, maxVibrationIntensity, PlayerController.instance.heldSteam / PlayerController.instance.maxHeldSteam);

        //�}�e���A���̊���U��̃C���f�b�N�X������
        if (steamGauge.fillAmount < 0.5f)
        {
            //�}�e���A���K�p
            gageImage.material = gageColorMaterialList[0];
        }
        else if (steamGauge.fillAmount < 0.7f)
        {
            //�}�e���A���K�p
            gageImage.material = gageColorMaterialList[1];
        }
        else
        {
            //�}�e���A���K�p
            gageImage.material = gageColorMaterialList[2];
        }
    }
}
