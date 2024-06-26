using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowActiveManager : MonoBehaviour
{
    [SerializeField, Header("エネミーコンポーネント")]
    Enemy enemy;
    [SerializeField, Header("管理するAllow")]
    GameObject managedAllow;

    private void Update()
    {
        if(enemy.IsFindPlayer)
        {
            managedAllow.SetActive(true);
        }
        else
        {
            managedAllow.SetActive(false);
        }
    }
}
