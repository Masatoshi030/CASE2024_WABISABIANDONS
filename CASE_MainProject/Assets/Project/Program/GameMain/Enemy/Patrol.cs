using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField, Header("�ړ��J�n�ʒu"), ReadOnly]
    Vector3 startPosition;

    [SerializeField, Header("�ړ��J�n�ʒu�ƖړI�n�Ƃ̋���"), ReadOnly]
    float distance;

    [SerializeField, Header("�ړI�n�̐e")]
    Transform targetParent;

    [SerializeField, Header("�ړI�n")]
    Transform[] patrols;

    [SerializeField, Header("�����鎞��"), ReadOnly]
    float time = 0.0f;
    [SerializeField, Header("�o�ߎ���"), ReadOnly]
    float cnt = 0.0f;

    private void Start()
    {
        if(targetParent)
        {
            patrols = new Transform[targetParent.childCount];
            for(int i = 0; i < targetParent.childCount; i++)
            {
                Transform trs = targetParent.GetChild(i);
                patrols[i] = trs;
            }
        }
    }

    public bool ExcutePatrol(int index, float moveSpeed, bool isLook = false)
    {
        cnt += Time.deltaTime;

        float t = cnt / time;
        if(t >= 1.0f)
        {
            t = 1.0f;
        }
        Vector3 pos = Vector3.Lerp(startPosition, patrols[index].position, t);
        // �������ړ������Ɍ�����(����)
        if(isLook)
        {
            transform.LookAt(patrols[index].position);
        }

        transform.localPosition = pos;
        if(t == 1.0f)
        {
            return true;
        }
        return false;
    }

    public Transform[] GetTargets()
    {
        return patrols;
    }

    public void SetInitPosition(Vector3 position, float moveSpeed, int index)
    {
        cnt = 0.0f;
        startPosition = position;
        distance = Vector3.Distance(startPosition, patrols[index].position);
        time = distance / moveSpeed;
    }
}
