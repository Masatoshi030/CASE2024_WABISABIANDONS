using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //プレイヤーのシングルトンインスタンス
    public static PlayerController instance;

    [SerializeField, Header("移動加速度")]
    float runSpeed = 1.0f;

    [SerializeField, Header("蒸気最大移動加速度")]
    float runSpeed_Steam = 1.0f;

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

    [SerializeField, Header("蒸気最大ジャンプ力")]
    float jumpPower_Steam = 8.0f;

    [SerializeField, Header("蒸気最大長押し加算ジャンプ力")]
    float jumpContinuationPower_Steam = 0.02f;

    float jumpTime = 0.0f;

    enum JUMP_STATE
    { Idle, Rising, Descending };
    [SerializeField, Header("ジャンプの状態（ステート）"), Toolbar(typeof(JUMP_STATE), "JumpState")]
    JUMP_STATE jumpState = JUMP_STATE.Idle;

    [SerializeField, Header("接地判定")]
    GroundJudgeController myGroundJudgeController;

    [SerializeField, Header("蒸気貯蔵量")]
    float heldSteam = 100.0f;

    [SerializeField, Header("瞬間出力蒸気量"), ReadOnly]
    float outSteamValue = 0.0f;

    [SerializeField, Header("最大瞬間出力蒸気量")]
    float outMaxSteamValue = 0.5f;

    [SerializeField, Header("スチーム音　AudioSource")]
    AudioSource au_Steam;

    // プレイヤーのRigidbody
    Rigidbody myRigidbody;

    // カメラの前方ベクトルをプレイヤーの進む方向とする
    Quaternion horizontalRotation;

    [SerializeField, ReadOnly]
    Vector2 runInput;

    public enum ATTACK_STATE
    { Idle, Attack};
    [SerializeField, Header("突撃状態（ステート）"), Toolbar(typeof(ATTACK_STATE), "AttackState")]
    public ATTACK_STATE attackState = ATTACK_STATE.Idle;

    [SerializeField, Header("突撃初速")]
    float attackSpeed = 5.0f;

    [SerializeField, Header("キャラクターアニメーション")]
    Animator characterAnimation;

    [SerializeField, Header("メインプレイヤーカメラオブジェクト"), ReadOnly]
    GameObject mainPlayerCamera_Obj;

    [SerializeField, Header("移動する力"), ReadOnly]
    Vector3 moveVelocity = Vector3.zero;

    [SerializeField, Header("突撃する力"), ReadOnly]
    Vector3 attackVelocity = Vector3.zero;

    [SerializeField, Header("突撃の時の姿勢制御軸")]
    GameObject attackShaft;

    [SerializeField, Header("噴出蒸気")]
    ParticleSystem compressor_SteamEffect;

    [SerializeField, Header("ノックバック力")]
    float knockBackPower = 5.0f;

    // Start is called before the first frame update
    void Start()
    {

        if (instance == null)
        {
            // 自身をインスタンスとする
            instance = this;
        }
        else
        {
            // インスタンスが既に存在していたら自身を消去する
            Destroy(gameObject);
        }

        //自分自身のRigidbodyを取得
        myRigidbody = GetComponent<Rigidbody>();

        //メインプレイヤーカメラオブジェクトの参照
        mainPlayerCamera_Obj = GameObject.Find("PlayerCamera_Brain");
    }

    // Update is called once per frame
    void Update()
    {
        //=== 重力 ===//
        moveVelocity.y = myRigidbody.velocity.y;

        //=== ジャンプ ===//

        OnJump();

        //=== 蒸気出力量 ===//

        //蒸気出力
        if (heldSteam > 0.0f)
        {
            //瞬間出力蒸気量
            outSteamValue = (float)DualSense_Manager.instance.GetInputState().RightTrigger.TriggerValue;

            //貯蔵圧力から減らす
            heldSteam -= outSteamValue * outMaxSteamValue;

            //蒸気音量調節
            au_Steam.volume = outSteamValue;

            if (heldSteam < 0.0f)
            {
                heldSteam = 0.0f;

                //蒸気エフェクト
                var emission = compressor_SteamEffect.emission;
                emission.rateOverTime = 0.0f;
            }
            else
            {
                //蒸気エフェクト
                var emission = compressor_SteamEffect.emission;
                emission.rateOverTime = 50.0f * outSteamValue;
            }
        }
        else
        {
            outSteamValue = 0.0f;
            au_Steam.volume = 0.0f;
        }

        //プレイヤーに力を加える
        myRigidbody.velocity = moveVelocity;

        //=== 突撃 ===//

        OnAttack();
    }

    private void FixedUpdate()
    {
        //=== 移動 ===//

        //移動量入力
        runInput = DualSense_Manager.instance.GetLeftStick();

        // 移動量計算　　移動入力 × 線形補間（通常スピードと蒸気スピードの間を蒸気出力量で補間）
        Vector2 runValue = runInput * Mathf.Lerp(runSpeed, runSpeed_Steam, outSteamValue);

        //フレーム制御
        runValue *= Time.deltaTime;

        //移動処理
        moveVelocity = horizontalRotation * new Vector3(runValue.x, moveVelocity.y, runValue.y);

        //地面にいるときは移動アニメーション
        characterAnimation.SetFloat("fRun", runInput.magnitude);

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

                //ジャンプアニメーション開始
                characterAnimation.SetTrigger("tJump");

                //初速をつける
                moveVelocity = new Vector3(moveVelocity.x, Mathf.Lerp(jumpPower, jumpPower_Steam, outSteamValue), moveVelocity.z);
            }
        }
        //上昇中
        else if (jumpState == JUMP_STATE.Rising)
        {
            //上昇中に×ボタンを押していたら
            if (DualSense_Manager.instance.GetInputState().CrossButton == DualSenseUnity.ButtonState.Down)
            {
                jumpTime += Time.deltaTime;

                //追加速度をつける
                moveVelocity = new Vector3(
                    moveVelocity.x,
                    moveVelocity.y + Mathf.Lerp(jumpContinuationPower, jumpContinuationPower_Steam, outSteamValue) * (jumpMaxTime - jumpTime / jumpMaxTime) * Time.deltaTime,
                    moveVelocity.z);
            }

            //ジャンプ最大時間を過ぎるか×ボタンを押すのをやめたらと降下に移行
            if (jumpTime > jumpMaxTime || DualSense_Manager.instance.GetInputState().CrossButton != DualSenseUnity.ButtonState.Down)
            {
                jumpState = JUMP_STATE.Descending;
            }
        }
        //降下中
        else if (jumpState == JUMP_STATE.Descending)
        {
            //接地判定が有効になったらジャンプ終了
            if (myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.On)
            {
                jumpState = JUMP_STATE.Idle;

                jumpTime = 0.0f;
            }
        }

        //接地する
        if (myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.Off)
        {
            characterAnimation.SetBool("bOnGround", false);
        }
        else
        {
            //ジャンプアニメーション終了
            characterAnimation.SetBool("bOnGround", true);
        }
    }

    void OnAttack()
    {

        //突撃中
        if (attackState == ATTACK_STATE.Attack)
        {

            //突撃速度
            transform.position += mainPlayerCamera_Obj.transform.forward * attackSpeed * Time.deltaTime;

            //頭を飛んでいくほうに向ける
            attackShaft.transform.LookAt(attackShaft.transform.position + mainPlayerCamera_Obj.transform.forward);      //前方ベクトルを向ける
            attackShaft.transform.Rotate(90.0f, 0.0f, 0.0f);

            //地面に着地すると終了
            if (myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.On)
            {
                StopAttack();
            }
        }

        //空中にいる状態で
        if (myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.Off)
        {
            //ジャンプボタンを押すと
            if(DualSense_Manager.instance.GetInputState().CrossButton == DualSenseUnity.ButtonState.NewDown)
            {
                //攻撃状態へ移行
                attackState = ATTACK_STATE.Attack;

                //突撃アニメーション開始
                characterAnimation.SetBool("bAttack", true);
            }
        }
    }

    public void StopAttack()
    {
        attackState = ATTACK_STATE.Idle;
        //突撃アニメーション終了
        characterAnimation.SetBool("bAttack", false);
        //姿勢を戻す
        attackShaft.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    public void KnockBack()
    {
        //ノックバックアニメーション終了
        characterAnimation.SetTrigger("tHit");

        //突撃方向の反対ベクトルの斜め上にノックバックする
        myRigidbody.velocity = Vector3.up * knockBackPower;
    }
}