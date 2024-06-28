using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StoryManager : MonoBehaviour
{

    [SerializeField, Header("ストーリーアニメーション")]
    Animator storyAnimation;

    [SerializeField, Header("トランジションアニメーション")]
    Animator transitionAnimation;

    [SerializeField, Header("ストーリーの最大カウント")]
    int storyCount = 0;

    [SerializeField, Header("ストーリーのカウント"), ReadOnly]
    int storyMaxCount = 3;

    [SerializeField, Header("操作をロック")]
    bool bLock = false;

    AudioSource myAudioSource;

    [SerializeField, Header("ボタン音")]
    AudioClip buttonSound;

    [SerializeField, Header("トランジション音")]
    AudioClip transitionSound;

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bLock == false)
        {
            //次へ進む
            if (DualSense_Manager.instance.GetInputState().CircleButton == DualSenseUnity.ButtonState.NewDown)
            {
                if (storyCount < storyMaxCount)
                {
                    //雲の遷移演出
                    transitionAnimation.SetTrigger("tClose");

                    //指定時間後に遷移開始
                    ChangeStory(1);

                    //ボタン音再生
                    myAudioSource.PlayOneShot(buttonSound);

                    //ページ音再生
                    myAudioSource.PlayOneShot(transitionSound);
                }
            }

            //前へ戻る
            if (DualSense_Manager.instance.GetInputState().CrossButton == DualSenseUnity.ButtonState.NewDown)
            {
                if (0 < storyCount)
                {
                    //雲の遷移演出
                    transitionAnimation.SetTrigger("tClose");

                    //指定時間後に遷移開始
                    ChangeStory(-1);

                    //ボタン音再生
                    myAudioSource.PlayOneShot(buttonSound);

                    //ページ音再生
                    myAudioSource.PlayOneShot(transitionSound);
                }
            }
        }
    }

    async void ChangeStory(int _AddCount)
    {
        //操作をロック
        bLock = true;

        // 待機
        await Task.Delay((int)(1000));

        //ストーリーを一つ進める
        storyCount += _AddCount;
        storyAnimation.SetInteger("StoryCount", storyCount);

        //アニメーションが終了するまで待つ
        await Task.Delay((int)(2000));

        //操作をロック解除
        bLock = false;
    }
}
