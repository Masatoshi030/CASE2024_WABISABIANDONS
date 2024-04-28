using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Parent : MonoBehaviour
{
    [SerializeField, Header("�ő�HP")]
    protected float maxHp;
    [SerializeField, Header("���݂�HP"), ReadOnly]
    protected float currentHp;
    [SerializeField, Header("�ő刳��")]
    protected float maxPressure;
    [SerializeField, Header("���݂̈���"), ReadOnly]
    protected float currentPressure;


    protected void Start()
    {
        currentHp = maxHp;
        currentPressure = maxPressure;
        Debug.Log("�J�n");
    }

    // Update is called once per frame
    protected void Update()
    {
        Debug.Log("�X�V");
    }

    /*
     * <summary>
     * �_���[�W�֐�
     * <param>
     * float : val ...�_���[�W��
     * <retrun>
     * �Ȃ�
     */
    public void Damage(float val)
    {
        currentHp -= val;
        DestroyCheck();
    }

    /*
     * <summary>
     * ���S����֐�
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected void DestroyCheck()
    {
        if(currentHp <= 0.0f)
        {
            // ���S�A�j���[�V�����Đ�
            // ���S��j��
            Destroy(gameObject);
        }
    }
}
