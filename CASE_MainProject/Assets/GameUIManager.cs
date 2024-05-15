using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{

    public static GameUIManager instance;

    [SerializeField, Header("�|�[�Y")]
    GameObject pauseObj;

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
}
