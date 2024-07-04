using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldValveCore : MonoBehaviour
{
    [SerializeField, Header("自動取得するバルブ")]
    AutoValveGet[] valves;

    [SerializeField, Header("ヒットストップの有効")]
    public bool useHitStop = false;

    [SerializeField, Header("ヒットストップの有効時間")]
    float hitStopTime = 0.2f;
    [SerializeField, Header("ヒットストップ時のタイムスケール")]
    float hitStopTimeScale = 0.1f;

    [SerializeField, Header("SEの有効")]
    bool useSE = true;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            StartCoreFunc();
        }
    }

    public void StartCoreFunc()
    {
        float time = 0.1f;
        if(useHitStop)
        {
            PlayerController.instance.OnHitStop(hitStopTime, hitStopTimeScale);
        }
        if(useSE)
        {
            AudioSource source = GetComponent<AudioSource>();
            source.PlayOneShot(source.clip);
        }
        

        foreach (AutoValveGet valve in valves)
        {
            valve.cnt = 0.0f;
            valve.isAuto = true;
            valve.waitTime = time;
            
            time += 0.05f;
        }
    }
}
