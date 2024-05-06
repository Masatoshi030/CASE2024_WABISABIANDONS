using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField, Header("移動開始位置"), ReadOnly]
    Vector3 startPosition;

    [SerializeField, Header("移動開始位置と目的地との距離"), ReadOnly]
    float distance;

    [SerializeField, Header("目的地の親")]
    Transform targetParent;

    [SerializeField, Header("目的地")]
    Transform[] patrols;

    [SerializeField, Header("かかる時間"), ReadOnly]
    float time = 0.0f;
    [SerializeField, Header("経過時間"), ReadOnly]
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
        // 向きを移動方向に向ける(強制)
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
