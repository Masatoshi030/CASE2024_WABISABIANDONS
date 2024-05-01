using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField, Header("移動速度")]
    Vector2 runSpeed = new Vector2(1.0f, 1.0f);

    [SerializeField, Header("ジャンプ力")]
    float jumpPower = 5.0f;

    [SerializeField, Header("蒸気貯蔵量"), ReadOnly]
    float heldSteam = 100.0f;

    [SerializeField, Header("出力蒸気量"), ReadOnly]
    float outSteam = 0.0f;

    Rigidbody myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //移動量入力
        Vector2 runValue = DualSense_Manager.instance.GetLeftStick() * runSpeed;

        //フレーム制御
        runValue *= Time.deltaTime;

        //座標変更
        this.transform.Translate(runValue.x, 0.0f, runValue.y);

        //ジャンプ
        if(DualSense_Manager.instance.GetInputState().CrossButton == DualSenseUnity.ButtonState.Down)
        {
            myRigidbody.velocity = transform.up * jumpPower;
        }

        //蒸気出力
        if (heldSteam > 0.0f)
        {
            heldSteam -= (float)DualSense_Manager.instance.GetInputState().RightTrigger.TriggerValue;

            if (heldSteam < 0.0f)
            {
                heldSteam = 0.0f;
            }
        }
    }
}
