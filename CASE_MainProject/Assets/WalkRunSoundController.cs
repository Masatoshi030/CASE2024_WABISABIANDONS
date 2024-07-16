using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkRunSoundController : MonoBehaviour
{

    AudioSource myAudioSource;

    [SerializeField, Header("歩きから走りまでの度合い 0 ~ 1"), Range(0.0f, 1.0f)]
    float walkToRunRate = 0.0f;

    [SerializeField, Header("次の音が出るまでのタイマー")]
    float runSoundCoolTime = 0.5f;

    float runSoundCoolTimer = 0.0f;

    [SerializeField, Header("加速度")]
    float runAccelerationSpeed = 1.0f;

    [SerializeField, Header("最遅速度")]
    float runMinSpeed = 0.5f;

    [SerializeField, Header("木の足音リスト")]
    AudioClip[] woodWalkList;

    bool bRun = true;


    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //走っていなければ早期リターン
        if(bRun == false)
        {
            return;
        }

        runSoundCoolTimer += Time.deltaTime * (runAccelerationSpeed * walkToRunRate + runMinSpeed);

        if(runSoundCoolTimer > runSoundCoolTime)
        {
            //速度でピッチを変える
            myAudioSource.pitch = Mathf.Lerp(1.0f, 2.0f, walkToRunRate);

            //速度で音量を変える
            myAudioSource.volume = Mathf.Lerp(0.5f, 0.8f, walkToRunRate);

            //ランダムなクリップを再生する
            myAudioSource.PlayOneShot(woodWalkList[Random.Range(0, woodWalkList.Length - 1)]);

            runSoundCoolTimer = 0.0f;
        }
    }

    public void SetWalkToRunRate(float _value)
    {
        if (_value >= 0.0f && _value <= 1.0f)
        {
            walkToRunRate = _value;

            if (bRun == false)
            {
                bRun = true;
            }
        }
        else if (_value < 0.0f)
        {
            bRun = false;
        }
    }
}
