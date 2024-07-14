using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class PageSelect : MonoBehaviour
{
    //定義領域＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
    [SerializeField, Header("ページ設定")]
    GameObject[] papers;
    [SerializeField, Header("クールタイム時間保存")]
    float timeElapsed;
    [SerializeField, Header("LEFT,RIGHTボタン長押しセレクトクールタイム")]
    float timeOut;
    [SerializeField, Header("横十字キー触っていない時間")]
    float noTouchTime;
    [SerializeField, Header("横十字キーのクールタイム")]
    float buttonCoolTime;

    [SerializeField, Header("効果音再生ソース")]
    AudioSource myAudioSource;

    [SerializeField, Header("ページめくり音")]
    AudioClip pageSwitchSound;

    static int maxPage = 4;    //ページの最大
    static int minPage = 0;     //ページの最小

    public static int selectPage;
    private Animator anim;

    // Start関数
    void Start()
    {

        //時間を戻す
        Time.timeScale = 1.0f;

        selectPage = 0;
        anim= gameObject.GetComponent<Animator>();

        //１ページ以外非表示
        papers[0].SetActive(true);
        papers[1].SetActive(false);
        papers[2].SetActive(false);
        papers[3].SetActive(false);

        //選択していたページ
        selectPage = PlayerPrefs.GetInt("SELECT[SelectPageCount]");

        //ページ切替処理
        SelectPaperProcess();
    }

    

    // Update関数
    void Update()
    {
        noTouchTime += Time.deltaTime;

        //右ボタン処理
        if (DualSense_Manager.instance.GetInputState().DPadRightButton == DualSenseUnity.ButtonState.Down)
        {
            //クールタイムが上がっていたらボタン入力処理
            if (noTouchTime > buttonCoolTime)
            {
                //Bool型のパラメーターであるbPageMoveをTrueにする
                anim.SetTrigger("tPageMove");
                myAudioSource.PlayOneShot(pageSwitchSound);
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
                selectPage++;
                timeElapsed = 0.0f;
            }

            timeElapsed += Time.unscaledDeltaTime;

            //一定時間押していると実行される処理
            if (timeElapsed > timeOut)
            {
                //Bool型のパラメーターであるbPageMoveをTrueにする
                anim.SetTrigger("tPageMove");
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
                myAudioSource.PlayOneShot(pageSwitchSound);
                selectPage++;
                timeElapsed = 0.0f;
            }

            if (selectPage > maxPage - 1)
            {
                selectPage = minPage;
            }
            //ボタンが触られたのでクールタイム初期化
            noTouchTime = 0.0f;
        }

        //左ボタン処理
        if (DualSense_Manager.instance.GetInputState().DPadLeftButton == DualSenseUnity.ButtonState.Down)
        {
            //クールタイムが上がっていたらボタン入力処理
            if (noTouchTime > buttonCoolTime)
            {
                //Bool型のパラメーターであるbPageMoveをTrueにする
                anim.SetTrigger("tPageMove");
                myAudioSource.PlayOneShot(pageSwitchSound);
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
                selectPage--;
                timeElapsed = 0.0f;
            }

            timeElapsed += Time.unscaledDeltaTime;

            //一定時間押していると実行される処理
            if (timeElapsed > timeOut)
            {
                //Bool型のパラメーターであるbPageMoveをTrueにする
                anim.SetTrigger("tPageMove");
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
                myAudioSource.PlayOneShot(pageSwitchSound);
                selectPage--;
                timeElapsed = 0.0f;
            }

            if (selectPage < minPage)
            {
                selectPage = maxPage - 1;
            }

            //ボタンが触られたのでクールタイム初期化
            noTouchTime = 0.0f;
        }

        //右スティック
        if (DualSense_Manager.instance.GetLeftStick().x > 0.5f &&
            1.0f <= DualSense_Manager.instance.GetLeftStick().x)
        {
            //クールタイムが上がっていたらボタン入力処理
            if (noTouchTime > buttonCoolTime)
            {
                //Bool型のパラメーターであるbPageMoveをTrueにする
                anim.SetTrigger("tPageMove");
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
                //ページ切り替え音再生
                myAudioSource.PlayOneShot(pageSwitchSound);
                selectPage++;
                timeElapsed = 0.0f;
            }

            timeElapsed += Time.unscaledDeltaTime;

            //一定時間押していると実行される処理
            if (timeElapsed > timeOut)
            {
                //Bool型のパラメーターであるbPageMoveをTrueにする
                anim.SetTrigger("tPageMove");
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
                //ページ切り替え音再生
                myAudioSource.PlayOneShot(pageSwitchSound);
                selectPage++;
                timeElapsed = 0.0f;
            }

            if (selectPage > maxPage - 1)
            {
                selectPage = minPage;
            }

            //ボタンが触られたのでクールタイム初期化
            noTouchTime = 0.0f;
        }

        //左スティック
        if (-0.5f > DualSense_Manager.instance.GetLeftStick().x &&
              DualSense_Manager.instance.GetLeftStick().x <= -1.0f)
        {
            //クールタイムが上がっていたらボタン入力処理
            if (noTouchTime > buttonCoolTime)
            {
                //Bool型のパラメーターであるbPageMoveをTrueにする
                anim.SetTrigger("tPageMove");
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
                //ページ切り替え音再生
                myAudioSource.PlayOneShot(pageSwitchSound);
                selectPage--;
                timeElapsed = 0.0f;
            }

            timeElapsed += Time.unscaledDeltaTime;

            //一定時間押していると実行される処理
            if (timeElapsed > timeOut)
            {
                //Bool型のパラメーターであるbPageMoveをTrueにする
                anim.SetTrigger("tPageMove");
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
                //ページ切り替え音再生
                myAudioSource.PlayOneShot(pageSwitchSound);
                selectPage--;
                timeElapsed = 0.0f;
            }

            if (selectPage < minPage)
            {
                selectPage = maxPage - 1;
            }

            //ボタンが触られたのでクールタイム初期化
            noTouchTime = 0.0f;
        }
        //ページ切替処理
        SelectPaperProcess();
    }

    private void SelectPaperProcess()
    {
        if (selectPage == 0)
        {
            papers[0].SetActive(true);
            papers[1].SetActive(false);
            papers[2].SetActive(false);
            papers[3].SetActive(false);
        }
        if (selectPage == 1)
        {
            papers[0].SetActive(false);
            papers[1].SetActive(true);
            papers[2].SetActive(false);
            papers[3].SetActive(false);
        }
        if (selectPage == 2)
        {
            papers[0].SetActive(false);
            papers[1].SetActive(false);
            papers[2].SetActive(true);
            papers[3].SetActive(false);
        }
        if (selectPage == 3)
        {
            papers[0].SetActive(false);
            papers[1].SetActive(false);
            papers[2].SetActive(false);
            papers[3].SetActive(true);
        }
    }
}
