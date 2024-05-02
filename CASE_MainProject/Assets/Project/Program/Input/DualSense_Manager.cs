using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualSenseUnity;
using System.ComponentModel;
using UnityEngine.SceneManagement;

public class DualSense_Manager : MonoBehaviour
{
    [SerializeField,Header("�R���g���[���[�̌�")]
    private uint controllerCount;

    [SerializeField, Header("���X�e�B�b�N�̃f�b�h")]
    float dead_LeftStick = 0.05f;

    [SerializeField, Header("�E�X�e�B�b�N�̃f�b�h")]
    float dead_RightStick = 0.05f;

    [SerializeField, Header("�U���p�^�[���P")]
    AnimationCurve rumble_Type1;

    //�R���g���[���[���X�g
    private List<DualSenseController> dualSenseControllers;

    //���C���R���g���[���[
    private DualSenseController mainController;

    //���C���R���g���[���[�̏��
    private ControllerInputState mainControllerInputState;

    //�E�����u���̃^�C�}�[
    float rightRumbleTimer = 0.0f;

    //�������u���̃^�C�}�[
    float leftRumbleTimer = 0.0f;

    //�E�����u���̃^�C�}�[
    float rightRumbleTimer_type = 0.0f;

    bool bLeftTriggerEffect = false;

    bool bRightTriggerEffect = false;

    ControllerOutputState output = new ControllerOutputState();

    //���g���K�[�̒�R�̊J�n�n�_
    float leftTriggerEffect_StartPosition = 0.0f;

    //���g���K�[�̒�R�̏I���n�_
    float leftTriggerEffect_EndPosition = 0.0f;

    //���g���K�[�̒�R�̑傫��
    float leftTriggerEffect_Force = 0.0f;

    //�V���O���g���C���X�^���X
    public static DualSense_Manager instance; // �C���X�^���X�̒�`


    private void Awake()
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

    // Start is called before the first frame update
    void Start()
    {
        //�R���g���[���[�̐����擾
        controllerCount = DualSense.GetControllerCount();

        //�R���g���[���[�̃n���h����S�Ď擾
        dualSenseControllers = DualSense.GetControllers();

        //�R���g���[���[�̐����ς������X�V�������Ă�
        DualSense.ControllerCountChanged += RefreshControllers;

        //�R���g���[���[�̏�������
        RefreshControllers();

        //���C���R���g���[���[��ݒ�
        mainController = dualSenseControllers[0];

        SetLeftTriggerNoEffect();
    }

    // Update is called once per frame
    void Update()
    {
        //���C���R���g���[���[�̓��͏�Ԃ��i�[
        mainControllerInputState = mainController.GetInputState();

        //�����u���^�C�}�[���O�ɂȂ�ƒ�~
        //�E
        if(rightRumbleTimer > 0.0f)
        {
            rightRumbleTimer -= Time.deltaTime;
            if(rightRumbleTimer < 0.0f)
            {
                StopRightRumble();
                rightRumbleTimer = 0.0f;
            }
        }
        //��
        if (leftRumbleTimer > 0.0f)
        {
            leftRumbleTimer -= Time.deltaTime;
            if (leftRumbleTimer < 0.0f)
            {
                StopLeftRumble();
                leftRumbleTimer = 0.0f;
            }
        }


        if (rightRumbleTimer_type > 0.0f)
        {
            rightRumbleTimer_type -= Time.deltaTime;
            if (rightRumbleTimer_type < 0.0f)
            {
                StopLeftRumble();
                rightRumbleTimer_type = 0.0f;
            }
            else
            {
                output.LeftRumbleIntensity = rumble_Type1.Evaluate((1.0f - rightRumbleTimer_type));
            }
        }

        mainController.SetOutputState(output);
    }

    /// <summary>
    /// �R���g���[���[�̍X�V����
    /// </summary>
    private void RefreshControllers()
    {
        controllerCount = DualSense.GetControllerCount();
        dualSenseControllers = DualSense.GetControllers();
    }

    /// <summary>
    /// ���C���R���g���[���[�̓��͏�Ԏ擾
    /// </summary>
    /// <returns>���C���R���g���[���[�̓��͏��</returns>
    public ControllerInputState GetInputState()
    {
        return mainControllerInputState;
    }

    /// <summary>
    /// �ݒ莞�Ԃ̊ԁA�R���g���[���[�̉E�����u�����U��
    /// </summary>
    /// <param name="_intensity">�U���̋��� [0~1]</param>
    /// <param name="_time">�p������</param>
    public void SetRightRumble(float _intensity, float _time)
    {
        output.RightRumbleIntensity = _intensity;
        rightRumbleTimer = _time;
    }

    /// <summary>
    /// �ݒ莞�Ԃ̊ԁA�R���g���[���[�̍������u�����U��
    /// </summary>
    /// <param name="_intensity">�U���̋��� [0~1]</param>
    /// <param name="_time">�p������</param>
    public void SetLeftRumble(float _intensity, float _time)
    {
        output.LeftRumbleIntensity = _intensity;
        leftRumbleTimer = _time;
    }

    public void SetRumble_Type1()
    {
        rightRumbleTimer_type = 1.0f;
    }

