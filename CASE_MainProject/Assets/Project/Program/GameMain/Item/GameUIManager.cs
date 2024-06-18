using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class GameUIManager : MonoBehaviour
{

    public static GameUIManager instance;

    [SerializeField, Header("ポーズ")]
    GameObject pauseObj;

    [SerializeField, Header("パネルフェードアニメーション")]
    Animator panelFadeAnimator;

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
    }

    public void SetPause(bool _active)
    {
        //表示・非表示　切り替え
        pauseObj.SetActive(_active);

        //バイブレーション　強制停止
        DualSense_Manager.instance.StopLeftRumble();
        DualSense_Manager.instance.StopRightRumble();

        //タイムスケール変更
        if (pauseObj.activeSelf)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
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
}
