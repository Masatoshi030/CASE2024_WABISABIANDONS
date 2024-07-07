using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldValveCore : MonoBehaviour
{
    [SerializeField, Header("Ž©“®Žæ“¾‚·‚éƒoƒ‹ƒu")]
    AutoValveGet[] valves;

    public bool useHitStop = false;

    [SerializeField, Header("SE‚Ì—LŒø")]
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
            PlayerController.instance.OnHitStop(0.5f, 0.1f);
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
