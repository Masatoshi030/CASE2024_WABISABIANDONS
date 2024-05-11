using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PageSelect : MonoBehaviour
{
    //定義領域＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
    [SerializeField, Header("現在のページ")]
    int serectPage;

    [SerializeField, Header("ページ1")]
    GameObject paper_1;
    [SerializeField, Header("ページ2")]
    GameObject paper_2;
    [SerializeField, Header("ページ3")]
    GameObject paper_3;
    [SerializeField, Header("ページ4")]
    GameObject paper_4;

    [SerializeField, Header("時間保存")]
    float timeElapsed;
    [SerializeField, Header("ボタンのセレクト実行する間時間")]
    float timeOut;
    [SerializeField, Header("触っていない時間")]
    float noTouchTime;
    [SerializeField, Header("ボタンのクールタイム")]
    float buttonCoolTime;

    private Animator anim;

    // Start関数
    void Start()
    {
        serectPage = 1;
        anim= gameObject.GetComponent<Animator>();

        //１ページ以外非表示
        paper_1.SetActive(true);
        paper_2.SetActive(false);
        paper_3.SetActive(false);
        paper_4.SetActive(false);
    }

    

    // Update関数
    void Update()
    {
        noTouchTime += Time.deltaTime;

        if (DualSense_Manager.instance.GetInputState().RightTrigger.TriggerValue > 0.01f &&
            1.0f <= DualSense_Manager.instance.GetInputState().RightTrigger.TriggerValue)
        {
            //クールタイムが上がっていたらボタン入力処理
            if (noTouchTime > buttonCoolTime && serectPage < 4)
            {
                //Bool型のパラメーターであるbPageMoveをTrueにする
                anim.SetTrigger("tPageMove");

                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
                serectPage++;
                timeElapsed = 0.0f;
            }
            timeElapsed += Time.deltaTime;
            //ボタンが触られたのでクールタイム初期化
            noTouchTime = 0.0f;
        }

        if (DualSense_Manager.instance.GetInputState().LeftTrigger.TriggerValue > 0.01f &&
           1.0f <= DualSense_Manager.instance.GetInputState().LeftTrigger.TriggerValue)
        {
            //クールタイムが上がっていたらボタン入力処理
            if (noTouchTime > buttonCoolTime && serectPage > 1)
            {
                //Bool型のパラメーターであるbPageMoveをTrueにする
                anim.SetTrigger("tPageMove");

                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
                serectPage--;
                timeElapsed = 0.0f;
            }
            timeElapsed += Time.deltaTime;
            //ボタンが触られたのでクールタイム初期化
            noTouchTime = 0.0f;
        }



        SelectPaperProcess();
    }

    private void SelectPaperProcess()
    {
        if (serectPage == 1)
        {
            paper_1.SetActive(true);
            paper_2.SetActive(false);
            paper_3.SetActive(false);
            paper_4.SetActive(false);
        }
        if (serectPage == 2)
        {
            paper_1.SetActive(false);
            paper_2.SetActive(true);
            paper_3.SetActive(false);
            paper_4.SetActive(false);
        }
        if (serectPage == 3)
        {
            paper_1.SetActive(false);
            paper_2.SetActive(false);
            paper_3.SetActive(true);
            paper_4.SetActive(false);
        }
        if (serectPage == 4)
        {
            paper_1.SetActive(false);
            paper_2.SetActive(false);
            paper_3.SetActive(false);
            paper_4.SetActive(true);
        }
    }
}
