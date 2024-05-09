using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollision_Manager : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField, Header("音声クリップリスト")]
    AudioClip[] soundClips;

    [SerializeField, Header("火花エフェクト")]
    GameObject hibana_ParticleEffect;

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
        if (other.tag == "Wall" || other.tag == "Ground" || other.tag == "Enemy")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
            {
                //突撃終了
                PlayerController.instance.StopAttack();

                //衝突音
                audioSource.PlayOneShot(soundClips[1]);

                //火花を生成する
                Instantiate(hibana_ParticleEffect, transform.position, Quaternion.identity);

                //敵に突撃した時の処理
                if (other.tag == "Enemy")
                {
                    //衝突金属音
                    audioSource.PlayOneShot(soundClips[0]);

                    //ダメージを与える
                    other.GetComponent<Enemy_Mob>().Damage(20.0f, transform.up);

                    //ノックバック
                    PlayerController.instance.KnockBack();
                }
            }
        }
    }
}