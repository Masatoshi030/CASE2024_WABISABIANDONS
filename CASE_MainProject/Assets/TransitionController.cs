using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{

    public static TransitionController instance;

    Animator transitionAnimator;

    public delegate void FadeOutEventFuction();

    FadeOutEventFuction eventFunction;

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

        //時間を戻す
        Time.timeScale = 1.0f;

        //アニメーター格納
        transitionAnimator = this.GetComponent<Animator>();

        //シーン切り替えをデフォルトイベント関数に設定
        SetEventFunction_SceneChangeSetName("Title");
    }

    /// <summary>
    /// シーン名を決めてデリゲートを設定
    /// </summary>
    /// <param name="_SceneName"></param>
    public void SetEventFunction_SceneChangeSetName(string _SceneName)
    {
        SceneChanger sceneChanger_buf = this.GetComponent<SceneChanger>();

        //遷移シーン名を変更
        sceneChanger_buf.changeSceneName = _SceneName;

        //デリゲートを設定
        eventFunction = sceneChanger_buf.SceneChange_ConfiguredScene;
    }

    public void SetFadeOut()
    {
        transitionAnimator.SetTrigger("Enable");
    }

    //関数ポインターを設定
    public void SetFadeOutEventFuction(FadeOutEventFuction _function)
    {
        eventFunction = _function;
    }

    //アニメーターに呼ばれるイベント関数
    public void TransitionEventFunction()
    {
        eventFunction();
    }
}
