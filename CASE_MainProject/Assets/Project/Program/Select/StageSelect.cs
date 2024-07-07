using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class StageSelect : MonoBehaviour
{
    [SerializeField, Header("クリアステージの数")]
    int checkNum;
    [SerializeField, Header("クールタイム時間保存")]
    float timeElapsed;
    [SerializeField, Header("UP,DOWNボタン長押しセレクトクールタイム")]
    float timeOut;
    [SerializeField, Header("UP,DOWN触っていない時間")]
    float noTouchTime;
    [SerializeField, Header("UP,DOWNボタンのクールタイム")]
    float buttonCoolTime;
    [SerializeField, Header("ページ設定")]
    GameObject[] papers;

    [SerializeField, Header("効果音再生ソース")]
    AudioSource myAudioSource;

    [SerializeField, Header("カーソル移動効果音")]
    AudioClip cursorMoveSound;

    [SerializeField, Header("ステージ決定音")]
    AudioClip stageEnterSound;

    //定義
    static int clearPage;                //現在のクリアページ
    static int clearSelectPage;    //現在のクリアセレクト数
    static int nowPage;     //今のページ
    static int nowSelect;  //今のセレクトステージ
    static int maxPage = 4;    //ページの最大
    static int minPage = 0;     //ページの最小
    static int maxSelectStage = 6;     //ステージの最大
    static int minSelectStage = 0;     //ステージの最小

    public GameObject[,] stageArray = new GameObject[4, 6];     //全ステージ数の配列
    public GameObject[,] checkArray = new GameObject[4, 6];     //ステージのクリア状況確認配列

    bool[,] bClear = new bool[4, 6];

    // Start関数
    void Start()
    {

        //ステージのセレクトUI格納
        for (int i = minPage; i < maxPage; i++)
        {
            for (int j = minSelectStage; j < maxSelectStage; j++)
            {
                stageArray[i, j] = papers[i].transform.GetChild(j).transform.GetChild(1).gameObject;
                checkArray[i, j] = papers[i].transform.GetChild(j).transform.GetChild(0).gameObject;
                checkArray[i, j].SetActive(true);

                string stageWord = "";

                //ワールド数
                switch (i)
                {
                    case 0:
                        stageWord = "A";
                        break;
                    case 1:
                        stageWord = "B";
                        break;
                    case 2:
                        stageWord = "C";
                        break;
                    case 3:
                        stageWord = "D";
                        break;
                    default:
                        stageWord = "A";
                        break;
                }

                string stageDataName = "SELECT_CLEAR[" + stageWord + "_" + (j + 1).ToString() + "]";

                bClear[i, j] = Convert.ToBoolean(PlayerPrefs.GetInt(stageDataName));

                if (bClear[i, j] == true)
                {
                    checkArray[i, j].GetComponent<Animator>().SetBool("bCheck", true);
                    checkArray[i, j].SetActive(true);
                }
            }
        }

        //選択していたステージ
        nowSelect = PlayerPrefs.GetInt("SELECT[SelectStageCount]");
    }

    // Update関数
    void Update()
    {
        //ページセレクトスクリプトから今のページを持ってくる
        nowPage = PageSelect.selectPage;

        //触っていない時間格納
        noTouchTime += Time.deltaTime;

        //下ボタン処理
        if (DualSense_Manager.instance.GetInputState().DPadDownButton == DualSenseUnity.ButtonState.Down)
        {
            //クールタイムが上がっていたらボタン入力処理
            if (noTouchTime > buttonCoolTime)
            {
                DualSense_Manager.instance.SetRightRumble(0.1f, 0.05f);
                myAudioSource.PlayOneShot(cursorMoveSound);
                nowSelect++;
                timeElapsed = 0.0f;
            }
            timeElapsed += Time.deltaTime;

            //一定時間押していると実行される処理
            if (timeElapsed > timeOut)
            {
                DualSense_Manager.instance.SetRightRumble(0.1f, 0.05f);
                myAudioSource.PlayOneShot(cursorMoveSound);
                nowSelect++;
                timeElapsed = 0.0f;
            }

            //上限に達したら上に戻る処理
            if (nowSelect > maxSelectStage - 1)
            {
                nowSelect = minSelectStage;
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
                DualSense_Manager.instance.SetRightRumble(0.1f, 0.05f);
                myAudioSource.PlayOneShot(cursorMoveSound);
                nowSelect--;
                timeElapsed = 0.0f;
            }

            timeElapsed += Time.deltaTime;

            //一定時間押していると実行される処理
            if (timeElapsed > timeOut)
            {
                DualSense_Manager.instance.SetRightRumble(0.1f, 0.05f);
                myAudioSource.PlayOneShot(cursorMoveSound);
                nowSelect--;
                timeElapsed = 0.0f;
            }
            //上限に達したら下に戻る処理
            if (nowSelect < minSelectStage)
            {
                nowSelect = maxSelectStage - 1;
            }

            //ボタンが触られたのでクールタイム初期化
            noTouchTime = 0.0f;
        }

        //セレクトされていない状態にする処理
        for (int i = minPage; i < maxPage; i++)
        {
            for (int j = minSelectStage; j < maxSelectStage; j++)
            {
                if (stageArray[i, j] != null)
                {
                    stageArray[i, j].SetActive(false);
                }

                if (bClear[i, j] == true)
                {
                    checkArray[i, j].GetComponent<Animator>().SetBool("bCheck", true);
                    checkArray[i, j].SetActive(true);
                }
            }
        }

        //セレクトされているボタンをActiveにする処理
        SelectActive(nowPage, nowSelect);

        //決定ボタン
        if (DualSense_Manager.instance.GetInputState().OptionsButton == DualSenseUnity.ButtonState.Down)
        {

            string stageName = "";

            //ワールド数
            switch(nowPage)
            {
                case 0:
                    stageName = "A";
                    break;
                case 1:
                    stageName = "B";
                    break;
                case 2:
                    stageName = "C";
                    break;
                case 3:
                    stageName = "D";
                    break;
                default:
                    stageName = "A";
                    break;
            }

            //ステージ数
            stageName += "_" + (nowSelect + 1).ToString();

            Debug.Log(stageName);

            this.GetComponent<SceneChanger>().SceneChange(stageName);

            //決定オン再生
            myAudioSource.PlayOneShot(stageEnterSound);

            //セレクトデータ保存
            PlayerPrefs.SetInt("SELECT[SelectPageCount]", nowPage);
            PlayerPrefs.SetInt("SELECT[SelectStageCount]", nowSelect);
        }
    }

    //セレクトされているフレームをActiveにする処理
    private void SelectActive(int _nowPage,int _nowSelect)
    {
        stageArray[_nowPage, _nowSelect].SetActive(true);
    }
}
