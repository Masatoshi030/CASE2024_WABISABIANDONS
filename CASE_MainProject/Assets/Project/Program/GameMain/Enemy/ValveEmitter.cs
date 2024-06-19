using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveEmitter : MonoBehaviour
{
    [SerializeField, Header("作成するオブジェクト")]
    GameObject instanceObject;

    public void Create(uint spawnNum, float velocitySpeed, bool isAuto)
    {
        // 30度刻みで生成する
        float originVelocitySpeed = velocitySpeed;
        float subAngle = 0.0f;
        float upVelocityPower = 2.0f;
        float sideVelocityPower = velocitySpeed;

        // 2秒で取り切れるようにする
        float duration = 1.0f;
        float timerAdd = 1.0f / spawnNum;
        if(isAuto)
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
