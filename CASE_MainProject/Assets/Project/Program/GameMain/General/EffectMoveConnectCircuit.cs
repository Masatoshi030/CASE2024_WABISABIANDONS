using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �I�u�W�F�N�g�̒�����Ԉړ��ƏI������H�ɒʒm����
public class EffectMoveConnectCircuit : MonoBehaviour
{
    Vector3 initPos;
    Vector3 targetPos;

    float rate;
    float cnt = 0.0f;

    bool isUpdate = false;
    bool isFinished = false;
    public bool IsFinished { get => isFinished; }

    [SerializeField, Header("�A�g���H"), ReadOnly]
    List<Circuit_ActiveOperation> circuit_Actives;
    public List<Circuit_ActiveOperation> Circuits { set => circuit_Actives = value; }

    [SerializeField, Header("��ԏI������H�ɓn���l"), ReadOnly]
    bool present = true;
    public bool Present { set => present = value; }


    // Update is called once per frame
    void Update()
    {
        if(isUpdate)
        {
            cnt += Time.deltaTime * rate;
            if(cnt >= 1.0f)
            {
                cnt = 1.0f;
                isUpdate = false;
                isFinished = true;
                for(int i = 0; i < circuit_Actives.Count; i++)
                {
                    circuit_Actives[i].Operate(present);
                }
            }
            Vector3 pos = Vector3.Lerp(initPos, targetPos, cnt);
            transform.position = pos;
            if(cnt >= 1.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Excute(Vector3 init, Vector3 target, float time)
    {
        isFinished = false;
        isUpdate = true;
        rate = 1.0f / time;
        initPos = init;
        targetPos = target;
        cnt = 0.0f;
    }
}
