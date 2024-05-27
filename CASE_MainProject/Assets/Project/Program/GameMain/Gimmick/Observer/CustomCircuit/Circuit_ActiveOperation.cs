using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シングルタイプ
// 一つのスイッチから得た情報を適用させる
public class Circuit_ActiveOperation : Circuit
{
    [SerializeField, Header("操作するオブジェクト")]
    GameObject target;
    public GameObject Target { get => target; }
    [SerializeField, Header("操作時エフェクト")]
    GameObject operateParticle;
    [SerializeField, Header("エフェクト発生位置")]
    Transform effectPos;

    [SerializeField, Header("操作時SE"), ReadOnly]
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
