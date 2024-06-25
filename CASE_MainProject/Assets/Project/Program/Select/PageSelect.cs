using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PageSelect : MonoBehaviour
{
    //定義領域＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
    [SerializeField, Header("ページ設定")]
    GameObject[] papers;
    [SerializeField, Header("横十字キー触っていない時間")]
    float noTouchTime;
    [SerializeField, Header("横十字キーのクールタイム")]
    float buttonCoolTime;

    [SerializeField, Header("効果音再生ソース")]
    AudioSource myAudioSource;

    [SerializeField, Header("ページめくり音")]
    AudioClip pageSwitchSound;

    public static int serectPage;
    private Animator anim;

    // Start関数
    void Start()
    {

        serectPage = 0;
        anim= gameObject.GetComponent<Animator>();

        //１ページ以外非表示
        papers[0].SetActive(true);
        papers[1].SetActive(false);
        papers[2].SetActive(false);
        papers[3].SetActive(false);
    }

    

    // Update関数
    void Update()
    {
        noTouchTime += Time.deltaTime;

        //右ボタン処理
        if (DualSense_Manager.instance.GetInputState().DPadRightButton == DualSenseUnity.ButtonState.Down)
        {
            //クールタイムが上がっていたらボタン入力処理
            if (noTouchTime > buttonCoolTime && serectPage < 3)
            {
                //Bool型のパラメーターであるbPageMoveをTrueにする
                anim.SetTrigger("tPageMove");

                //ページ切り替え音再生
                myAudioSource.PlayOneShot(pageSwitchSound);

                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
                serectPage++;
            }
            //ボタンが触られたのでクールタイム初期化
            noTouchTime = 0.0f;
        }

        //左ボタン処理
        if (DualSense_Manager.instance.GetInputState().DPadLeftButton == DualSenseUnity.ButtonState.Down)
        {
            //クールタイムが上がっていたらボタン入力処理
            if (noTouchTime > buttonCoolTime && serectPage > 0)
            {
                //Bool型のパラメーターであるbPageMoveをTrueにする
                anim.SetTrigger("tPageMove");

                //ページ切り替え音再生
                myAudioSource.PlayOneShot(pageSwitchSound);

                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
                serectPage--;
            }
            //ボタンが触られたのでクールタイム初期化
            noTouchTime = 0.0f;
        }
        //ページ切替処理
        SelectPaperProcess();
    }

    private void SelectPaperProcess()
    {
        if (serectPage == 0)
        {
            papers[0].SetActive(true);
            papers[1].SetActive(false);
            papers[2].SetActive(false);
            papers[3].SetActive(false);
        }
        if (serectPage == 1)
        {
            papers[0].SetActive(false);
            papers[1].SetActive(true);
            papers[2].SetActive(false);
            papers[3].SetActive(false);
        }
        if (serectPage == 2)
        {
            papers[0].SetActive(false);
            papers[1].SetActive(false);
            papers[2].SetActive(true);
            papers[3].SetActive(false);
        }
        if (serectPage == 3)
        {
            papers[0].SetActive(false);
            papers[1].SetActive(false);
            papers[2].SetActive(false);
            papers[3].SetActive(true);
        }
    }
}
