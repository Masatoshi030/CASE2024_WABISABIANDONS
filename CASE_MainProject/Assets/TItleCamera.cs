using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TItleCamera : MonoBehaviour
{
    [SerializeField, Header("���_�ړ��̊��x")]
    float aimSpeed = 0.1f;

    [SerializeField, Header("�߂鎞��")]
    float backSpeed = 1.0f;

    [SerializeField, Header("�߂�܂Ń^�C�}�["), ReadOnly]
    float backTimer = 0.0f;

    [SerializeField, Header("�߂��Ԃ̐ݒ�")]
    public AnimationCurve backLinear = AnimationCurve.Linear(
    timeStart: 0f,
    valueStart: 0f,
    timeEnd: 1f,
    valueEnd: 1f
    );

    Quaternion StartAngle;

    Quaternion lastFrameAngle;

    // Start is called before the first frame update
    void Start()
    {
        StartAngle = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 aimValue =  DualSense_Manager.instance.GetRightStick() * aimSpeed * Time.deltaTime;

        if (aimValue.magnitude > 0.1f)
        {
            transform.Rotate(-aimValue.y, aimValue.x, 0.0f);

            lastFrameAngle = transform.rotation;

            backTimer = backSpeed;
        }
        else
        {
            backTimer -= Time.deltaTime;

            if(backTimer <= 0.0f)
            {
                backTimer = 0.0f;
            }

            transform.rotation = Quaternion.Slerp(StartAngle, lastFrameAngle, backLinear.Evaluate(backTimer / backSpeed));
        }

        this.GetComponent<SceneChanger>().SceneChange("Select");
    }
}
