using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFreeRotation : MonoBehaviour
{
    [SerializeField]
    private float ObjectSpeed;

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

        Vector3 rotation = new Vector3(RotationX, RotationY, RotationZ) * ObjectSpeed * Time.deltaTime;

        // ���[�J�����W��ŁA���݂̉�]�ʂ։��Z����
        myTransform.Rotate(rotation);
    }
}
