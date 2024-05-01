using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField, Header("移動加速度")]
    Vector2 runSpeed = new Vector2(1.0f, 1.0f);

    [SerializeField, Header("回転補間速度")]
    float rotationLerpSpeed = 1.0f;

    [SerializeField, Header("ジャンプ力")]
    float jumpPower = 5.0f;

    [SerializeField, Header("移動方向に向く軸（Shaft）")]
    Transform horizontalRotationShaft;

    [SerializeField, Header("蒸気貯蔵量"), ReadOnly]
    float heldSteam = 100.0f;

    [SerializeField, Header("出力蒸気量"), ReadOnly]
    float outSteam = 0.0f;

    // プレイヤーのRigidbody
    Rigidbody myRigidbody;

    // カメラの前方ベクトルをプレイヤーの進む方向とする
    Quaternion horizontalRotation;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //=== 回転 ===//

        //カメラの前方ベクトル
        horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);


        //=== ジャンプ ===//

        //ジャンプ
        if (DualSense_Manager.instance.GetInputState().CrossButton == DualSenseUnity.ButtonState.Down)
        {
            myRigidbody.velocity = transform.up * jumpPower;
        }

        //=== 蒸気出力量 ===//

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

    private void FixedUpdate()
    {

        //=== 移動 ===//

        //移動量入力
        Vector2 runValue = DualSense_Manager.instance.GetLeftStick();

        //移動加速度
        runValue *= runSpeed;

        //フレーム制御
        runValue *= Time.deltaTime;

        //移動処理
        myRigidbody.velocity = horizontalRotation * new Vector3(runValue.x, 0.0f, runValue.y);

        //シャフトの向きを変更してプレイヤーを移動方向に向かせる
        if(runValue.magnitude > 0.1f)
        {
            horizontalRotationShaft.localRotation = Quaternion.Lerp(
                horizontalRotationShaft.localRotation,
                horizontalRotation,
                rotationLerpSpeed * Time.deltaTime);
        }
    }
}
