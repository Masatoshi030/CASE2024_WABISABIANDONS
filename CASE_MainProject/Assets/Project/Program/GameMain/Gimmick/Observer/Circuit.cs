using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �d�q��H
public class Circuit : MonoBehaviour
{
    [SerializeField, Header("�ŏI�I�ȉ��Z����"), ReadOnly]
    protected bool result = false;
    public virtual void Operate(Switch obj) { }
}
