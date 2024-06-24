using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{

    [SerializeField, Header("蒸気ゲージ")]
    Image steamGauge;

    [SerializeField, Header("振動の最小Intensity")]
    float minVibrationIntensity = 0.25f;

    [SerializeField, Header("振動の最小Intensity")]
    float maxVibrationIntensity = 1.0f;

    [SerializeField, Header("ゲージ最大許容範囲")]
    float gageMaxToleranceSteamValue = 10.0f;

    [SerializeField, Header("ゲージSpriteRenderer")]
    Image gageImage;

    [SerializeField, Header("ゲージの色リスト")]
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

        //マテリアルの割り振りのインデックスを決定
        if (steamGauge.fillAmount < 0.5f)
        {
            //マテリアル適用
            gageImage.material = gageColorMaterialList[0];
        }
        else if (steamGauge.fillAmount < 0.7f)
        {
            //マテリアル適用
            gageImage.material = gageColorMaterialList[1];
        }
        else
        {
            //マテリアル適用
            gageImage.material = gageColorMaterialList[2];
        }
    }
}
