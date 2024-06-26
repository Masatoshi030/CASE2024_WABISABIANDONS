using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //�v���C���[�̃V���O���g���C���X�^���X
    public static PlayerController instance;

    

    //=== �ړ� ===//
    [SerializeField, ReadOnly]
    Vector2 runInput;

    [SerializeField, Header("�ړ������x")]
    float runSpeed = 1.0f;

    [SerializeField, Header("�󒆈ړ����x����")]
    float runSpeed_AirAttenuation = 0.5f;

    [SerializeField, Header("���C�ő�ړ������x")]
    float runSpeed_Steam = 1.0f;

    [SerializeField, Header("��]��ԑ��x")]
    float rotationLerpSpeed = 1.0f;

    [SerializeField, Header("��]�����Ɍ������iShaft�j")]
    Transform horizontalRotationShaft;

    [SerializeField, Header("�ړ������Ɍ������iShaft�j")]
    Transform moveRotationShaft;

    [SerializeField, Header("�ړ������"), ReadOnly]
    Vector3 moveVelocity = Vector3.zero;

    [SerializeField, Header("�ړ����b�N")]
    bool bMoveLock = false;

    // �J�����̑O���x�N�g�����v���C���[�̐i�ޕ����Ƃ���
    Quaternion horizontalRotation;



    //=== �W�����v ===//
    [SerializeField, Header("�W�����v��")]
    float jumpPower = 5.0f;

    [SerializeField, Header("���������Z�W�����v��")]
    float jumpContinuationPower = 0.01f;

    [SerializeField, Header("�W�����v�����C�G�t�F�N�g")]
    GameObject jump_SteamEffect;

    [SerializeField, Header("�W�����v�p���ő厞��")]
    float jumpMaxTime = 1.0f;

    float jumpTime = 0.0f;

    enum JUMP_STATE
    { Idle, Rising, Descending };
    [SerializeField, Header("�W�����v�̏�ԁi�X�e�[�g�j"), Toolbar(typeof(JUMP_STATE), "JumpState")]
    JUMP_STATE jumpState = JUMP_STATE.Idle;

    // �v���C���[��Rigidbody
    Rigidbody myRigidbody;

    [SerializeField, Header("�ڒn����")]
    GroundJudgeController myGroundJudgeController;



    //=== ���C ===//
    [SerializeField, Header("���C������")]
    public float heldSteam = 100.0f;

    [SerializeField, Header("�ő���C������")]
    public float maxHeldSteam = 100.0f;

    [SerializeField, Header("�u�ԏo�͏��C��"), ReadOnly]
    float outSteamValue = 0.0f;

    [SerializeField, Header("�ő�u�ԏo�͏��C��")]
    float outMaxSteamValue = 0.5f;

    [SerializeField, Header("���R������")]
    float naturalAddPressure = 10.0f;

    [SerializeField, Header("�X�`�[�����@AudioSource")]
    AudioSource au_Steam;

    [SerializeField, Header("���C�ʂő傫����ς���@�\��L��")]
    bool bSteamScaleEnable = true;

    [SerializeField, Header("���C�̃p���p���x�����ő傫�����ς�鎲")]
    GameObject steamScaleShaft;

    [SerializeField, Header("�p���p���̎��̍ő�X�P�[��")]
    Vector3 steamMaxScale = new Vector3(1, 1, 1);



    //=== �R�K�X ===//
    [SerializeField, Header("�R�K�X�L��"), ReadOnly]
    bool bCombustibleGas = false;

    [SerializeField, Header("�����|�C���g")]
    GameObject explosionPoint;

    [SerializeField, Header("�R�K�X�̔����|�C���g�Ԋu����")]
    float explosionPoint_InstallationIntervalDistance = 0.3f;

    [SerializeField, Header("�O��̉R�K�X�̔����|�C���g�Ԋu���W"), ReadOnly]
    Vector3 explosionPoint_InstallationIntervalLastPosition = Vector3.zero;

    [SerializeField, Header("���C�����|�C���g")]
    GameObject steamInstantiatePoint;

    [SerializeField, Header("���o���C")]
    ParticleSystem compressor_SteamEffect;



    //=== �ˌ� ===//
    public enum ATTACK_STATE
    { Idle, Aim, Attack, KnockBack};
    [SerializeField, Header("�ˌ���ԁi�X�e�[�g�j"), Toolbar(typeof(ATTACK_STATE), "AttackState")]
    public ATTACK_STATE attackState = ATTACK_STATE.Idle;

    [SerializeField, Header("�ˌ�����")]
    float attackSpeed = 5.0f;

    [SerializeField, Header("�ˌ������"), ReadOnly]
    Vector3 attackVelocity = Vector3.zero;

    [SerializeField, Header("�ˌ��̎��̎p�����䎲")]
    GameObject attackShaft;

    [SerializeField, Header("�ːi������C��")]
    float attackUpCorrectionPower = 1.0f;

    [SerializeField, Header("�ˌ��Q�[�W")]
    AttackGaugeController attackGauge;

    [SerializeField, Header("�ˌ��Q�[�W�����܂鑬�x")]
    float attackGauge_AddSpeed = 2.0f;

    //�ˌ��^�[�Q�b�g���W
    Vector3 AttackTargetPosition;

    //�ˌ��Q�[�W�̗��߂��l
    float gaugeAttackValue = 0.0f;

    [SerializeField, Header("�ˌ��\�t���O")]
    public bool bAttackPossible = true;



    //=== ���S ===//
    [SerializeField, Header("�j�ЃG�t�F�N�g")]
    GameObject partsEffect;

    [SerializeField, Header("�����G�t�F�N�g")]
    GameObject explosionEffect;


    //=== �o���u���C�Ԃ���� ===//
    [SerializeField, Header("�o���u�Ԃ���ю���")]
    float valveFlyTime = 1.0f;

    [SerializeField, Header("�o���u�Ԃ���у^�C�}�[")]
    float valveFlyTimer = 0.0f;

    //�o���u�łԂ����ł����x�N�g��
    Vector3 valveFlyVector = Vector3.zero;

    public enum VALVE_FLY_STATE
    { Idle, Fly };
    [SerializeField, Header("�o���u�Ԃ���я�ԁi�X�e�[�g�j"), Toolbar(typeof(VALVE_FLY_STATE), "ValveFlyState")]
    public VALVE_FLY_STATE valveFlyState = VALVE_FLY_STATE.Idle;



    //=== GoldValve ===//
    [SerializeField, Header("�S�[���h�o���u�J�E���g")]
    int heldGoldValve = 0;

    [SerializeField, Header("�S�[���h�o���u�̌��ʉ�")]
    AudioClip goldValveSoundClip;

    [SerializeField, Header("�S�[���h�o���u�̌��ʉ��}�l�[�W���[")]
    SoundEffectManager goldValveSoundEffectManager;


    //=== BGM�ESE ===//
    BGMManager mainBGMManager;


    [SerializeField, Header("�L�����N�^�[�A�j���[�V����")]
    Animator characterAnimation;

    [SerializeField, Header("���C���v���C���[�J�����I�u�W�F�N�g"), ReadOnly]
    GameObject mainPlayerCamera_Obj;

    [SerializeField, Header("�m�b�N�o�b�N��")]
    float knockBackPower = 5.0f;

    [SerializeField, Header("���Ԋu�ň�񏈗����鏈���̊Ԋu")]
    float fixedIntervalTime = 1.0f;

    float fixedIntervalTimer = 0.0f;

    [SerializeField, Header("VolumesAnimation"), ReadOnly]
    Animator volumeAnimation;

    [SerializeField, Header("���샍�b�N")]
    bool bLock = false;

    [SerializeField, Header("�v���C���[�̃��C���[�}�X�N")]
    int playerLayerMask = 6;

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

        //�R�K�X�̐ݒ�
        bCombustibleGas = GameSetting.instance.bCombustibleGasEnable;

        //�������g��Rigidbody���擾
        myRigidbody = GetComponent<Rigidbody>();

        //���C���v���C���[�J�����I�u�W�F�N�g�̎Q��
        mainPlayerCamera_Obj = GameObject.Find("PlayerCamera_Brain");

        //VolumeAnimation�Q��
        volumeAnimation = GameObject.Find("Volumes").GetComponent<Animator>();

        //�ˌ��Q�[�W�̎Q��
        attackGauge = GameObject.Find("AttackGauge").GetComponent<AttackGaugeController>();
        attackGauge.gameObject.SetActive(false);

        //mainBGMManager���擾
        mainBGMManager = GameObject.Find("mainBGMManager").GetComponent<BGMManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //���b�N�����������瑁�����^�[��
        if (bLock)
        {
            return;
        }

        //=== �d�� ===//
        moveVelocity.y = myRigidbody.velocity.y;


        //=== �W�����v ===//

        OnJump();



        //=== ���C���� ===//
        if (outSteamValue > 0.0f)
        {
        }
        else
        {
            //���R����
            heldSteam += naturalAddPressure * Time.deltaTime;
        }

        //���S����
        if (heldSteam > maxHeldSteam)
        {
            //��������b�N
            bLock = true;

            //�v���C���[��Body���\����
            characterAnimation.gameObject.SetActive(false);

            //�j�ЃG�t�F�N�g����
            Instantiate(partsEffect, transform.position, Quaternion.identity);

            //�����G�t�F�N�g����
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

            //�t�F�[�h
            GameUIManager.instance.SetFade(true, 2.0f);

            //�U������
            DualSense_Manager.instance.SetLeftRumble(1.0f, 0.5f);

            //BGM���t�F�[�h�I��
            mainBGMManager.SetFadeStop(2.0f);

        }


        //=== ���C�o�͗� ===//

        //���C�o��
        if (heldSteam > 0.0f)
        {
            //�u�ԏo�͏��C��
            outSteamValue = (float)DualSense_Manager.instance.GetInputState().LeftTrigger.TriggerValue;

            //���o���C�̐U��
            DualSense_Manager.instance.SetRightRumble(outSteamValue, 0.05f);

            //�������͂��猸�炷
            heldSteam -= outSteamValue * outMaxSteamValue;

            //���C���ʒ���
            au_Steam.volume = outSteamValue;

            //���C�̏�ԂŃ|�X�g�G�t�F�N�g���u�����h
            volumeAnimation.SetFloat("fPressure", heldSteam / maxHeldSteam);

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
        if (bMoveLock == false)
        {
            myRigidbody.velocity = moveVelocity;
        }


        //=== �R�K�X ===//

        if (bCombustibleGas)
        {
            if (outSteamValue > 0.2f)
            {
                //�O��̐ݒu���W�ƍ��̍��W�Ƃ̍������ݒ�Ԋu�����������ǂ���
                float offsetDistance = Vector3.Distance(explosionPoint_InstallationIntervalLastPosition, transform.position);

                if (offsetDistance > explosionPoint_InstallationIntervalDistance)
                {
                    //�����|�C���g�𐶐�
                    Instantiate(explosionPoint, steamInstantiatePoint.transform.position, Quaternion.identity);

                    //���W���L�^
                    explosionPoint_InstallationIntervalLastPosition = transform.position;
                }
            }
        }


        //=== �ˌ� ===//

            OnAttack();


        //=== ���Ԋu���� ===//

        OnFixedInterval();
    }

    private void FixedUpdate()
    {

        //���b�N�����������瑁�����^�[��
        if (bLock)
        {
            return;
        }

        //=== �ړ� ===//

        //�ړ��ʓ���
        runInput = DualSense_Manager.instance.GetLeftStick();

        // �ړ��ʌv�Z�@�@�ړ����� �~ ���`��ԁi�ʏ�X�s�[�h�Ə��C�X�s�[�h�̊Ԃ����C�o�͗ʂŕ�ԁj
        Vector2 runValue = runInput * Mathf.Lerp(runSpeed, runSpeed_Steam, outSteamValue);

        //�󒆂ɂ���ƈړ����x����������
        if(myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.Off)
        {
            runValue *= runSpeed_AirAttenuation;
        }

        //�t���[������
        runValue *= Time.deltaTime;

        //�ړ�����
        moveVelocity = horizontalRotation * new Vector3(runValue.x, moveVelocity.y, runValue.y);

        //�n�ʂɂ���Ƃ��͈ړ��A�j���[�V����
        characterAnimation.SetFloat("fRun", runInput.magnitude + outSteamValue);

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


        //=== �o���u�Ԃ���� ===//
        OnValveJump();
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
            if (DualSense_Manager.instance.GetInputState().LeftTrigger.TriggerValue >= 1.0f)
            {
                //�W�����v�㏸��Ԃֈڍs
                jumpState = JUMP_STATE.Rising;

                //�W�����v�A�j���[�V�����J�n
                characterAnimation.SetTrigger("tJump");

                //����������
                moveVelocity = new Vector3(moveVelocity.x, jumpPower, moveVelocity.z);

                //���C�G�t�F�N�g
                Instantiate(jump_SteamEffect, transform.position, Quaternion.identity);
            }
        }
        //�㏸��
        else if (jumpState == JUMP_STATE.Rising)
        {
            //�㏸���Ɂ~�{�^���������Ă�����
            if (DualSense_Manager.instance.GetInputState().LeftTrigger.TriggerValue > 0.0f)
            {
                jumpTime += Time.deltaTime;

                //�ǉ����x������
                moveVelocity = new Vector3(
                    moveVelocity.x,
                    moveVelocity.y + jumpContinuationPower * (jumpMaxTime - jumpTime / jumpMaxTime) * Time.deltaTime,
                    moveVelocity.z);
            }

            //�W�����v�ő厞�Ԃ��߂��邩�~�{�^���������̂���߂���~���Ɉڍs
            if (jumpTime > jumpMaxTime || DualSense_Manager.instance.GetInputState().LeftTrigger.TriggerValue == 0.0f)
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log("player");
        }
        if(other.tag == "GoldValve")
        {
            //�擾�t���O
            other.GetComponent<GoldValveController>().GetGoldValve();
        }

        if(other.tag == "DamageSteam")
        {
            //�_���[�W
            heldSteam += 10.0f;

            //�m�b�N�o�b�N
            KnockBack();
        }
    }

    public void GetGoldValve()
    {
        //�A�C�e���J�E���g�𑝂₷
        heldGoldValve++;

        //�擾���Đ�
        goldValveSoundEffectManager.PlaySoundEffect(goldValveSoundClip);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.root.tag == "MoveGround")
        {
            transform.parent = collision.transform.parent.parent.GetComponent<MoveGrouond>().startPoint.transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.root.tag == "MoveGround")
        {
            transform.parent = null;
            transform.rotation = Quaternion.identity;
        }
    }

    public void SetValveJump(Vector3 _forceVec)
    {
        //���x�[��
        myRigidbody.velocity = Vector3.zero;

        //�ˌ��I��
        StopAttack();

        valveFlyState = VALVE_FLY_STATE.Fly;

        valveFlyVector = _forceVec;

        Debug.Log("�΂�ԁI");
    }

    void OnValveJump()
    {
        if (valveFlyState == VALVE_FLY_STATE.Fly)
        {
            valveFlyTimer += Time.deltaTime;

            transform.position += valveFlyVector * Time.deltaTime;

            if(myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.On)
            {
                valveFlyState = VALVE_FLY_STATE.Idle;
                valveFlyTimer = 0.0f;

                myRigidbody.velocity = Vector3.zero;
            }
        }
    }

    public void Damage(float _damage)
    {
        characterAnimation.SetTrigger("tDamage");

        //�ˌ������̔��΃x�N�g���̎΂ߏ�Ƀm�b�N�o�b�N����
        myRigidbody.velocity = (Vector3.up - moveRotationShaft.transform.forward) * knockBackPower;

        heldSteam += _damage;
    }

    void OnAttack()
    {

        //�ˌ���
        if (attackState == ATTACK_STATE.Attack)
        {

            //�ˌ����x�@�v�Z�����^�[�Q�b�g�ւ̃x�N�g���𐳋K�����A���x����Z����
            transform.position += (AttackTargetPosition - transform.position).normalized * (attackSpeed * gaugeAttackValue) * Time.deltaTime;

            //������ł����ق��Ɍ�����
            attackShaft.transform.LookAt(AttackTargetPosition);      //�O���x�N�g����������
            attackShaft.transform.Rotate(90.0f, 0.0f, 0.0f);
        }

        //�n�ʂɒ��n����ƏI��
        if (myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.On)
        {
            if (attackState == ATTACK_STATE.Attack || attackState == ATTACK_STATE.KnockBack)
            {
                StopAttack();
            }

            //�ˌ��\�t���O��L���ɂ���
            bAttackPossible = true;
        }

        //�_���Ă��鎞
        if (attackState == ATTACK_STATE.Aim)
        {
            attackGauge.AddValue(Time.deltaTime * attackGauge_AddSpeed);
        }

        //�ˌ����\�ł����
        if (bAttackPossible)
        {
            //�󒆂ɂ����Ԃ�
            if (myGroundJudgeController.onGroundState == GroundJudgeController.ON_GROUND_STATE.Off)
            {
                //�E�g���K�[��������
                if (DualSense_Manager.instance.GetInputState().RightTrigger.ActiveState == DualSenseUnity.ButtonState.NewDown)
                {
                    //�U����Ԃֈڍs
                    attackState = ATTACK_STATE.Aim;

                    //�ˌ��A�j���[�V�����J�n
                    characterAnimation.SetBool("bAttack", true);

                    //�ˌ��|�X�g�G�t�F�N�g�L��
                    volumeAnimation.SetBool("bAttack", true);

                    //�ˌ��Q�[�W���o��
                    attackGauge.gameObject.SetActive(true);

                    //�S�Ă̕����̗͂��O�ɂ���
                    myRigidbody.velocity = Vector3.zero;

                    //�d�͂𖳌��ɂ���
                    myRigidbody.useGravity = false;

                    //���x�����Z�b�g
                    PlayerVirtualCameraController.instance.OnAim(100, 100);

                    //�ړ����b�N
                    bMoveLock = true;

                    //�X���[
                    //Time.timeScale = 0.1f;
                }

                //�E�g���K�[�𗣂�
                if (DualSense_Manager.instance.GetInputState().RightTrigger.ActiveState == DualSenseUnity.ButtonState.NewUp)
                {
                    if (attackState == ATTACK_STATE.Aim)
                    {
                        //�U����Ԃֈڍs
                        attackState = ATTACK_STATE.Attack;

                        //�ˌ��Q�[�W���痭�߂����ʂ��擾
                        gaugeAttackValue = attackGauge.GetValue_Normalize();

                        //�ˌ��Q�[�W���B��
                        attackGauge.SetValue(0.0f);
                        attackGauge.gameObject.SetActive(false);

                        //���x�����Z�b�g
                        PlayerVirtualCameraController.instance.OnAimReset();

                        //�ˌ��\�t���O�𖳌��ɂ���
                        bAttackPossible = false;

                        //�X�N���[���̒��S�i�J�[�\�������킹���I�u�W�F�N�g�j�Ɍ������x�N�g�����v�Z����

                        //�J�����̑O���x�N�g���������l�Ƃ���@�������^�[�Q�b�g�����o����Ȃ��Ă��O���ɔ�ׂ�悤�ɂ���B
                        AttackTargetPosition = mainPlayerCamera_Obj.transform.forward;

                        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                        RaycastHit hit;

                        int LayerMask = ~(1 << playerLayerMask);

                        // Ray�������ɓ����������ǂ������m�F
                        if (Physics.Raycast(ray, out hit, 1000.0f, LayerMask))
                        {
                            AttackTargetPosition = hit.point;
                        }

                        //�X���[�I��
                        //Time.timeScale = 1.0f;
                    }
                }
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
        //�ˌ��|�X�g�G�t�F�N�g����
        volumeAnimation.SetBool("bAttack", false);

        //�d�͂𖳌��ɂ���
        myRigidbody.useGravity = true;

        //�ړ����b�N
        bMoveLock = false;
    }

    public void KnockBack()
    {
        //�m�b�N�o�b�N�A�j���[�V�����I��
        characterAnimation.SetTrigger("tHit");

        Debug.Log("Knock");

        //�ˌ������̔��΃x�N�g���̎΂ߏ�Ƀm�b�N�o�b�N����
        myRigidbody.velocity = Vector3.up * knockBackPower;

        //�m�b�N�o�b�N��Ԃɂ���
        attackState = ATTACK_STATE.KnockBack;
    }

    public void KnockBack(float power,  Vector3 direction)
    {
        //�m�b�N�o�b�N�A�j���[�V�����I��
        characterAnimation.SetTrigger("tHit");

        Debug.Log("Knock");

        //�ˌ������̔��΃x�N�g���̎΂ߏ�Ƀm�b�N�o�b�N����
        myRigidbody.velocity = direction * power;

        //�m�b�N�o�b�N��Ԃɂ���
        attackState = ATTACK_STATE.KnockBack;
    }

    void OnFixedInterval()
    {
        //�J�E���g
        fixedIntervalTimer += Time.deltaTime;
        //��莞�Ԃ��o��
        if(fixedIntervalTimer > fixedIntervalTime)
        {
            //=== ���� ===//

            //if (heldSteam > 0.0f)
            //{
            //    //�g���K�[��R��
            //    DualSense_Manager.instance.SetLeftTriggerContinuousResistanceEffect(0.0f, heldSteam / maxHeldSteam);
            //}
            //else
            //{
            //    //�g���K�[��R�𖳌��ɂ���
            //    DualSense_Manager.instance.SetLeftTriggerNoEffect();
            //}

            if (bSteamScaleEnable)
            {
                //���C�̃p���p���x�����Ńv���C���[�̃X�P�[����ς���
                steamScaleShaft.transform.localScale = Vector3.Lerp(new Vector3(1.0f, 1.0f, 1.0f), steamMaxScale, heldSteam / maxHeldSteam);
            }

            //���g���K�[�̒�R�ݒ�
            DualSense_Manager.instance.SetLeftTriggerEffect_Position(0.6f, 0.7f, 1.0f);

            //�^�C�}�[�����Z�b�g
            fixedIntervalTimer = 0.0f;
        }
    }

    public void OnGoal()
    {
        //�v���C���[�̃J�����𖳌��ɂ���
        mainPlayerCamera_Obj.GetComponent<Camera>().enabled = false;

        //�v���C���[�̓������~�߂�
        bLock = true;

        //�`���[�g���A���̃K�C�h���폜
        GameObject.Find("TutorialCanvas").SetActive(false);
    }
}