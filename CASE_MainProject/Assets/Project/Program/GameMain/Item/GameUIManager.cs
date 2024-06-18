using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class GameUIManager : MonoBehaviour
{

    public static GameUIManager instance;

    [SerializeField, Header("�|�[�Y")]
    GameObject pauseObj;

    [SerializeField, Header("�p�l���t�F�[�h�A�j���[�V����")]
    Animator panelFadeAnimator;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            // ���g���C���X�^���X�Ƃ���
            instance = this;
        }
        else
        {
            // �C���X�^���X�����ɑ��݂��Ă����玩�g����������
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(DualSense_Manager.instance.GetInputState().OptionsButton == DualSenseUnity.ButtonState.NewDown)
        {
            SetPause(!pauseObj.activeSelf);
        }
    }

    public void SetPause(bool _active)
    {
        //�\���E��\���@�؂�ւ�
        pauseObj.SetActive(_active);

        //�o�C�u���[�V�����@������~
        DualSense_Manager.instance.StopLeftRumble();
        DualSense_Manager.instance.StopRightRumble();

        //�^�C���X�P�[���ύX
        if (pauseObj.activeSelf)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public async void SetFade(bool _enable, float _waitTime)
    {
        // �w�莞�ԑҋ@
        await Task.Delay((int)(_waitTime * 1000));

        //�t�F�[�h�J�n
        panelFadeAnimator.SetBool("bEnable", _enable);

        // �w�莞�ԑҋ@
        await Task.Delay((int)(_waitTime * 1000));

        //�V�[�������[�h
        this.GetComponent<SceneChanger>().Reload();
    }
}