    /// <summary>
    /// �R���g���[���[�̉E�����u�����~
    /// </summary>
    public void StopRightRumble()
    {
        output.RightRumbleIntensity = 0.0f;
        rightRumbleTimer = 0.0f;
    }

    /// <summary>
    /// �R���g���[���[�̍������u�����~
    /// </summary>
    public void StopLeftRumble()
    {
        output.LeftRumbleIntensity = 0.0f;
        leftRumbleTimer = 0.0f;
    }

    /// <summary>
    /// ���X�e�B�b�N�̓���
    /// </summary>
    /// <returns>���X�e�B�b�N�̓��́i�f�b�h�������j</returns>
    public Vector2 GetLeftStick()
    {
        Vector2 returnValue = new Vector2(
            (float)mainControllerInputState.LeftStick.XAxis,
            (float)mainControllerInputState.LeftStick.YAxis);

        //�f�b�h X
        if (Mathf.Abs(returnValue.x) < dead_LeftStick && Mathf.Abs(returnValue.y) < dead_LeftStick)
        {
            returnValue.x = 0.0f;
            returnValue.y = 0.0f;
        }

        return returnValue;
    }

    /// <summary>
    /// �E�X�e�B�b�N�̓���
    /// </summary>
    /// <returns>�E�X�e�B�b�N�̓��́i�f�b�h�������j</returns>
    public Vector2 GetRightStick()
    {
        Vector2 returnValue = new Vector2(
            (float)mainControllerInputState.RightStick.XAxis,
            (float)mainControllerInputState.RightStick.YAxis);

        //�f�b�h X
        if (Mathf.Abs(returnValue.x) < dead_RightStick && Mathf.Abs(returnValue.y) < dead_RightStick)
        {
            returnValue.x = 0.0f;
            returnValue.y = 0.0f;
        }

        return returnValue;
    }

    /// <summary>
    /// �E�g���K�[�̒�R��ݒ肷��i�A�Ŋ��G�j
    /// </summary>
    /// <param name="_startPosition">��R�̊J�n�n�_[0 ~ 1]</param>
    /// <param name="_force">��R�̑傫��[0 ~ 1]</param>
    /// <param name="_frequency">�g���K�[��R�̐U���x��[0 ~ 1]</param>
    /// <param name="_keepEffect">�����Ă���Ԃ��U��������[true false]</param>
    public void SetRightTriggerEffect_KeepEffect(float _startPosition, float _force, float _frequency, bool _keepEffect)
    {
        output.RightTriggerEffect.InitializeExtendedEffect(_startPosition, _force, _force, _force, _frequency, _keepEffect);
    }

    /// <summary>
    /// �E�g���K�[�̒�R��ݒ肷��i��R�n�_�ݒ�j
    /// </summary>
    /// <param name="_startPosition">��R�̊J�n�n�_[0 ~ 1]</param>
    /// <param name="_endPosition">��R�̏I���n�_[0 ~ 1]</param>
    /// <param name="_force">��R�̑傫��[0 ~ 1]</param>
    public void SetRightTriggerEffect_Position(float _startPosition, float _endPosition,float _force)
    {
        output.RightTriggerEffect.InitializeSectionResistanceEffect(_startPosition, _endPosition, _force);
    }

    /// <summary>
    /// ���g���K�[�̒�R��ݒ肷��i�A�Ŋ��G�j
    /// </summary>
    /// <param name="_startPosition">��R�̊J�n�n�_[0 ~ 1]</param>
    /// <param name="_force">��R�̑傫��[0 ~ 1]</param>
    /// <param name="_frequency">�g���K�[��R�̐U���x��[0 ~ 1]</param>
    /// <param name="_keepEffect">�����Ă���Ԃ��U��������[true false]</param>
    public void SetLeftTriggerEffect_KeepEffect(float _startPosition, float _force, float _frequency, bool _keepEffect)
    {
        output.LeftTriggerEffect.InitializeExtendedEffect(_startPosition, _force, _force, _force, _frequency, _keepEffect);
    }

    /// <summary>
    /// ���g���K�[�̒�R��ݒ肷��i��R�n�_�ݒ�j
    /// </summary>
    /// <param name="_startPosition">��R�̊J�n�n�_[0 ~ 1]</param>
    /// <param name="_endPosition">��R�̏I���n�_[0 ~ 1]</param>
    /// <param name="_force">��R�̑傫��[0 ~ 1]</param>
    public void SetLeftTriggerEffect_Position(float _startPosition, float _endPosition, float _force)
    {
        output.RightTriggerEffect.InitializeSectionResistanceEffect(_startPosition, _endPosition, _force);
    }

    /// <summary>
    /// �E�g���K�[�̒�R�𖳌��ɂ���
    /// </summary>
    public void SetRightTriggerNoEffect()
    {
        output.RightTriggerEffect.InitializeNoResistanceEffect();
    }

    /// <summary>
    /// ���g���K�[�̒�R�𖳌��ɂ���
    /// </summary>
    public void SetLeftTriggerNoEffect()
    {
        output.LeftTriggerEffect.InitializeNoResistanceEffect();
    }
}
