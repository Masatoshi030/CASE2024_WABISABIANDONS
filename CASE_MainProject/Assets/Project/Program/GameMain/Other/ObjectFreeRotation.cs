using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFreeRotation : MonoBehaviour
{
    [SerializeField]
    private int ObjectSpeed;

    [SerializeField]
    private int RotationX;

    [SerializeField]
    private int RotationY;

    [SerializeField]
    private int RotationZ;


    // Update is called once per frame
    void Update()
    {
        // transform���擾
        Transform myTransform = this.transform;

        // ���[�J�����W��ŁA���݂̉�]�ʂ։��Z����
        myTransform.Rotate(RotationX * ObjectSpeed, RotationY * ObjectSpeed, RotationZ * ObjectSpeed);
    }
}
