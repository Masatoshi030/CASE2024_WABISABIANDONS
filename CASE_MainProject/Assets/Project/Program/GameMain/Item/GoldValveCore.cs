using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldValveCore : MonoBehaviour
{
    [SerializeField, Header("自動取得するバルブ")]
    GoldValveController[] valves;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            foreach(GoldValveController valve in valves)
            {
                valve.GetGoldValve();
            }
        }
    }

    public void StartCoreFunc()
    {
        foreach (GoldValveController valve in valves)
        {
            valve.GetGoldValve();
        }
    }
}
