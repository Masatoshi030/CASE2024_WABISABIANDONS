using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualSenseUnity;
using System.ComponentModel;
using UnityEngine.SceneManagement;

public class DualSense_Manager : MonoBehaviour
{
    [SerializeField,Header("コントローラーの個数")]
    private uint controllerCount;

    [SerializeField, Header("左スティックのデッド")]
    float dead_LeftStick = 0.05f;

    [SerializeField, Header("右スティックのデッド")]
    float dead_RightStick = 0.05f;

    [SerializeField, Header("振動パターン１")]
    AnimationCurve rumble_Type1;

    //コントローラーリスト
    private List<DualSenseController> dualSenseControllers;

    //メインコントローラー
    private DualSenseController mainController;

    //メインコントローラーの状態
    private ControllerInputState mainControllerInputState;

    //右ランブルのタイマー
    float rightRumbleTimer = 0.0f;

    //左ランブルのタイマー
    float leftRumbleTimer = 0.0f;

    //右ランブルのタイマー
    float rightRumbleTimer_type = 0.0f;

    bool bLeftTriggerEffect = false;

    bool bRightTriggerEffect = false;

    ControllerOutputState output = new ControllerOutputState();

    //左トリガーの抵抗の開始地点
    float leftTriggerEffect_StartPosition = 0.0f;

    //左トリガーの抵抗の終了地点
    float leftTriggerEffect_EndPosition = 0.0f;

    //左トリガーの抵抗の大きさ
    float leftTriggerEffect_Force = 0.0f;

    //シングルトンインスタンス
    public static DualSense_Manager instance; // インスタンスの定義


