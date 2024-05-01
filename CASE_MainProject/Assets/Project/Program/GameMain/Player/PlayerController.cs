using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField, Header("�ړ������x")]
    Vector2 runSpeed = new Vector2(1.0f, 1.0f);

    [SerializeField, Header("��]��ԑ��x")]
    float rotationLerpSpeed = 1.0f;

    [SerializeField, Header("�W�����v��")]
    float jumpPower = 5.0f;

    [SerializeField, Header("�ړ������Ɍ������iShaft�j")]
    Transform horizontalRotationShaft;

    [SerializeField, Header("���C������"), ReadOnly]
    float heldSteam = 100.0f;

    [SerializeField, Header("�o�͏��C��"), ReadOnly]
    float outSteam = 0.0f;

    // �v���C���[��Rigidbody
    Rigidbody myRigidbody;

    // �J�����̑O���x�N�g�����v���C���[�̐i�ޕ����Ƃ���
    Quaternion horizontalRotation;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //=== ��] ===//

        //�J�����̑O���x�N�g��
        horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);


        //=== �W�����v ===//

        //�W�����v
        if (DualSense_Manager.instance.GetInputState().CrossButton == DualSenseUnity.ButtonState.Down)
        {
            myRigidbody.velocity = transform.up * jumpPower;
        }

        //=== ���C�o�͗� ===//

        //���C�o��
        if (heldSteam > 0.0f)
        {
            heldSteam -= (float)DualSense_Manager.instance.GetInputState().RightTrigger.TriggerValue;

            if (heldSteam < 0.0f)
            {
                heldSteam = 0.0f;
            }
        }
    }

    private void FixedUpdate()
    {

        //=== �ړ� ===//

        //�ړ��ʓ���
        Vector2 runValue = DualSense_Manager.instance.GetLeftStick();

        //�ړ������x
        runValue *= runSpeed;

        //�t���[������
        runValue *= Time.deltaTime;

        //�ړ�����
        myRigidbody.velocity = horizontalRotation * new Vector3(runValue.x, 0.0f, runValue.y);

        //�V���t�g�̌�����ύX���ăv���C���[���ړ������Ɍ�������
        if(runValue.magnitude > 0.1f)
        {
            horizontalRotationShaft.localRotation = Quaternion.Lerp(
                horizontalRotationShaft.localRotation,
                horizontalRotation,
                rotationLerpSpeed * Time.deltaTime);
        }
    }
}
