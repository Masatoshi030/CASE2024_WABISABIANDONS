using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField, Header("�ړ����x")]
    Vector2 runSpeed = new Vector2(1.0f, 1.0f);

    [SerializeField, Header("�W�����v��")]
    float jumpPower = 5.0f;

    [SerializeField, Header("���C������"), ReadOnly]
    float heldSteam = 100.0f;

    [SerializeField, Header("�o�͏��C��"), ReadOnly]
    float outSteam = 0.0f;

    Rigidbody myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //�ړ��ʓ���
        Vector2 runValue = DualSense_Manager.instance.GetLeftStick() * runSpeed;

        //�t���[������
        runValue *= Time.deltaTime;

        //���W�ύX
        this.transform.Translate(runValue.x, 0.0f, runValue.y);

        //�W�����v
        if(DualSense_Manager.instance.GetInputState().CrossButton == DualSenseUnity.ButtonState.Down)
        {
            myRigidbody.velocity = transform.up * jumpPower;
        }

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
}
