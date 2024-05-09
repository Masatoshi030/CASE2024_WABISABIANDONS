using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollision_Manager : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField, Header("�����N���b�v���X�g")]
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
                //�ˌ��I��
                PlayerController.instance.StopAttack();

                //�Փˉ�
                audioSource.PlayOneShot(soundClips[0]);
            }
        }
    }
}
