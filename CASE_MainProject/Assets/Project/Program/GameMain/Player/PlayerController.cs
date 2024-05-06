using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("�ړ������x")]
    float runSpeed = 1.0f;

    [SerializeField, Header("���C�ő�ړ������x")]
    float runSpeed_Steam = 1.0f;

    [SerializeField, Header("��]��ԑ��x")]
    float rotationLerpSpeed = 1.0f;

    [SerializeField, Header("��]�����Ɍ������iShaft�j")]
    Transform horizontalRotationShaft;

    [SerializeField, Header("�ړ������Ɍ������iShaft�j")]
    Transform moveRotationShaft;

    [SerializeField, Header("�W�����v��")]
    float jumpPower = 5.0f;

    [SerializeField, Header("���������Z�W�����v��")]
    float jumpContinuationPower = 0.01f;

    [SerializeField, Header("�W�����v�p���ő厞��")]
    float jumpMaxTime = 1.0f;

    float jumpTime = 0.0f;

    enum JUMP_STATE
    { Idle, Rising, Descending };
    [SerializeField, Header("�W�����v�̏�ԁi�X�e�[�g�j"), Toolbar(typeof(JUMP_STATE), "JumpState")]
    JUMP_STATE jumpState = JUMP_STATE.Idle;

    [SerializeField, Header("�ڒn����")]
    GroundJudgeController myGroundJudgeController;

    [SerializeField, Header("���C������"), ReadOnly]
    float heldSteam = 100.0f;

    [SerializeField, Header("�u�ԏo�͏��C��"), ReadOnly]
    float outSteamValue = 0.0f;

    [SerializeField, Header("�ő�u�ԏo�͏��C��")]
    float outMaxSteamValue = 0.5f;

    [SerializeField, Header("�X�`�[�����@AudioSource")]
    AudioSource au_Steam;


    // �v���C���[��Rigidbody
    Rigidbody myRigidbody;

    // �J�����̑O���x�N�g�����v���C���[�̐i�ޕ����Ƃ���
    Quaternion horizontalRotation;

    [SerializeField, ReadOnly]
    Vector2 runInput;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        //=== �W�����v ===//

        OnJump();

        //=== ���C�o�͗� ===//

        //���C�o��
        if (heldSteam > 0.0f)
        {
            //�u�ԏo�͏��C��
            outSteamValue = (float)DualSense_Manager.instance.GetInputState().RightTrigger.TriggerValue;

            //�������͂��猸�炷
            heldSteam -= outSteamValue * outMaxSteamValue;

            //���C���ʒ���
            au_Steam.volume = outSteamValue;

            if (heldSteam < 0.0f)
            {
                heldSteam = 0.0f;
            }
        }
        else
        {
            outSteamValue = 0.0f;
            au_Steam.volume = 0.0f;
        }
    }

    private void FixedUpdate()
    {
        //=== �ړ� ===//

        //�ړ��ʓ���
        runInput = DualSense_Manager.instance.GetLeftStick();

        // �ړ��ʌv�Z�@�@�ړ����� �~ ���`��ԁi�ʏ�X�s�[�h�Ə��C�X�s�[�h�̊Ԃ����C�o�͗ʂŕ�ԁj
        Vector2 runValue = runInput * Mathf.Lerp(runSpeed, runSpeed_Steam, outSteamValue);

        //�t���[������
        runValue *= Time.deltaTime;

        //�ړ�����
        myRigidbody.velocity = horizontalRotation * new Vector3(runValue.x, myRigidbody.velocity.y, runValue.y);


        //=== ��] ===//

        //�J�����̑O���x�N�g��
        horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        //�V���t�g�̌�����ύX���ăv���C���[���ړ������Ɍ�������
        if (runValue.magnitude > 0.1f)
        {
            //�J�����̑O���x�N�g��������
            horizontalRotationShaft.localRotation = Quaternion.Lerp(
                horizontalRotationShaft.localRotation,
                horizontalRotation,
                rotationLerpSpeed * Time.deltaTime);

            //�ړ��̕����֌���
            moveRotationShaft.localRotation = Quaternion.Lerp(
                moveRotationShaft.localRotation,
                Quaternion.LookRotation(new Vector3(runInput.x, 0.0f, runInput.y)),
                rotationLerpSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// �W�����v����
    /// </summary>
    void OnJump()
    {
        //�W�����v�҂����
        if (jumpState == JUMP_STATE.Idle)
        {
            //�ŏ��̏����ƃW�����v�J�n����
            if (DualSense_Manager.instance.GetInputState().CrossButton == DualSenseUnity.ButtonState.NewDown)
            {
                //�W�����v�㏸��Ԃֈڍs
                jumpState = JUMP_STATE.Rising;
                //����������
                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpPower, myRigidbody.velocity.z);
            }
        }
        //�㏸��
        else if(jumpState == JUMP_STATE.Rising)
        {
            //�㏸���Ɂ~�{�^���������Ă�����
            if (DualSense_Manager.instance.GetInputState().CrossButton == DualSenseUnity.ButtonState.Down)
            {
                jumpTime += Time.deltaTime;

                //�ǉ����x������
                myRigidbody.velocity = new Vector3(
                    myRigidbody.velocity.x, 
                    myRigidbody.velocity.y + jumpContinuationPower * (jumpMaxTime - jumpTime / jumpMaxTime) * Time.deltaTime,
                    myRigidbody.velocity.z);
            }

            //�W�����v�ő厞�Ԃ��߂��邩�~�{�^���������̂���߂���ƍ~���Ɉڍs
            if(jumpTime > jumpMaxTime || DualSense_Manager.instance.GetInputState().CrossButton != DualSenseUnity.ButtonState.Down)
            {
                jumpState = JUMP_STATE.Descending;
            }
        }
        //�~����
        else if(jumpState == JUMP_STATE.Descending)
        {
            //�ڒn���肪�L���ɂȂ�����W�����v�I��
            if(myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.On)
            {
                jumpState = JUMP_STATE.Idle;
                jumpTime = 0.0f;
            }
        }
    }
}
