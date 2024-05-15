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
        GameObject paper1 = GameObject.Find("Paper_1");
        if (paper1 != null )
        {
            //Paper_1の子オブジェクトを取得
            stageArray[0, 0] = paper1.transform.Find("Line_1_1/Select_1_1")?.gameObject;
            stageArray[0, 1] = paper1.transform.Find("Line_1_2/Select_1_2")?.gameObject;
            stageArray[0, 2] = paper1.transform.Find("Line_1_3/Select_1_3")?.gameObject;
            stageArray[0, 3] = paper1.transform.Find("Line_1_4/Select_1_4")?.gameObject;
            stageArray[0, 4] = paper1.transform.Find("Line_1_5/Select_1_5")?.gameObject;
            stageArray[0, 5] = paper1.transform.Find("Line_1_6/Select_1_6")?.gameObject;
        }

        // Paper_2オブジェクトを取得
        GameObject paper2 = GameObject.Find("Paper_2");
        if (paper2 != null)
        {
            // Paper_2の子オブジェクトを配列に割り当て
            stageArray[1, 0] = paper2.transform.Find("Line_2_1/Select_2_1")?.gameObject;
            stageArray[1, 1] = paper2.transform.Find("Line_2_2/Select_2_2")?.gameObject;
            stageArray[1, 2] = paper2.transform.Find("Line_2_3/Select_2_3")?.gameObject;
            stageArray[1, 3] = paper2.transform.Find("Line_2_4/Select_2_4")?.gameObject;
            stageArray[1, 4] = paper2.transform.Find("Line_2_5/Select_2_5")?.gameObject;
            stageArray[1, 5] = paper2.transform.Find("Line_2_6/Select_2_6")?.gameObject;
        }

        // Paper_3オブジェクトを取得
        GameObject paper3 = GameObject.Find("Paper_3");
        if (paper3 != null)
        {
            // Paper_2の子オブジェクトを配列に割り当て
            stageArray[2, 0] = paper3.transform.Find("Line_3_1/Select_3_1")?.gameObject;
            stageArray[2, 1] = paper3.transform.Find("Line_3_2/Select_3_2")?.gameObject;
            stageArray[2, 2] = paper3.transform.Find("Line_3_3/Select_3_3")?.gameObject;
            stageArray[2, 3] = paper3.transform.Find("Line_3_4/Select_3_4")?.gameObject;
            stageArray[2, 4] = paper3.transform.Find("Line_3_5/Select_3_5")?.gameObject;
            stageArray[2, 5] = paper3.transform.Find("Line_3_6/Select_3_6")?.gameObject;
        }

        // Paper_4オブジェクトを取得
        GameObject paper4 = GameObject.Find("Paper_4");
        if (paper4 != null)
        {
            // Paper_4の子オブジェクトを配列に割り当て
            stageArray[3, 0] = paper4.transform.Find("Line_4_1/Select_4_1")?.gameObject;
            stageArray[3, 1] = paper4.transform.Find("Line_4_2/Select_4_2")?.gameObject;
            stageArray[3, 2] = paper4.transform.Find("Line_4_3/Select_4_3")?.gameObject;
            stageArray[3, 3] = paper4.transform.Find("Line_4_4/Select_4_4")?.gameObject;
            stageArray[3, 4] = paper4.transform.Find("Line_4_5/Select_4_5")?.gameObject;
            stageArray[3, 5] = paper4.transform.Find("Line_4_6/Select_4_6")?.gameObject;
        }


        //for (int i = minPage; i < maxPage; i++)
        //{
        //    for (int j = minSelectStage; j < maxSelectStage; j++)
        //    {
        //        if (stageArray[i, j] != null)
        //        {
        //            Debug.Log(stageArray[i, j].name);
        //            stageArray[i, j].SetActive(false);
        //        }
        //    }
        //}
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
                    stageArray[i, j].SetActive(false);
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
        stageArray[_nowPage, _nowSelect].SetActive(true);
    }
}
