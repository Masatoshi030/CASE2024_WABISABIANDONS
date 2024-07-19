using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTarget : MonoBehaviour
{
    [SerializeField, Header("対象のタグ")]
    string targetTag = "Enemy";
    [SerializeField, Header("破壊対象のオブジェクト")]
    GameObject destroyTarget;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == targetTag)
        {
            Destroy(destroyTarget);
        }
    }

}
