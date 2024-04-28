using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Parent : MonoBehaviour
{
    public enum EnemyType
    { 
        [InspectorName("���u")]Mob, 
        [InspectorName("�{�X")]Boss
    };

    [SerializeField, Header("�G�^�C�v"), Toolbar(typeof(EnemyType))]
    protected EnemyType enemyType;

    [SerializeField, Header("�ő�HP")]
    protected float maxHp;
    [SerializeField, Header("���݂�HP"), ReadOnly]
    protected float currentHp;
    [SerializeField, Header("�ő刳��")]
    protected float maxPressure;
    [SerializeField, Header("���݂̈���"), ReadOnly]
    protected float currentPressure;

    protected static GameObject target;


    protected void Start()
    {
        if(target == null)
        {
            target = GameObject.Find("Player");
            if(target != null)
            {
                Debug.Log("������");
            }
        }

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
            // �q�N���X�̔j��֐����Ăяo��
            DestroyFunc();
        }
    }

    /*
     * <summary>
     * ���S���֐�
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected abstract void DestroyFunc();
}
