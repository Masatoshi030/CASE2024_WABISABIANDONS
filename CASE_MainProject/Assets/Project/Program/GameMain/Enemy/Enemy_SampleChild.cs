using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SampleChild : Enemy_Parent
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    // �L�����ɂ���Ĕj���̋������ς��Ǝv���̂�override�ł�낵���ł�
    public override void DestroyFunc()
    {
        Debug.Log("�j��֐�");
    }
}
