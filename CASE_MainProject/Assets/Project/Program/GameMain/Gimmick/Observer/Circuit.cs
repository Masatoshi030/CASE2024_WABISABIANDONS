using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 電子回路
public class Circuit : MonoBehaviour
{
    [SerializeField, Header("最終的な演算結果"), ReadOnly]
    protected bool result = false;
    public virtual void Operate(Switch obj) { }
}
