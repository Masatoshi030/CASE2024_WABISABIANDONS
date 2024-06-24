using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GetImagesInCanvas : MonoBehaviour
{
    [SerializeField, Header("画面の状態　0:再開/1:リスタート/2:セレクト")]
    int selectButton;

    [Header("表示ボタンイラスト選択")]
    [SerializeField, Header("再開ボタン　ON/OFF")]
    GameObject SaikaiObjectToActivate;
    [SerializeField] 
    GameObject SaikaiObjectToDeactivate;

    [SerializeField, Header("リスタート　ON/OFF")]
    GameObject ReStartObjectToActivate;
    [SerializeField]
    GameObject ReStartObjectToDeactivate;

    [SerializeField, Header("セレクトボタン　ON/OFF")]
    GameObject SelectObjectToActivate;
    [SerializeField]
    GameObject SelectObjectToDeactivate;

     [SerializeField, Header("時間保存")]
     float timeElapsed;
    [SerializeField, Header("ボタンのセレクト実行する間時間")]
    float timeOut;
    [SerializeField, Header("触っていない時間")]
    float noTouchTime;
    [SerializeField, Header("ボタンのクールタイム")]
    float buttonCoolTime;

    AudioSource myAudioSource;

    [SerializeField, Header("効果音クリップリスト")]
    AudioClip[] SoundClips;

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();

        selectButton = 0;
        timeOut = 0.2f;
        buttonCoolTime = 0.1f;

        // ボタン非アクティブにする
        SaikaiObjectToActivate.SetActive(false);
        ReStartObjectToActivate.SetActive(false);
        SelectObjectToActivate.SetActive(false);
    }

    void Update()
    {
        noTouchTime += Time.unscaledDeltaTime;

        //下ボタン処理
        if (DualSense_Manager.instance.GetInputState().DPadDownButton == DualSenseUnity.ButtonState.Down)
        {
            //クールタイムが上がっていたらボタン入力処理
            if (noTouchTime > buttonCoolTime)
            {
                ChangeButton(1);
            }
            timeElapsed += Time.unscaledDeltaTime;
            
            //一定時間押していると実行される処理
            if (timeElapsed > timeOut)
            {
                ChangeButton(1);
            }

            if (selectButton > 2)
            {
                selectButton = 0;
            }

            //ボタンが触られたのでクールタイム初期化
            noTouchTime = 0.0f;
        }
       
       //上ボタン処理
        if (DualSense_Manager.instance.GetInputState().DPadUpButton == DualSenseUnity.ButtonState.Down)
        {
            //クールタイムが上がっていたらボタン入力処理
            if (noTouchTime > buttonCoolTime)
            {
                ChangeButton(-1);
            }

            timeElapsed += Time.unscaledDeltaTime;

            //一定時間押していると実行される処理
            if (timeElapsed > timeOut)
            {
                ChangeButton(-1);
            }

            if (selectButton < 0)
            {
                selectButton = 2;
            }

            //ボタンが触られたのでクールタイム初期化
            noTouchTime = 0.0f;
        }

        if (DualSense_Manager.instance.GetLeftStick().y > 0.5f &&
            1.0f <= DualSense_Manager.instance.GetLeftStick().y)
        {
            //クールタイムが上がっていたらボタン入力処理
            if (noTouchTime > buttonCoolTime)
            {
                ChangeButton(-1);
            }

            timeElapsed += Time.unscaledDeltaTime;

            //一定時間押していると実行される処理
            if (timeElapsed > timeOut)
            {
                ChangeButton(-1);
            }

            if (selectButton < 0)
            {
                selectButton = 2;
            }

            //ボタンが触られたのでクールタイム初期化
            noTouchTime = 0.0f;
        }

        if (-0.5f>DualSense_Manager.instance.GetLeftStick().y &&
             DualSense_Manager.instance.GetLeftStick().y<= - 1.0f)
        {
            //クールタイムが上がっていたらボタン入力処理
            if (noTouchTime > buttonCoolTime)
            {
                ChangeButton(1);
            }

            timeElapsed += Time.unscaledDeltaTime;

            //一定時間押していると実行される処理
            if (timeElapsed > timeOut)
            {
                ChangeButton(1);
            }

            if (selectButton > 2)
            {
                selectButton = 0;
            }

            //ボタンが触られたのでクールタイム初期化
            noTouchTime = 0.0f;
        }

        if(DualSense_Manager.instance.GetInputState().CircleButton== DualSenseUnity.ButtonState.NewDown)
        {

            //時間を戻す
            Time.timeScale = 1.0f;

            switch (selectButton)
            {
                case 0:

                    GameUIManager.instance.SetPause(false);

                    break;

                case 1:

                    this.GetComponent<SceneChanger>().Reload();

                    break;

                case 2:

                    this.GetComponent<SceneChanger>().SceneChange("Select");

                    break;
            }
        }

        if (selectButton ==0)
        {
            // SaikaiObjectToActivateをアクティブにする
            SaikaiObjectToActivate.SetActive(true);
            // SaikaiObjectToDeactivateを非アクティブにする
            SaikaiObjectToDeactivate.SetActive(false);
        }
        else
        {
            // SaikaiObjectToActivateを非アクティブにする
            SaikaiObjectToActivate.SetActive(false);
            // SaikaiObjectToDeactivateをアクティブにする
            SaikaiObjectToDeactivate.SetActive(true);
        }

        if (selectButton == 1)
        {
            // ReStartObjectToActivateをアクティブにする
            ReStartObjectToActivate.SetActive(true);
            // ReStartObjectToDeactivateを非アクティブにする
            ReStartObjectToDeactivate.SetActive(false);
        }
        else
        {
            // ReStartObjectToActivateを非アクティブにする
            ReStartObjectToActivate.SetActive(false);
            // ReStartObjectToDeactivateをアクティブにする
            ReStartObjectToDeactivate.SetActive(true);
        }

        if (selectButton == 2)
        {
            // SelectObjectToActivateをアクティブにする
            SelectObjectToActivate.SetActive(true);
            // SelectObjectToDeactivateを非アクティブにする
            SelectObjectToDeactivate.SetActive(false);
        }
        else
        {
            // SelectObjectToActivateを非アクティブにする
            SelectObjectToActivate.SetActive(false);
            // SelectObjectToDeactivateをアクティブにする
            SelectObjectToDeactivate.SetActive(true);
        }
    }

    void ChangeButton(int _plusCount)
    {
        //DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
        selectButton += _plusCount;
        timeElapsed = 0.0f;

        myAudioSource.PlayOneShot(SoundClips[0]);
    }
}