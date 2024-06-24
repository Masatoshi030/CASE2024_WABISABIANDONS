using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GetImagesInCanvas : MonoBehaviour
{
    [SerializeField, Header("��ʂ̏�ԁ@0:�ĊJ/1:���X�^�[�g/2:�Z���N�g")]
    int selectButton;

    [Header("�\���{�^���C���X�g�I��")]
    [SerializeField, Header("�ĊJ�{�^���@ON/OFF")]
    GameObject SaikaiObjectToActivate;
    [SerializeField] 
    GameObject SaikaiObjectToDeactivate;

    [SerializeField, Header("���X�^�[�g�@ON/OFF")]
    GameObject ReStartObjectToActivate;
    [SerializeField]
    GameObject ReStartObjectToDeactivate;

    [SerializeField, Header("�Z���N�g�{�^���@ON/OFF")]
    GameObject SelectObjectToActivate;
    [SerializeField]
    GameObject SelectObjectToDeactivate;

     [SerializeField, Header("���ԕۑ�")]
     float timeElapsed;
    [SerializeField, Header("�{�^���̃Z���N�g���s����Ԏ���")]
    float timeOut;
    [SerializeField, Header("�G���Ă��Ȃ�����")]
    float noTouchTime;
    [SerializeField, Header("�{�^���̃N�[���^�C��")]
    float buttonCoolTime;

    AudioSource myAudioSource;

    [SerializeField, Header("���ʉ��N���b�v���X�g")]
    AudioClip[] SoundClips;

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();

        selectButton = 0;
        timeOut = 0.2f;
        buttonCoolTime = 0.1f;

        // �{�^����A�N�e�B�u�ɂ���
        SaikaiObjectToActivate.SetActive(false);
        ReStartObjectToActivate.SetActive(false);
        SelectObjectToActivate.SetActive(false);
    }

    void Update()
    {
        noTouchTime += Time.unscaledDeltaTime;

        //���{�^������
        if (DualSense_Manager.instance.GetInputState().DPadDownButton == DualSenseUnity.ButtonState.Down)
        {
            //�N�[���^�C�����オ���Ă�����{�^�����͏���
            if (noTouchTime > buttonCoolTime)
            {
                ChangeButton(1);
            }
            timeElapsed += Time.unscaledDeltaTime;
            
            //��莞�ԉ����Ă���Ǝ��s����鏈��
            if (timeElapsed > timeOut)
            {
                ChangeButton(1);
            }

            if (selectButton > 2)
            {
                selectButton = 0;
            }

            //�{�^�����G��ꂽ�̂ŃN�[���^�C��������
            noTouchTime = 0.0f;
        }
       
       //��{�^������
        if (DualSense_Manager.instance.GetInputState().DPadUpButton == DualSenseUnity.ButtonState.Down)
        {
            //�N�[���^�C�����オ���Ă�����{�^�����͏���
            if (noTouchTime > buttonCoolTime)
            {
                ChangeButton(-1);
            }

            timeElapsed += Time.unscaledDeltaTime;

            //��莞�ԉ����Ă���Ǝ��s����鏈��
            if (timeElapsed > timeOut)
            {
                ChangeButton(-1);
            }

            if (selectButton < 0)
            {
                selectButton = 2;
            }

            //�{�^�����G��ꂽ�̂ŃN�[���^�C��������
            noTouchTime = 0.0f;
        }

        if (DualSense_Manager.instance.GetLeftStick().y > 0.5f &&
            1.0f <= DualSense_Manager.instance.GetLeftStick().y)
        {
            //�N�[���^�C�����オ���Ă�����{�^�����͏���
            if (noTouchTime > buttonCoolTime)
            {
                ChangeButton(-1);
            }

            timeElapsed += Time.unscaledDeltaTime;

            //��莞�ԉ����Ă���Ǝ��s����鏈��
            if (timeElapsed > timeOut)
            {
                ChangeButton(-1);
            }

            if (selectButton < 0)
            {
                selectButton = 2;
            }

            //�{�^�����G��ꂽ�̂ŃN�[���^�C��������
            noTouchTime = 0.0f;
        }

        if (-0.5f>DualSense_Manager.instance.GetLeftStick().y &&
             DualSense_Manager.instance.GetLeftStick().y<= - 1.0f)
        {
            //�N�[���^�C�����オ���Ă�����{�^�����͏���
            if (noTouchTime > buttonCoolTime)
            {
                ChangeButton(1);
            }

            timeElapsed += Time.unscaledDeltaTime;

            //��莞�ԉ����Ă���Ǝ��s����鏈��
            if (timeElapsed > timeOut)
            {
                ChangeButton(1);
            }

            if (selectButton > 2)
            {
                selectButton = 0;
            }

            //�{�^�����G��ꂽ�̂ŃN�[���^�C��������
            noTouchTime = 0.0f;
        }

        if(DualSense_Manager.instance.GetInputState().CircleButton== DualSenseUnity.ButtonState.NewDown)
        {

            //���Ԃ�߂�
            Time.timeScale = 1.0f;

            switch (selectButton)
            {
                case 0:

                    GameUIManager.instance.SetPause(false);

                    break;

                case 1:

                    this.GetComponent<SceneChanger>().Reload();

                    break;

                case 2:

                    this.GetComponent<SceneChanger>().SceneChange("Select");

                    break;
            }
        }

        if (selectButton ==0)
        {
            // SaikaiObjectToActivate���A�N�e�B�u�ɂ���
            SaikaiObjectToActivate.SetActive(true);
            // SaikaiObjectToDeactivate���A�N�e�B�u�ɂ���
            SaikaiObjectToDeactivate.SetActive(false);
        }
        else
        {
            // SaikaiObjectToActivate���A�N�e�B�u�ɂ���
            SaikaiObjectToActivate.SetActive(false);
            // SaikaiObjectToDeactivate���A�N�e�B�u�ɂ���
            SaikaiObjectToDeactivate.SetActive(true);
        }

        if (selectButton == 1)
        {
            // ReStartObjectToActivate���A�N�e�B�u�ɂ���
            ReStartObjectToActivate.SetActive(true);
            // ReStartObjectToDeactivate���A�N�e�B�u�ɂ���
            ReStartObjectToDeactivate.SetActive(false);
        }
        else
        {
            // ReStartObjectToActivate���A�N�e�B�u�ɂ���
            ReStartObjectToActivate.SetActive(false);
            // ReStartObjectToDeactivate���A�N�e�B�u�ɂ���
            ReStartObjectToDeactivate.SetActive(true);
        }

        if (selectButton == 2)
        {
            // SelectObjectToActivate���A�N�e�B�u�ɂ���
            SelectObjectToActivate.SetActive(true);
            // SelectObjectToDeactivate���A�N�e�B�u�ɂ���
            SelectObjectToDeactivate.SetActive(false);
        }
        else
        {
            // SelectObjectToActivate���A�N�e�B�u�ɂ���
            SelectObjectToActivate.SetActive(false);
            // SelectObjectToDeactivate���A�N�e�B�u�ɂ���
            SelectObjectToDeactivate.SetActive(true);
        }
    }

    void ChangeButton(int _plusCount)
    {
        //DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
        selectButton += _plusCount;
        timeElapsed = 0.0f;

        myAudioSource.PlayOneShot(SoundClips[0]);
    }
}