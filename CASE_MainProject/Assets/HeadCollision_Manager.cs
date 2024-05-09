using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollision_Manager : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField, Header("音声クリップリスト")]
    AudioClip[] soundClips;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wall" || other.tag == "Ground")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
            {
                //突撃終了
                PlayerController.instance.StopAttack();

                //衝突音
                audioSource.PlayOneShot(soundClips[0]);
            }
        }
    }
}
