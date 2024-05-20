using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    [SerializeField, Header("クールタイム時間保存")]
    float timeElapsed;
    [SerializeField, Header("UP,DOWNボタン長押しセレクトクールタイム")]
    float timeOut;
    [SerializeField, Header("UP,DOWN触っていない時間")]
    float noTouchTime;
    [SerializeField, Header("UP,DOWNボタンのクールタイム")]
    float buttonCoolTime;


    [SerializeField, Header("ページ1")]
    GameObject[] papers;

    //定義
    int nowPage;    //今のページ
    int nowSelect;  //今のセレクトステージ
    int maxPage = 4;    //ページの最大
    int minPage = 0;    //ページの最小
    int maxSelectStage = 6;     //ステージの最大
    int minSelectStage = 0;     //ステージの最小
    public GameObject[,] stageArray = new GameObject[4, 6];     //全ステージ数の配列

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                stageArray[i, j] = papers[i].transform.GetChild(j).gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Active状態以外は非表示にする処理
        for (int i = minPage; i < maxPage; i++)
        {
            for (int j = minSelectStage; j < maxSelectStage; j++)
            {
                if (stageArray[i, j] != null)
                {

                }
            }
        }

        //ページセレクトスクリプトから今のページを持ってくる
        nowPage = PageSelect.serectPage;

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

            if (nowSelect < minSelectStage)
            {
                nowSelect = maxSelectStage - 1;
            }

            //ボタンが触られたのでクールタイム初期化
            noTouchTime = 0.0f;
        }
        SelectActive(nowPage, nowSelect);
    }

    //セレクトされているボタンをActiveにする処理
    private void SelectActive(int _nowPage,int _nowSelect)
    {
        stageArray[_nowPage, _nowSelect].transform.GetChild(0).GetComponent<Animator>().SetBool("bCheck", true);
        stageArray[_nowPage, _nowSelect].transform.GetChild(0).gameObject.SetActive(true);
    }
}
