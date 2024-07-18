using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class GameUIManager : MonoBehaviour
{

    public static GameUIManager instance;

    [SerializeField, Header("ポーズ")]
    GameObject pauseObj;

    [SerializeField, Header("パネルフェードアニメーション")]
    Animator panelFadeAnimator;

    [SerializeField, Header("ScreenSmokePanel")]
    Image screenSmokePanel;

    [SerializeField, Header("画面スモークが消えるまでの時間")]
    float screenSmokeThinTime = 3.0f;

    float screenSmokeThinTimer = 0.0f;

    [SerializeField, Header("蒸気空マーク")]
    GameObject noPressureMarkObject;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(DualSense_Manager.instance.GetInputState().OptionsButton == DualSenseUnity.ButtonState.NewDown)
        {
            SetPause(!pauseObj.activeSelf);
        }

        if(pauseObj.activeSelf == true)
        {
            if(DualSense_Manager.instance.GetInputState().CircleButton == DualSenseUnity.ButtonState.NewDown)
            {
                SetPause(false);
            }
        }

        //画面スモークエフェクトの自然消滅
        if(screenSmokeThinTimer > 0.0f)
        {
            //タイマー計測
            screenSmokeThinTimer -= Time.deltaTime;

            //０に矯正
            if(screenSmokeThinTimer <= 0.0f)
            {
                screenSmokeThinTimer = 0.0f;
            }

            //線形補間でα値を計算
            Color colorbuf = screenSmokePanel.color;

            colorbuf.a = Mathf.Lerp(0.0f, 1.0f, screenSmokeThinTimer / screenSmokeThinTime);

            screenSmokePanel.color = colorbuf;
        }

        //プレイヤーの蒸気が空になったらマークを表示
        if (PlayerController.instance.bSteamEmptyCoolDown)
        {
            if (noPressureMarkObject.activeSelf == false)
            {
                noPressureMarkObject.SetActive(true);
            }
        }
        else
        {
            if (noPressureMarkObject.activeSelf == true)
            {
                noPressureMarkObject.SetActive(false);
            }
        }
    }

    public void SetPause(bool _active)
    {
        //表示・非表示　切り替え
        pauseObj.SetActive(_active);

        //バイブレーション　強制停止
        DualSense_Manager.instance.StopLeftRumble();
        DualSense_Manager.instance.StopRightRumble();

        //タイムスケール変更 視点移動の有無
        if (pauseObj.activeSelf)
        {
            Time.timeScale = 0.0f;

            //視点移動を無効
            CinemachineCameraSetting.instance.SetAimLock(true);
        }
        else
        {
            Time.timeScale = 1.0f;

            //視点移動を有効
            CinemachineCameraSetting.instance.SetAimLock(false);
        }
    }

    public async void SetFade(bool _enable, float _waitTime)
    {
        // 指定時間待機
        await Task.Delay((int)(_waitTime * 1000));

        //フェード開始
        panelFadeAnimator.SetBool("bEnable", _enable);

        // 指定時間待機
        await Task.Delay((int)(_waitTime * 1000));

        //シーンリロード
        this.GetComponent<SceneChanger>().Reload();
    }

    public void SetScreenSmoke()
    {
        //タイマーを最大時間にリセット
        screenSmokeThinTimer = screenSmokeThinTime;
    }
}
