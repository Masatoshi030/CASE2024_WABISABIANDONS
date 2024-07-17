using DualSenseUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    [SerializeField, Header("選択中のボタンカウント"), ReadOnly]
    int selectButtonCount = 0;

    int lastSelectButtonCount = 0;

    [SerializeField, Header("ループ　有効")]
    bool bLoop = false;

    [SerializeField, Header("連続入力間隔")]
    float continuousCoolDownTime = 0.25f;

    [SerializeField, Header("ボタンの拡大率")]
    Vector2 activeButtonScale = Vector2.one;

    [SerializeField, Header("ボタンリスト"), ReadOnly]
    GameObject[] buttons;

    Vector2 startButtonScale;

    float continuousCoolDownTimer = 0.0f;

    [SerializeField, Header("ボタン選択音")]
    AudioClip selectSound;

    [SerializeField, Header("ボタン決定音")]
    AudioClip enterSound;

    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = this.GetComponent<AudioSource>();

        //ボタンの領域を確保
        buttons = new GameObject[transform.childCount];

        //ボタン取得
        for(int i = 0; i< transform.childCount;i++)
        {
            buttons[i] = transform.GetChild(i).gameObject;
        }

        startButtonScale = buttons[0].transform.localScale;

        //選択中のボタン
        buttons[selectButtonCount].transform.localScale = new Vector3(startButtonScale.x * activeButtonScale.x, startButtonScale.y * activeButtonScale.y, 1.0f);
        buttons[selectButtonCount].transform.GetChild(1).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (DualSense_Manager.instance.GetInputState().LeftStick.YAxis > 0.8f ||
            DualSense_Manager.instance.GetInputState().DPadUpButton == ButtonState.NewDown)
        {
            if (continuousCoolDownTimer <= 0.0f)
            {

                //タイマーリセット
                continuousCoolDownTimer = continuousCoolDownTime;

                //ボタン更新処理
                ChangeButton(-1);
            }

            if (continuousCoolDownTimer > 0.0f)
            {
                continuousCoolDownTimer -= Time.deltaTime;
            }
        }
        else if (DualSense_Manager.instance.GetInputState().LeftStick.YAxis < -0.8f ||
                    DualSense_Manager.instance.GetInputState().DPadDownButton == ButtonState.NewDown)
        {
            if (continuousCoolDownTimer <= 0.0f)
            {

                //タイマーリセット
                continuousCoolDownTimer = continuousCoolDownTime;

                //ボタン更新処理
                ChangeButton(1);
            }

            if (continuousCoolDownTimer > 0.0f)
            {
                continuousCoolDownTimer -= Time.deltaTime;
            }
        }
        else
        {
            //タイマーリセット
            continuousCoolDownTimer = 0.0f;
        }

        if(DualSense_Manager.instance.GetInputState().CrossButton == ButtonState.NewDown)
        {
            if (TransitionController.instance.bTransitioning == false)
            {
                //決定音再生
                myAudioSource.PlayOneShot(enterSound);

                switch (selectButtonCount)
                {
                    //ステージセレクト
                    case 0:
                        TransitionController.instance.SetEventFunction_SceneChangeSetName("Select");
                        TransitionController.instance.SetFadeOut();

                        break;

                    case 1:
                        break;

                    //アプリケーション終了
                    case 2:
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                        break;

                    default:
                        break;
                }
            }
        }
    }

    void ChangeButton(int _addCount)
    {

        int countBuf = selectButtonCount + _addCount;

        //決定音再生
        //myAudioSource.PlayOneShot(selectSound);

        //カウント限界処理
        if (bLoop)
        {
            //最初に戻ると最後に行く
            if (countBuf < 0)
            {
                countBuf = buttons.Length - 1;

                //前回のカウントを記録
                lastSelectButtonCount = selectButtonCount;

                //カウントを進める
                selectButtonCount = countBuf;

                //振動
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
            }
            //最後まで行くと最初に戻る
            else if (countBuf >= buttons.Length)
            {
                countBuf = 0;

                //前回のカウントを記録
                lastSelectButtonCount = selectButtonCount;

                //カウントを進める
                selectButtonCount = countBuf;

                //振動
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
            }
            else
            {
                //前回のカウントを記録
                lastSelectButtonCount = selectButtonCount;

                //カウントを進める
                selectButtonCount = countBuf;

                //振動
                DualSense_Manager.instance.SetRightRumble(0.1f, 0.05f);
            }
        }
        else
        {
            //最初にとどまる
            if (countBuf < 0)
            {
                //振動
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
            }
            //最後にとどまる
            else if (countBuf >= buttons.Length)
            {
                //振動
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
            }
            else
            {
                //前回のカウントを記録
                lastSelectButtonCount = selectButtonCount;

                //カウントを進める
                selectButtonCount = countBuf;

                //振動
                DualSense_Manager.instance.SetRightRumble(0.1f, 0.05f);
            }
        }

        //前回選択していたボタン
        buttons[lastSelectButtonCount].transform.localScale = new Vector3(startButtonScale.x, startButtonScale.y, 1.0f);
        buttons[lastSelectButtonCount].transform.GetChild(1).gameObject.SetActive(false);

        //選択中のボタン
        buttons[selectButtonCount].transform.localScale = new Vector3(startButtonScale.x * activeButtonScale.x, startButtonScale.y * activeButtonScale.y, 1.0f);
        buttons[selectButtonCount].transform.GetChild(1).gameObject.SetActive(true);
    }
}
