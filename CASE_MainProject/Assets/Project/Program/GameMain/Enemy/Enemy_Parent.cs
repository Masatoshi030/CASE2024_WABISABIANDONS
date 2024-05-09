using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Parent : MonoBehaviour
{
    protected const float padA = 20.0f;
    protected const float padB = 10.0f;
    public enum EnemyType
    { 
        [InspectorName("���u")]Mob, 
        [InspectorName("�{�X")]Boss
    };

    public struct EnemySetting
    {
        public EnemyType type;
        public float hp;
        public float pressure;
    }

    [SerializeField, Header("�G�^�C�v"), Toolbar(typeof(EnemyType))]
    protected EnemyType enemyType;

    [Space(padA), Header("--��b�p�����[�^--")]
    [SerializeField, Header("�ő�HP")]
    protected float maxHp;
    [SerializeField, Header("���݂�HP"), ReadOnly]
    protected float currentHp;
    [SerializeField, Header("�ő刳��")]
    protected float maxPressure;
    [SerializeField, Header("���݂̈���"), ReadOnly]
    protected float currentPressure;

    protected static GameObject target;

    protected void Awake()
    {
        if (target == null)
        {
            target = GameObject.Find("Player");
            if (target != null)
            {
                Debug.Log("Player�̊i�[����");
            }
        }
    }

    protected void Start()
    {
        currentHp = maxHp;
        currentPressure = maxPressure;
    }

    // Update is called once per frame
    protected void Update()
    {

    }

    /*
     * <summary>
     * �_���[�W�֐�
     * <param>
     * float : val ...�_���[�W��
     * <retrun>
     * �Ȃ�
     */
    public virtual void Damage(float val)
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
    protected virtual void DestroyFunc() { }
}
