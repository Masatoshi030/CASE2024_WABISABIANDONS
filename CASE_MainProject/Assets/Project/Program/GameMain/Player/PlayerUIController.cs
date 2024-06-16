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

    SinVibration mySinVibration;

    // Start is called before the first frame update
    void Start()
    {
        mySinVibration = this.GetComponent<SinVibration>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        steamGauge.fillAmount = PlayerController.instance.heldSteam / PlayerController.instance.maxHeldSteam;

        mySinVibration.Intensity = Mathf.Lerp(minVibrationIntensity, maxVibrationIntensity, PlayerController.instance.heldSteam / PlayerController.instance.maxHeldSteam);
    }
}
