using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StoryManager : MonoBehaviour
{

    [SerializeField, Header("�X�g�[���[�A�j���[�V����")]
    Animator storyAnimation;

    [SerializeField, Header("�g�����W�V�����A�j���[�V����")]
    Animator transitionAnimation;

    [SerializeField, Header("�X�g�[���[�̍ő�J�E���g")]
    int storyCount = 0;

    [SerializeField, Header("�X�g�[���[�̃J�E���g"), ReadOnly]
    int storyMaxCount = 3;

    [SerializeField, Header("��������b�N")]
    bool bLock = false;

    AudioSource myAudioSource;

    [SerializeField, Header("�{�^����")]
    AudioClip buttonSound;

    [SerializeField, Header("�g�����W�V������")]
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
            //���֐i��
            if (DualSense_Manager.instance.GetInputState().CircleButton == DualSenseUnity.ButtonState.NewDown)
            {
                if (storyCount < storyMaxCount)
                {
                    //�_�̑J�ډ��o
                    transitionAnimation.SetTrigger("tClose");

                    //�w�莞�Ԍ�ɑJ�ڊJ�n
                    ChangeStory(1);

                    //�{�^�����Đ�
                    myAudioSource.PlayOneShot(buttonSound);

                    //�y�[�W���Đ�
                    myAudioSource.PlayOneShot(transitionSound);
                }
            }

            //�O�֖߂�
            if (DualSense_Manager.instance.GetInputState().CrossButton == DualSenseUnity.ButtonState.NewDown)
            {
                if (0 < storyCount)
                {
                    //�_�̑J�ډ��o
                    transitionAnimation.SetTrigger("tClose");

                    //�w�莞�Ԍ�ɑJ�ڊJ�n
                    ChangeStory(-1);

                    //�{�^�����Đ�
                    myAudioSource.PlayOneShot(buttonSound);

                    //�y�[�W���Đ�
                    myAudioSource.PlayOneShot(transitionSound);
                }
            }
        }
    }

    async void ChangeStory(int _AddCount)
    {
        //��������b�N
        bLock = true;

        // �ҋ@
        await Task.Delay((int)(1000));

        //�X�g�[���[����i�߂�
        storyCount += _AddCount;
        storyAnimation.SetInteger("StoryCount", storyCount);

        //�A�j���[�V�������I������܂ő҂�
        await Task.Delay((int)(2000));

        //��������b�N����
        bLock = false;
    }
}