    private void Awake()
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
    }

    // Start is called before the first frame update
    void Start()
    {
        //コントローラーの数を取得
        controllerCount = DualSense.GetControllerCount();

        //コントローラーのハンドルを全て取得
        dualSenseControllers = DualSense.GetControllers();

        //コントローラーの数が変わったら更新処理を呼ぶ
        DualSense.ControllerCountChanged += RefreshControllers;

        //コントローラーの初期処理
        RefreshControllers();

        //メインコントローラーを設定
        mainController = dualSenseControllers[0];

        SetLeftTriggerNoEffect();
    }

    // Update is called once per frame
    void Update()
    {
        //メインコントローラーの入力状態を格納
        mainControllerInputState = mainController.GetInputState();

        //ランブルタイマーが０になると停止
        //右
        if(rightRumbleTimer > 0.0f)
        {
            rightRumbleTimer -= Time.deltaTime;
            if(rightRumbleTimer < 0.0f)
            {
                StopRightRumble();
                rightRumbleTimer = 0.0f;
            }
        }
        //左
        if (leftRumbleTimer > 0.0f)
        {
            leftRumbleTimer -= Time.deltaTime;
            if (leftRumbleTimer < 0.0f)
            {
                StopLeftRumble();
                leftRumbleTimer = 0.0f;
            }
        }


        if (rightRumbleTimer_type > 0.0f)
        {
            rightRumbleTimer_type -= Time.deltaTime;
            if (rightRumbleTimer_type < 0.0f)
            {
                StopLeftRumble();
                rightRumbleTimer_type = 0.0f;
            }
            else
            {
                output.LeftRumbleIntensity = rumble_Type1.Evaluate((1.0f - rightRumbleTimer_type));
            }
        }

        mainController.SetOutputState(output);
    }

    /// <summary>
    /// コントローラーの更新処理
    /// </summary>
    private void RefreshControllers()
    {
        controllerCount = DualSense.GetControllerCount();
        dualSenseControllers = DualSense.GetControllers();
    }

    /// <summary>
    /// メインコントローラーの入力状態取得
    /// </summary>
    /// <returns>メインコントローラーの入力状態</returns>
    public ControllerInputState GetInputState()
    {
        return mainControllerInputState;
    }

    /// <summary>
    /// 設定時間の間、コントローラーの右ランブルが振動
    /// </summary>
    /// <param name="_intensity">振動の強さ [0~1]</param>
    /// <param name="_time">継続時間</param>
    public void SetRightRumble(float _intensity, float _time)
    {
        output.RightRumbleIntensity = _intensity;
        rightRumbleTimer = _time;
    }

    /// <summary>
    /// 設定時間の間、コントローラーの左ランブルが振動
    /// </summary>
    /// <param name="_intensity">振動の強さ [0~1]</param>
    /// <param name="_time">継続時間</param>
    public void SetLeftRumble(float _intensity, float _time)
    {
        output.LeftRumbleIntensity = _intensity;
        leftRumbleTimer = _time;
    }

    public void SetRumble_Type1()
    {
        rightRumbleTimer_type = 1.0f;
    }

    /// <summary>
    /// コントローラーの右ランブルを停止
    /// </summary>
    public void StopRightRumble()
    {
        output.RightRumbleIntensity = 0.0f;
        rightRumbleTimer = 0.0f;
    }

    /// <summary>
    /// コントローラーの左ランブルを停止
    /// </summary>
    public void StopLeftRumble()
    {
        output.LeftRumbleIntensity = 0.0f;
        leftRumbleTimer = 0.0f;
    }

    /// <summary>
    /// 左スティックの入力
    /// </summary>
    /// <returns>左スティックの入力（デッドを加味）</returns>
    public Vector2 GetLeftStick()
    {
        Vector2 returnValue = new Vector2(
            (float)mainControllerInputState.LeftStick.XAxis,
            (float)mainControllerInputState.LeftStick.YAxis);

        //デッド X
        if (Mathf.Abs(returnValue.x) < dead_LeftStick && Mathf.Abs(returnValue.y) < dead_LeftStick)
        {
            returnValue.x = 0.0f;
            returnValue.y = 0.0f;
        }

        return returnValue;
    }

    /// <summary>
    /// 右スティックの入力
    /// </summary>
    /// <returns>右スティックの入力（デッドを加味）</returns>
    public Vector2 GetRightStick()
    {
        Vector2 returnValue = new Vector2(
            (float)mainControllerInputState.RightStick.XAxis,
            (float)mainControllerInputState.RightStick.YAxis);

        //デッド X
        if (Mathf.Abs(returnValue.x) < dead_RightStick && Mathf.Abs(returnValue.y) < dead_RightStick)
        {
            returnValue.x = 0.0f;
            returnValue.y = 0.0f;
        }

        return returnValue;
    }

    /// <summary>
    /// 右トリガーの抵抗を設定する（連打感触）
    /// </summary>
    /// <param name="_startPosition">抵抗の開始地点[0 ~ 1]</param>
    /// <param name="_force">抵抗の大きさ[0 ~ 1]</param>
    /// <param name="_frequency">トリガー抵抗の振動度合[0 ~ 1]</param>
    /// <param name="_keepEffect">握っている間も振動をする[true false]</param>
    public void SetRightTriggerEffect_KeepEffect(float _startPosition, float _force, float _frequency, bool _keepEffect)
    {
        output.RightTriggerEffect.InitializeExtendedEffect(_startPosition, _force, _force, _force, _frequency, _keepEffect);
    }

    /// <summary>
    /// 右トリガーの抵抗を設定する（抵抗地点設定）
    /// </summary>
    /// <param name="_startPosition">抵抗の開始地点[0 ~ 1]</param>
    /// <param name="_endPosition">抵抗の終了地点[0 ~ 1]</param>
    /// <param name="_force">抵抗の大きさ[0 ~ 1]</param>
    public void SetRightTriggerEffect_Position(float _startPosition, float _endPosition,float _force)
    {
        output.RightTriggerEffect.InitializeSectionResistanceEffect(_startPosition, _endPosition, _force);
    }

    /// <summary>
    /// 左トリガーの抵抗を設定する（連打感触）
    /// </summary>
    /// <param name="_startPosition">抵抗の開始地点[0 ~ 1]</param>
    /// <param name="_force">抵抗の大きさ[0 ~ 1]</param>
    /// <param name="_frequency">トリガー抵抗の振動度合[0 ~ 1]</param>
    /// <param name="_keepEffect">握っている間も振動をする[true false]</param>
    public void SetLeftTriggerEffect_KeepEffect(float _startPosition, float _force, float _frequency, bool _keepEffect)
    {
        output.LeftTriggerEffect.InitializeExtendedEffect(_startPosition, _force, _force, _force, _frequency, _keepEffect);
    }

    /// <summary>
    /// 左トリガーの抵抗を設定する（抵抗地点設定）
    /// </summary>
    /// <param name="_startPosition">抵抗の開始地点[0 ~ 1]</param>
    /// <param name="_endPosition">抵抗の終了地点[0 ~ 1]</param>
    /// <param name="_force">抵抗の大きさ[0 ~ 1]</param>
    public void SetLeftTriggerEffect_Position(float _startPosition, float _endPosition, float _force)
    {
        output.RightTriggerEffect.InitializeSectionResistanceEffect(_startPosition, _endPosition, _force);
    }

    /// <summary>
    /// 右トリガーの抵抗を無効にする
    /// </summary>
    public void SetRightTriggerNoEffect()
    {
        output.RightTriggerEffect.InitializeNoResistanceEffect();
    }

    /// <summary>
    /// 左トリガーの抵抗を無効にする
    /// </summary>
    public void SetLeftTriggerNoEffect()
    {
        output.LeftTriggerEffect.InitializeNoResistanceEffect();
    }
}
