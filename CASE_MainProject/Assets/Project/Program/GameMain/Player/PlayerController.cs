using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //プレイヤーのシングルトンインスタンス
    public static PlayerController instance;

    

    //=== 移動 ===//
    [SerializeField, ReadOnly]
    Vector2 runInput;

    [SerializeField, Header("移動加速度")]
    float runSpeed = 1.0f;

    [SerializeField, Header("空中移動速度減衰")]
    float runSpeed_AirAttenuation = 0.5f;

    [SerializeField, Header("蒸気最大移動加速度")]
    float runSpeed_Steam = 1.0f;

    [SerializeField, Header("回転補間速度")]
    float rotationLerpSpeed = 1.0f;

    [SerializeField, Header("回転方向に向く軸（Shaft）")]
    Transform horizontalRotationShaft;

    [SerializeField, Header("移動方向に向く軸（Shaft）")]
    Transform moveRotationShaft;

    [SerializeField, Header("移動する力"), ReadOnly]
    Vector3 moveVelocity = Vector3.zero;

    [SerializeField, Header("移動ロック")]
    bool bMoveLock = false;

    // カメラの前方ベクトルをプレイヤーの進む方向とする
    Quaternion horizontalRotation;



    //=== ジャンプ ===//
    [SerializeField, Header("ジャンプ力")]
    float jumpPower = 5.0f;

    [SerializeField, Header("長押し加算ジャンプ力")]
    float jumpContinuationPower = 0.01f;

    [SerializeField, Header("ジャンプ時蒸気エフェクト")]
    GameObject jump_SteamEffect;

    [SerializeField, Header("ジャンプ継続最大時間")]
    float jumpMaxTime = 1.0f;

    float jumpTime = 0.0f;

    enum JUMP_STATE
    { Idle, Rising, Descending };
    [SerializeField, Header("ジャンプの状態（ステート）"), Toolbar(typeof(JUMP_STATE), "JumpState")]
    JUMP_STATE jumpState = JUMP_STATE.Idle;

    // プレイヤーのRigidbody
    Rigidbody myRigidbody;

    [SerializeField, Header("接地判定")]
    GroundJudgeController myGroundJudgeController;



    //=== 蒸気 ===//
    [SerializeField, Header("蒸気貯蔵量")]
    public float heldSteam = 100.0f;

    [SerializeField, Header("最大蒸気貯蔵量")]
    public float maxHeldSteam = 100.0f;

    [SerializeField, Header("瞬間出力蒸気量"), ReadOnly]
    float outSteamValue = 0.0f;

    [SerializeField, Header("最大瞬間出力蒸気量")]
    float outMaxSteamValue = 0.5f;

    [SerializeField, Header("自然加圧量")]
    float naturalAddPressure = 10.0f;

    [SerializeField, Header("スチーム音　AudioSource")]
    AudioSource au_Steam;

    [SerializeField, Header("蒸気量で大きさを変える機能を有効")]
    bool bSteamScaleEnable = true;

    [SerializeField, Header("蒸気のパンパン度合いで大きさが変わる軸")]
    GameObject steamScaleShaft;

    [SerializeField, Header("パンパンの時の最大スケール")]
    Vector3 steamMaxScale = new Vector3(1, 1, 1);



    //=== 可燃ガス ===//
    [SerializeField, Header("可燃ガス有効"), ReadOnly]
    bool bCombustibleGas = false;

    [SerializeField, Header("爆発ポイント")]
    GameObject explosionPoint;

    [SerializeField, Header("可燃ガスの爆発ポイント間隔距離")]
    float explosionPoint_InstallationIntervalDistance = 0.3f;

    [SerializeField, Header("前回の可燃ガスの爆発ポイント間隔座標"), ReadOnly]
    Vector3 explosionPoint_InstallationIntervalLastPosition = Vector3.zero;

    [SerializeField, Header("蒸気生成ポイント")]
    GameObject steamInstantiatePoint;

    [SerializeField, Header("噴出蒸気")]
    ParticleSystem compressor_SteamEffect;



    //=== 突撃 ===//
    public enum ATTACK_STATE
    { Idle, Aim, Attack, KnockBack};
    [SerializeField, Header("突撃状態（ステート）"), Toolbar(typeof(ATTACK_STATE), "AttackState")]
    public ATTACK_STATE attackState = ATTACK_STATE.Idle;

    [SerializeField, Header("突撃初速")]
    float attackSpeed = 5.0f;

    [SerializeField, Header("突撃する力"), ReadOnly]
    Vector3 attackVelocity = Vector3.zero;

    [SerializeField, Header("突撃の時の姿勢制御軸")]
    GameObject attackShaft;

    [SerializeField, Header("突進上方向修正")]
    float attackUpCorrectionPower = 1.0f;

    [SerializeField, Header("突撃ゲージ")]
    AttackGaugeController attackGauge;

    [SerializeField, Header("突撃ゲージが溜まる速度")]
    float attackGauge_AddSpeed = 2.0f;

    //突撃ターゲット座標
    Vector3 AttackTargetPosition;

    //突撃ゲージの溜めた値
    float gaugeAttackValue = 0.0f;

    [SerializeField, Header("突撃可能フラグ")]
    public bool bAttackPossible = true;



    //=== 死亡 ===//
    [SerializeField, Header("破片エフェクト")]
    GameObject partsEffect;

    [SerializeField, Header("爆発エフェクト")]
    GameObject explosionEffect;


    //=== バルブ蒸気ぶっ飛び ===//
    [SerializeField, Header("バルブぶっ飛び時間")]
    float valveFlyTime = 1.0f;

    [SerializeField, Header("バルブぶっ飛びタイマー")]
    float valveFlyTimer = 0.0f;

    //バルブでぶっ飛んでいくベクトル
    Vector3 valveFlyVector = Vector3.zero;

    public enum VALVE_FLY_STATE
    { Idle, Fly };
    [SerializeField, Header("バルブぶっ飛び状態（ステート）"), Toolbar(typeof(VALVE_FLY_STATE), "ValveFlyState")]
    public VALVE_FLY_STATE valveFlyState = VALVE_FLY_STATE.Idle;



    //=== GoldValve ===//
    [SerializeField, Header("ゴールドバルブカウント")]
    int heldGoldValve = 0;

    [SerializeField, Header("ゴールドバルブの効果音")]
    AudioClip goldValveSoundClip;

    [SerializeField, Header("ゴールドバルブの効果音マネージャー")]
    SoundEffectManager goldValveSoundEffectManager;


    //=== BGM・SE ===//
    BGMManager mainBGMManager;


    [SerializeField, Header("キャラクターアニメーション")]
    Animator characterAnimation;

    [SerializeField, Header("メインプレイヤーカメラオブジェクト"), ReadOnly]
    GameObject mainPlayerCamera_Obj;

    [SerializeField, Header("ノックバック力")]
    float knockBackPower = 5.0f;

    [SerializeField, Header("一定間隔で一回処理する処理の間隔")]
    float fixedIntervalTime = 1.0f;

    float fixedIntervalTimer = 0.0f;

    [SerializeField, Header("VolumesAnimation"), ReadOnly]
    Animator volumeAnimation;

    [SerializeField, Header("動作ロック")]
    bool bLock = false;

    [SerializeField, Header("プレイヤーのレイヤーマスク")]
    int playerLayerMask = 6;

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

        //可燃ガスの設定
        bCombustibleGas = GameSetting.instance.bCombustibleGasEnable;

        //自分自身のRigidbodyを取得
        myRigidbody = GetComponent<Rigidbody>();

        //メインプレイヤーカメラオブジェクトの参照
        mainPlayerCamera_Obj = GameObject.Find("PlayerCamera_Brain");

        //VolumeAnimation参照
        volumeAnimation = GameObject.Find("Volumes").GetComponent<Animator>();

        //突撃ゲージの参照
        attackGauge = GameObject.Find("AttackGauge").GetComponent<AttackGaugeController>();
        attackGauge.gameObject.SetActive(false);

        //mainBGMManagerを取得
        mainBGMManager = GameObject.Find("mainBGMManager").GetComponent<BGMManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //ロックがかかったら早期リターン
        if (bLock)
        {
            return;
        }

        //=== 重力 ===//
        moveVelocity.y = myRigidbody.velocity.y;


        //=== ジャンプ ===//

        OnJump();



        //=== 蒸気貯蔵 ===//
        if (outSteamValue > 0.0f)
        {
        }
        else
        {
            //自然加圧
            heldSteam += naturalAddPressure * Time.deltaTime;
        }

        //死亡処理
        if (heldSteam > maxHeldSteam)
        {
            //操作をロック
            bLock = true;

            //プレイヤーのBodyを非表示に
            characterAnimation.gameObject.SetActive(false);

            //破片エフェクト生成
            Instantiate(partsEffect, transform.position, Quaternion.identity);

            //爆発エフェクト生成
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

            //フェード
            GameUIManager.instance.SetFade(true, 2.0f);

            //振動処理
            DualSense_Manager.instance.SetLeftRumble(1.0f, 0.5f);

            //BGMをフェード終了
            mainBGMManager.SetFadeStop(2.0f);

        }


        //=== 蒸気出力量 ===//

        //蒸気出力
        if (heldSteam > 0.0f)
        {
            //瞬間出力蒸気量
            outSteamValue = (float)DualSense_Manager.instance.GetInputState().LeftTrigger.TriggerValue;

            //噴出蒸気の振動
            DualSense_Manager.instance.SetRightRumble(outSteamValue, 0.05f);

            //貯蔵圧力から減らす
            heldSteam -= outSteamValue * outMaxSteamValue;

            //蒸気音量調節
            au_Steam.volume = outSteamValue;

            //蒸気の状態でポストエフェクトをブレンド
            volumeAnimation.SetFloat("fPressure", heldSteam / maxHeldSteam);

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
        if (bMoveLock == false)
        {
            myRigidbody.velocity = moveVelocity;
        }


        //=== 可燃ガス ===//

        if (bCombustibleGas)
        {
            if (outSteamValue > 0.2f)
            {
                //前回の設置座標と今の座標との差分が設定間隔よりも遠いかどうか
                float offsetDistance = Vector3.Distance(explosionPoint_InstallationIntervalLastPosition, transform.position);

                if (offsetDistance > explosionPoint_InstallationIntervalDistance)
                {
                    //爆発ポイントを生成
                    Instantiate(explosionPoint, steamInstantiatePoint.transform.position, Quaternion.identity);

                    //座標を記録
                    explosionPoint_InstallationIntervalLastPosition = transform.position;
                }
            }
        }


        //=== 突撃 ===//

            OnAttack();


        //=== 一定間隔処理 ===//

        OnFixedInterval();
    }

    private void FixedUpdate()
    {

        //ロックがかかったら早期リターン
        if (bLock)
        {
            return;
        }

        //=== 移動 ===//

        //移動量入力
        runInput = DualSense_Manager.instance.GetLeftStick();

        // 移動量計算　　移動入力 × 線形補間（通常スピードと蒸気スピードの間を蒸気出力量で補間）
        Vector2 runValue = runInput * Mathf.Lerp(runSpeed, runSpeed_Steam, outSteamValue);

        //空中にいると移動速度を減衰する
        if(myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.Off)
        {
            runValue *= runSpeed_AirAttenuation;
        }

        //フレーム制御
        runValue *= Time.deltaTime;

        //移動処理
        moveVelocity = horizontalRotation * new Vector3(runValue.x, moveVelocity.y, runValue.y);

        //地面にいるときは移動アニメーション
        characterAnimation.SetFloat("fRun", runInput.magnitude + outSteamValue);

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


        //=== バルブぶっ飛び ===//
        OnValveJump();
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
            if (DualSense_Manager.instance.GetInputState().LeftTrigger.TriggerValue >= 1.0f)
            {
                //ジャンプ上昇状態へ移行
                jumpState = JUMP_STATE.Rising;

                //ジャンプアニメーション開始
                characterAnimation.SetTrigger("tJump");

                //初速をつける
                moveVelocity = new Vector3(moveVelocity.x, jumpPower, moveVelocity.z);

                //蒸気エフェクト
                Instantiate(jump_SteamEffect, transform.position, Quaternion.identity);
            }
        }
        //上昇中
        else if (jumpState == JUMP_STATE.Rising)
        {
            //上昇中に×ボタンを押していたら
            if (DualSense_Manager.instance.GetInputState().LeftTrigger.TriggerValue > 0.0f)
            {
                jumpTime += Time.deltaTime;

                //追加速度をつける
                moveVelocity = new Vector3(
                    moveVelocity.x,
                    moveVelocity.y + jumpContinuationPower * (jumpMaxTime - jumpTime / jumpMaxTime) * Time.deltaTime,
                    moveVelocity.z);
            }

            //ジャンプ最大時間を過ぎるか×ボタンを押すのをやめたら降下に移行
            if (jumpTime > jumpMaxTime || DualSense_Manager.instance.GetInputState().LeftTrigger.TriggerValue == 0.0f)
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log("player");
        }
        if(other.tag == "GoldValve")
        {
            //取得フラグ
            other.GetComponent<GoldValveController>().GetGoldValve();
        }

        if(other.tag == "DamageSteam")
        {
            //ダメージ
            heldSteam += 10.0f;

            //ノックバック
            KnockBack();
        }
    }

    public void GetGoldValve()
    {
        //アイテムカウントを増やす
        heldGoldValve++;

        //取得音再生
        goldValveSoundEffectManager.PlaySoundEffect(goldValveSoundClip);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.root.tag == "MoveGround")
        {
            transform.parent = collision.transform.parent.parent.GetComponent<MoveGrouond>().startPoint.transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.root.tag == "MoveGround")
        {
            transform.parent = null;
            transform.rotation = Quaternion.identity;
        }
    }

    public void SetValveJump(Vector3 _forceVec)
    {
        //速度ゼロ
        myRigidbody.velocity = Vector3.zero;

        //突撃終了
        StopAttack();

        valveFlyState = VALVE_FLY_STATE.Fly;

        valveFlyVector = _forceVec;

        Debug.Log("ばるぶ！");
    }

    void OnValveJump()
    {
        if (valveFlyState == VALVE_FLY_STATE.Fly)
        {
            valveFlyTimer += Time.deltaTime;

            transform.position += valveFlyVector * Time.deltaTime;

            if(myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.On)
            {
                valveFlyState = VALVE_FLY_STATE.Idle;
                valveFlyTimer = 0.0f;

                myRigidbody.velocity = Vector3.zero;
            }
        }
    }

    public void Damage(float _damage)
    {
        characterAnimation.SetTrigger("tDamage");

        //突撃方向の反対ベクトルの斜め上にノックバックする
        myRigidbody.velocity = (Vector3.up - moveRotationShaft.transform.forward) * knockBackPower;

        heldSteam += _damage;
    }

    void OnAttack()
    {

        //突撃中
        if (attackState == ATTACK_STATE.Attack)
        {

            //突撃速度　計算したターゲットへのベクトルを正規化し、速度を乗算する
            transform.position += (AttackTargetPosition - transform.position).normalized * (attackSpeed * gaugeAttackValue) * Time.deltaTime;

            //頭を飛んでいくほうに向ける
            attackShaft.transform.LookAt(AttackTargetPosition);      //前方ベクトルを向ける
            attackShaft.transform.Rotate(90.0f, 0.0f, 0.0f);
        }

        //地面に着地すると終了
        if (myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.On)
        {
            if (attackState == ATTACK_STATE.Attack || attackState == ATTACK_STATE.KnockBack)
            {
                StopAttack();
            }

            //突撃可能フラグを有効にする
            bAttackPossible = true;
        }

        //狙っている時
        if (attackState == ATTACK_STATE.Aim)
        {
            attackGauge.AddValue(Time.deltaTime * attackGauge_AddSpeed);
        }

        //突撃が可能であれば
        if (bAttackPossible)
        {
            //空中にいる状態で
            if (myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.Off)
            {
                //右トリガーを押すと
                if (DualSense_Manager.instance.GetInputState().RightTrigger.ActiveState == DualSenseUnity.ButtonState.NewDown)
                {
                    //攻撃状態へ移行
                    attackState = ATTACK_STATE.Aim;

                    //突撃アニメーション開始
                    characterAnimation.SetBool("bAttack", true);

                    //突撃ポストエフェクト有効
                    volumeAnimation.SetBool("bAttack", true);

                    //突撃ゲージを出現
                    attackGauge.gameObject.SetActive(true);

                    //全ての方向の力を０にする
                    myRigidbody.velocity = Vector3.zero;

                    //重力を無効にする
                    myRigidbody.useGravity = false;

                    //感度をリセット
                    PlayerVirtualCameraController.instance.OnAim(100, 100);

                    //移動ロック
                    bMoveLock = true;

                    //スロー
                    //Time.timeScale = 0.1f;
                }

                //右トリガーを離す
                if (DualSense_Manager.instance.GetInputState().RightTrigger.ActiveState == DualSenseUnity.ButtonState.NewUp)
                {
                    if (attackState == ATTACK_STATE.Aim)
                    {
                        //攻撃状態へ移行
                        attackState = ATTACK_STATE.Attack;

                        //突撃ゲージから溜めた結果を取得
                        gaugeAttackValue = attackGauge.GetValue_Normalize();

                        //突撃ゲージを隠す
                        attackGauge.SetValue(0.0f);
                        attackGauge.gameObject.SetActive(false);

                        //感度をリセット
                        PlayerVirtualCameraController.instance.OnAimReset();

                        //突撃可能フラグを無効にする
                        bAttackPossible = false;

                        //スクリーンの中心（カーソルを合わせたオブジェクト）に向かうベクトルを計算する

                        //カメラの前方ベクトルを初期値とする　※もしターゲットが検出されなくても前方に飛べるようにする。
                        AttackTargetPosition = mainPlayerCamera_Obj.transform.forward;

                        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                        RaycastHit hit;

                        int LayerMask = ~(1 << playerLayerMask);

                        // Rayが何かに当たったかどうかを確認
                        if (Physics.Raycast(ray, out hit, 1000.0f, LayerMask))
                        {
                            AttackTargetPosition = hit.point;
                        }

                        //スロー終了
                        //Time.timeScale = 1.0f;
                    }
                }
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
        //突撃ポストエフェクト無効
        volumeAnimation.SetBool("bAttack", false);

        //重力を無効にする
        myRigidbody.useGravity = true;

        //移動ロック
        bMoveLock = false;
    }

    public void KnockBack()
    {
        //ノックバックアニメーション終了
        characterAnimation.SetTrigger("tHit");

        Debug.Log("Knock");

        //突撃方向の反対ベクトルの斜め上にノックバックする
        myRigidbody.velocity = Vector3.up * knockBackPower;

        //ノックバック状態にする
        attackState = ATTACK_STATE.KnockBack;
    }

    public void KnockBack(float power,  Vector3 direction)
    {
        //ノックバックアニメーション終了
        characterAnimation.SetTrigger("tHit");

        Debug.Log("Knock");

        //突撃方向の反対ベクトルの斜め上にノックバックする
        myRigidbody.velocity = direction * power;

        //ノックバック状態にする
        attackState = ATTACK_STATE.KnockBack;
    }

    void OnFixedInterval()
    {
        //カウント
        fixedIntervalTimer += Time.deltaTime;
        //一定時間が経つと
        if(fixedIntervalTimer > fixedIntervalTime)
        {
            //=== 処理 ===//

            //if (heldSteam > 0.0f)
            //{
            //    //トリガー抵抗力
            //    DualSense_Manager.instance.SetLeftTriggerContinuousResistanceEffect(0.0f, heldSteam / maxHeldSteam);
            //}
            //else
            //{
            //    //トリガー抵抗を無効にする
            //    DualSense_Manager.instance.SetLeftTriggerNoEffect();
            //}

            if (bSteamScaleEnable)
            {
                //蒸気のパンパン度合いでプレイヤーのスケールを変える
                steamScaleShaft.transform.localScale = Vector3.Lerp(new Vector3(1.0f, 1.0f, 1.0f), steamMaxScale, heldSteam / maxHeldSteam);
            }

            //左トリガーの抵抗設定
            DualSense_Manager.instance.SetLeftTriggerEffect_Position(0.6f, 0.7f, 1.0f);

            //タイマーをリセット
            fixedIntervalTimer = 0.0f;
        }
    }

    public void OnGoal()
    {
        //プレイヤーのカメラを無効にする
        mainPlayerCamera_Obj.GetComponent<Camera>().enabled = false;

        //プレイヤーの動きを止める
        bLock = true;

        //チュートリアルのガイドを削除
        GameObject.Find("TutorialCanvas").SetActive(false);
    }
}