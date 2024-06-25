using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{

    AudioSource BGM_AudioSource;

    float startVolume = 0.0f;

    [SerializeField, Header("�t�F�[�h�̑��x����")]
    float fadeTime = 2.0f;

    [SerializeField, Header("�t�F�[�h�̃^�C�}�[")]
    float fadeTimer = 0.0f;

    enum BGM_STATE
    { FadeIn, Play, FadeOut, End};
    [SerializeField, Header("���y�̍Đ����"), Toolbar(typeof(BGM_STATE), "BGMState")]
    BGM_STATE BGMState = BGM_STATE.FadeIn;

    // Start is called before the first frame update
    void Start()
    {
        BGM_AudioSource = this.GetComponent<AudioSource>();

        BGMState = BGM_STATE.FadeIn;

        startVolume = BGM_AudioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if(BGMState == BGM_STATE.FadeIn)
        {
            fadeTimer += Time.deltaTime;

            //���ʂ���`���
            BGM_AudioSource.volume = Mathf.Lerp(0.0f, startVolume, fadeTimer / fadeTime);

            //�t�F�[�h�C���I��
            if(fadeTimer > fadeTime)
            {
                BGMState = BGM_STATE.Play;

                BGM_AudioSource.volume = startVolume;

                fadeTimer = 0.0f;
            }
        }

        if (BGMState == BGM_STATE.FadeOut)
        {
            fadeTimer += Time.deltaTime;

            //���ʂ���`���
            BGM_AudioSource.volume = Mathf.Lerp(startVolume, 0.0f, fadeTimer / fadeTime);

            //�t�F�[�h�C���I��
            if (fadeTimer > fadeTime)
            {
                BGMState = BGM_STATE.End;

                BGM_AudioSource.volume = 0.0f;

                fadeTimer = 0.0f;
            }
        }
    }

    public void SetFadeStop(float _fadeTime)
    {
        fadeTime = _fadeTime;

        BGMState = BGM_STATE.FadeOut;
    }
}
