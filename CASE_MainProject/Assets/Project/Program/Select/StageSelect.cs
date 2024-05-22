using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
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
            }
        }
    }

    // Update関数
    void Update()
    {
        //ページセレクトスクリプトから今のページを持ってくる
        nowPage = PageSelect.serectPage;

        //触っていない時間格納
        noTouchTime += Time.deltaTime;

        //下ボタン処理
        if (DualSense_Manager.instance.GetInputState().DPadDownButton == DualSenseUnity.ButtonState.Down)
        {
            //クールタイムが上がっていたらボタン入力処理
            if (noTouchTime > buttonCoolTime)
            {
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.04f);
                nowSelect++;
                timeElapsed = 0.0f;
            }
            timeElapsed += Time.deltaTime;

            //一定時間押していると実行される処理
            if (timeElapsed > timeOut)
            {
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.04f);
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
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.04f);
                nowSelect--;
                timeElapsed = 0.0f;
            }

            timeElapsed += Time.deltaTime;

            //一定時間押していると実行される処理
            if (timeElapsed > timeOut)
            {
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.04f);

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
            }
        }
        //セレクトされているボタンをActiveにする処理
        SelectActive(nowPage, nowSelect);

        //チェックを表示する
        //チェックが全体のステージ数を越えていないか判定する処理
        if (checkNum >= 0 && maxPage * maxSelectStage >= checkNum)
        {
            clearPage = checkNum / maxSelectStage;
            clearSelectPage = checkNum % maxSelectStage;

            //越えている場合(クリアステージが6以上)
            if (clearPage > 0)
            {
                for (int i = 0; i < clearPage; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        checkArray[i, j].GetComponent<Animator>().SetBool("bCheck", true);
                        checkArray[i, j].SetActive(true);
                    }
                    for (int j = 0; j < clearSelectPage; j++)
                    {
                        checkArray[clearPage, j].GetComponent<Animator>().SetBool("bCheck", true);
                        checkArray[clearPage, j].SetActive(true);
                    }
                }
            }
            else  //越えていない場合(クリアステージが6以上)
            {
                for (int i = 0; i < clearSelectPage; i++)
                {
                    checkArray[clearPage, i].GetComponent<Animator>().SetBool("bCheck", true);
                    checkArray[clearPage, i].SetActive(true);
                }
            }
        }
        else if (checkNum > 24)     //24ステージ以上を選択してしまった場合
        {
            checkNum = 24;
        }
    }

    //セレクトされているフレームをActiveにする処理
    private void SelectActive(int _nowPage,int _nowSelect)
    {
        stageArray[_nowPage, _nowSelect].SetActive(true);
    }
}
