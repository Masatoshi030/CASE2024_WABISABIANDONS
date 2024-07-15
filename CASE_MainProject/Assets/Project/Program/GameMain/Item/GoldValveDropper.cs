using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldValveDropper : MonoBehaviour
{
    [SerializeField, Header("バルブのドロップ数")]
    int dropValveNum = 12;
    public int DropValveNum { get => dropValveNum; }

    [SerializeField, Header("自動取得するか")]
    bool isAutoGet = false;

    [SerializeField, Header("拡散スピード")]
    float diffusionSpeed =1.0f;
    public void DropValve()
    {
        DropValveManager.instance.CreateValves(dropValveNum, transform.position, isAutoGet, diffusionSpeed);
    }
}
