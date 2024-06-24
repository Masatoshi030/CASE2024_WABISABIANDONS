using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveEmitter : MonoBehaviour
{
    [SerializeField, Header("�쐬����I�u�W�F�N�g")]
    GameObject instanceObject;

    public void Create(uint spawnNum, float velocitySpeed,float maxGetTime, bool isAuto)
    {
        // 30�x���݂Ő�������
        float originVelocitySpeed = velocitySpeed;
        float subAngle = 0.0f;
        float upVelocityPower = 7.5f;
        float sideVelocityPower = velocitySpeed;

        // 1.1�b�Ŏ��؂��悤�ɂ���(�v���C���[�܂ł̋����⊮�܂�(2.1�b))
        float duration = 0.2f;
        float timerAdd = maxGetTime / spawnNum;
        if(isAuto)
        {
            for (int i = 0; i < spawnNum; i++)
            {
                float angle = 30.0f * i + subAngle;
                transform.rotation = Quaternion.identity;
                transform.Rotate(Vector3.up, angle);
                GameObject obj = Instantiate(instanceObject, transform.position + (Vector3.up * 3.5f), Quaternion.identity);
                Vector3 force = transform.forward;
                force *= velocitySpeed;
                force.y += upVelocityPower;
                obj.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                AutoValveGet autoValve = obj.GetComponent<AutoValveGet>();
                autoValve.waitTime = duration;
                autoValve.isAuto = true;
                
                duration += timerAdd;
                if (i != 0 && i % 12 == 0)
                {
                    subAngle += 10.0f;
                    velocitySpeed += originVelocitySpeed;
                }
            }
        }
        else
        {
            for (int i = 0; i < spawnNum; i++)
            {
                float angle = 30.0f * i + subAngle;
                transform.rotation = Quaternion.identity;
                transform.Rotate(Vector3.up, angle);
                GameObject obj = Instantiate(instanceObject, transform.position + (Vector3.up * 3.5f), Quaternion.identity);
                Vector3 force = transform.forward;
                force.y += upVelocityPower;
                obj.GetComponent<Rigidbody>().AddForce(force * velocitySpeed, ForceMode.Impulse);
                duration += timerAdd;
                if (i != 0 && i % 12 == 0)
                {
                    subAngle += 10.0f;
                    velocitySpeed += originVelocitySpeed;
                }
            }
        }
    }
}
