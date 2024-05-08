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

    void Start()
    {
        selectButton = 0;

        // ボタン非アクティブにする
        SaikaiObjectToActivate.SetActive(false);
        ReStartObjectToActivate.SetActive(false);
        SelectObjectToActivate.SetActive(false);
    }

    void Update()
    {
        noTouchTime += Time.deltaTime;
        
        //下ボタン処理
        if (DualSense_Manager.instance.GetInputState().DPadDownButton == DualSenseUnity.ButtonState.Down)
        {
            //クールタイムが上がっていたらボタン入力処理
            if (noTouchTime > buttonCoolTime)
            {
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.04f);
                selectButton++;
                timeElapsed = 0.0f;
            }
            timeElapsed += Time.deltaTime;
            
            //一定時間押していると実行される処理
            if (timeElapsed > timeOut)
            {
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.04f);
                selectButton++;
                timeElapsed = 0.0f;
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
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.04f);
                selectButton--;
                timeElapsed = 0.0f;
            }

            timeElapsed += Time.deltaTime;

            //一定時間押していると実行される処理
            if (timeElapsed > timeOut)
            {
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.04f);

                selectButton--;
                timeElapsed = 0.0f;
            }

            if (selectButton < 0)
            {
                selectButton = 2;
            }

            //ボタンが触られたのでクールタイム初期化
            noTouchTime = 0.0f;
        }

        //if(DualSense_Manager.instance.GetLeftStick().y == 1.0f)
        // {

        // }


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
}
