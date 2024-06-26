using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowActiveManager : MonoBehaviour
{
    [SerializeField, Header("�G�l�~�[�R���|�[�l���g")]
    Enemy enemy;
    [SerializeField, Header("�Ǘ�����Allow")]
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
