using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("移動加速度")]
    Vector2 runSpeed = new Vector2(1.0f, 1.0f);

    [SerializeField, Header("回転補間速度")]
    float rotationLerpSpeed = 1.0f;

    [SerializeField, Header("回転方向に向く軸（Shaft）")]
    Transform horizontalRotationShaft;

    [SerializeField, Header("移動方向に向く軸（Shaft）")]
    Transform moveRotationShaft;

    [SerializeField, Header("ジャンプ力")]
    float jumpPower = 5.0f;

    [SerializeField, Header("長押し加算ジャンプ力")]
    float jumpContinuationPower = 0.01f;

    [SerializeField, Header("ジャンプ継続最大時間")]
    float jumpMaxTime = 1.0f;

    float jumpTime = 0.0f;

    enum JUMP_STATE
    { Idle, Rising, Descending };
    [SerializeField, Header("ジャンプの状態（ステート）"), Toolbar(typeof(JUMP_STATE), "JumpState")]
    JUMP_STATE jumpState = JUMP_STATE.Idle;

    [SerializeField, Header("接地判定")]
    GroundJudgeController myGroundJudgeController;

    [SerializeField, Header("蒸気貯蔵量"), ReadOnly]
    float heldSteam = 100.0f;

    [SerializeField, Header("出力蒸気量"), ReadOnly]
    float outSteam = 0.0f;

    // プレイヤーのRigidbody
    Rigidbody myRigidbody;

    // カメラの前方ベクトルをプレイヤーの進む方向とする
    Quaternion horizontalRotation;

    [SerializeField, ReadOnly]
    Vector2 runInput;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        //=== ジャンプ ===//

        OnJump();

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
        runInput = DualSense_Manager.instance.GetLeftStick();

        //移動加速度
        Vector2 runValue = runInput * runSpeed * Time.deltaTime;

        //移動処理
        myRigidbody.velocity = horizontalRotation * new Vector3(runValue.x, myRigidbody.velocity.y, runValue.y);


        //=== 回転 ===//

        //カメラの前方ベクトル
        horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        //シャフトの向きを変更してプレイヤーを移動方向に向かせる
        if (runValue.magnitude > 0.1f)
        {
            //カメラの前方ベクトルを向く
            horizontalRotationShaft.localRotation = Quaternion.Lerp(
                horizontalRotationShaft.localRotation,
                horizontalRotation,
                rotationLerpSpeed * Time.deltaTime);

            //移動の方向へ向く
            moveRotationShaft.localRotation = Quaternion.Lerp(
                moveRotationShaft.localRotation,
                Quaternion.LookRotation(new Vector3(runInput.x, 0.0f, runInput.y)),
                rotationLerpSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    void OnJump()
    {
        //ジャンプ待ち状態
        if (jumpState == JUMP_STATE.Idle)
        {
            //最初の初速とジャンプ開始命令
            if (DualSense_Manager.instance.GetInputState().CrossButton == DualSenseUnity.ButtonState.NewDown)
            {
                //ジャンプ上昇状態へ移行
                jumpState = JUMP_STATE.Rising;
                //初速をつける
                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpPower, myRigidbody.velocity.z);
            }
        }
        //上昇中
        else if(jumpState == JUMP_STATE.Rising)
        {
            //上昇中に×ボタンを押していたら
            if (DualSense_Manager.instance.GetInputState().CrossButton == DualSenseUnity.ButtonState.Down)
            {
                jumpTime += Time.deltaTime;

                //追加速度をつける
                myRigidbody.velocity = new Vector3(
                    myRigidbody.velocity.x, 
                    myRigidbody.velocity.y + jumpContinuationPower * (jumpMaxTime - jumpTime / jumpMaxTime) * Time.deltaTime,
                    myRigidbody.velocity.z);
            }

            //ジャンプ最大時間を過ぎるか×ボタンを押すのをやめたらと降下に移行
            if(jumpTime > jumpMaxTime || DualSense_Manager.instance.GetInputState().CrossButton != DualSenseUnity.ButtonState.Down)
            {
                jumpState = JUMP_STATE.Descending;
            }
        }
        //降下中
        else if(jumpState == JUMP_STATE.Descending)
        {
            //接地判定が有効になったらジャンプ終了
            if(myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.On)
            {
                jumpState = JUMP_STATE.Idle;
                jumpTime = 0.0f;
            }
        }
    }
}
