using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �V���O���^�C�v
// ��̃X�C�b�`���瓾������K�p������
public class Circuit_ActiveOperation : Circuit
{
    [SerializeField, Header("���삷��I�u�W�F�N�g")]
    GameObject target;
    public GameObject Target { get => target; }
    [SerializeField, Header("���쎞�G�t�F�N�g")]
    GameObject operateParticle;
    [SerializeField, Header("�G�t�F�N�g�����ʒu")]
    Transform effectPos;

    [SerializeField, Header("���쎞SE"), ReadOnly]
    AudioSource audioSource;

    private void Start()
    {
        if(target.GetComponent<AudioSource>())
        {
            audioSource = target.GetComponent<AudioSource>();
        }
    }

    public override void Operate(Switch obj)
    {
        Debug.Log("Operate");
        result = obj.IsActive;
        target.SetActive(obj.IsActive);
        if(result &&  operateParticle != null)
        {
            Instantiate(operateParticle, effectPos.position, Quaternion.identity);
            if(audioSource != null)
            {
                audioSource.Play();
            }
        }
    }

    public void Operate(bool value)
    {
        Debug.Log("Operate");
        result = value;
        target.SetActive(value);
        if (result && operateParticle != null)
        {
            Instantiate(operateParticle, effectPos.position, Quaternion.identity);
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
