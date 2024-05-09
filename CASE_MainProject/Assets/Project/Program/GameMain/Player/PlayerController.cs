using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //�v���C���[�̃V���O���g���C���X�^���X
    public static PlayerController instance;

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

    [SerializeField, Header("���C�ő�W�����v��")]
    float jumpPower_Steam = 8.0f;

    [SerializeField, Header("���C�ő咷�������Z�W�����v��")]
    float jumpContinuationPower_Steam = 0.02f;

    float jumpTime = 0.0f;

    enum JUMP_STATE
    { Idle, Rising, Descending };
    [SerializeField, Header("�W�����v�̏�ԁi�X�e�[�g�j"), Toolbar(typeof(JUMP_STATE), "JumpState")]
    JUMP_STATE jumpState = JUMP_STATE.Idle;

    [SerializeField, Header("�ڒn����")]
    GroundJudgeController myGroundJudgeController;

    [SerializeField, Header("���C������")]
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

    public enum ATTACK_STATE
    { Idle, Attack};
    [SerializeField, Header("�ˌ���ԁi�X�e�[�g�j"), Toolbar(typeof(ATTACK_STATE), "AttackState")]
    public ATTACK_STATE attackState = ATTACK_STATE.Idle;

    [SerializeField, Header("�ˌ�����")]
    float attackSpeed = 5.0f;

    [SerializeField, Header("�L�����N�^�[�A�j���[�V����")]
    Animator characterAnimation;

    [SerializeField, Header("���C���v���C���[�J�����I�u�W�F�N�g"), ReadOnly]
    GameObject mainPlayerCamera_Obj;

    [SerializeField, Header("�ړ������"), ReadOnly]
    Vector3 moveVelocity = Vector3.zero;

    [SerializeField, Header("�ˌ������"), ReadOnly]
    Vector3 attackVelocity = Vector3.zero;

    [SerializeField, Header("�ˌ��̎��̎p�����䎲")]
    GameObject attackShaft;

    [SerializeField, Header("���o���C")]
    ParticleSystem compressor_SteamEffect;

    [SerializeField, Header("�m�b�N�o�b�N��")]
    float knockBackPower = 5.0f;

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

        //�������g��Rigidbody���擾
        myRigidbody = GetComponent<Rigidbody>();

        //���C���v���C���[�J�����I�u�W�F�N�g�̎Q��
        mainPlayerCamera_Obj = GameObject.Find("PlayerCamera_Brain");
    }

    // Update is called once per frame
    void Update()
    {
        //=== �d�� ===//
        moveVelocity.y = myRigidbody.velocity.y;

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

                //���C�G�t�F�N�g
                var emission = compressor_SteamEffect.emission;
                emission.rateOverTime = 0.0f;
            }
            else
            {
                //���C�G�t�F�N�g
                var emission = compressor_SteamEffect.emission;
                emission.rateOverTime = 50.0f * outSteamValue;
            }
        }
        else
        {
            outSteamValue = 0.0f;
            au_Steam.volume = 0.0f;
        }

        //�v���C���[�ɗ͂�������
        myRigidbody.velocity = moveVelocity;

        //=== �ˌ� ===//

        OnAttack();
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
        moveVelocity = horizontalRotation * new Vector3(runValue.x, moveVelocity.y, runValue.y);

        //�n�ʂɂ���Ƃ��͈ړ��A�j���[�V����
        characterAnimation.SetFloat("fRun", runInput.magnitude);

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

                //�W�����v�A�j���[�V�����J�n
                characterAnimation.SetTrigger("tJump");

                //����������
                moveVelocity = new Vector3(moveVelocity.x, Mathf.Lerp(jumpPower, jumpPower_Steam, outSteamValue), moveVelocity.z);
            }
        }
        //�㏸��
        else if (jumpState == JUMP_STATE.Rising)
        {
            //�㏸���Ɂ~�{�^���������Ă�����
            if (DualSense_Manager.instance.GetInputState().CrossButton == DualSenseUnity.ButtonState.Down)
            {
                jumpTime += Time.deltaTime;

                //�ǉ����x������
                moveVelocity = new Vector3(
                    moveVelocity.x,
                    moveVelocity.y + Mathf.Lerp(jumpContinuationPower, jumpContinuationPower_Steam, outSteamValue) * (jumpMaxTime - jumpTime / jumpMaxTime) * Time.deltaTime,
                    moveVelocity.z);
            }

            //�W�����v�ő厞�Ԃ��߂��邩�~�{�^���������̂���߂���ƍ~���Ɉڍs
            if (jumpTime > jumpMaxTime || DualSense_Manager.instance.GetInputState().CrossButton != DualSenseUnity.ButtonState.Down)
            {
                jumpState = JUMP_STATE.Descending;
            }
        }
        //�~����
        else if (jumpState == JUMP_STATE.Descending)
        {
            //�ڒn���肪�L���ɂȂ�����W�����v�I��
            if (myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.On)
            {
                jumpState = JUMP_STATE.Idle;

                jumpTime = 0.0f;
            }
        }

        //�ڒn����
        if (myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.Off)
        {
            characterAnimation.SetBool("bOnGround", false);
        }
        else
        {
            //�W�����v�A�j���[�V�����I��
            characterAnimation.SetBool("bOnGround", true);
        }
    }

    void OnAttack()
    {

        //�ˌ���
        if (attackState == ATTACK_STATE.Attack)
        {

            //�ˌ����x
            transform.position += mainPlayerCamera_Obj.transform.forward * attackSpeed * Time.deltaTime;

            //������ł����ق��Ɍ�����
            attackShaft.transform.LookAt(attackShaft.transform.position + mainPlayerCamera_Obj.transform.forward);      //�O���x�N�g����������
            attackShaft.transform.Rotate(90.0f, 0.0f, 0.0f);

            //�n�ʂɒ��n����ƏI��
            if (myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.On)
            {
                StopAttack();
            }
        }

        //�󒆂ɂ����Ԃ�
        if (myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.Off)
        {
            //�W�����v�{�^����������
            if(DualSense_Manager.instance.GetInputState().CrossButton == DualSenseUnity.ButtonState.NewDown)
            {
                //�U����Ԃֈڍs
                attackState = ATTACK_STATE.Attack;

                //�ˌ��A�j���[�V�����J�n
                characterAnimation.SetBool("bAttack", true);
            }
        }
    }

    public void StopAttack()
    {
        attackState = ATTACK_STATE.Idle;
        //�ˌ��A�j���[�V�����I��
        characterAnimation.SetBool("bAttack", false);
        //�p����߂�
        attackShaft.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    public void KnockBack()
    {
        //�m�b�N�o�b�N�A�j���[�V�����I��
        characterAnimation.SetTrigger("tHit");

        //�ˌ������̔��΃x�N�g���̎΂ߏ�Ƀm�b�N�o�b�N����
        myRigidbody.velocity = Vector3.up * knockBackPower;
    }
}