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

    [SerializeField, Header("パーツ散開エフェクト")]
    GameObject partsSplit_ParticleEffect;


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
        if (other.tag == "Wall" || other.tag == "Ground" || other.tag == "Enemy" || other.tag == "Dummy")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
            {
                //突撃終了
                PlayerController.instance.StopAttack();

                //衝突音
                audioSource.PlayOneShot(soundClips[1]);

                //火花を生成する
                Instantiate(hibana_ParticleEffect, transform.position, Quaternion.identity);

                //小振動
                DualSense_Manager.instance.SetLeftRumble(0.25f, 0.1f);

                //敵に突撃した時の処理
                if (other.tag == "Dummy")
                {
                    //衝突金属音
                    audioSource.PlayOneShot(soundClips[0]);

                    //ノックバック
                    PlayerController.instance.KnockBack();

                    //パーツ散開エフェクトを生成する
                    Instantiate(partsSplit_ParticleEffect, transform.position, Quaternion.identity);

                    //ヒットストップ
                    HitStopManager.instance.HitStopEffect(0.5f, 0.25f);

                    //小振動
                    DualSense_Manager.instance.SetRumble_Type1();

                    Destroy(other.gameObject);

                    //倒したら
                    //if (other.GetComponent<Enemy_Mob>().Damage(20.0f, transform.up))
                    //{
                    //    //パーツ散開エフェクトを生成する
                    //    Instantiate(partsSplit_ParticleEffect, transform.position, Quaternion.identity);
                    //
                    //    //ヒットストップ
                    //    HitStopManager.instance.HitStopEffect(0.5f, 0.25f);
                    //
                    //    //小振動
                    //    DualSense_Manager.instance.SetRumble_Type1();
                    //}
                    //倒してない
                    //else
                    //{
                    //    //ヒットストップ
                    //    HitStopManager.instance.HitStopEffect(0.2f, 0.25f);
                    //
                    //    //小振動
                    //    DualSense_Manager.instance.SetLeftRumble(0.75f, 0.1f);
                    //}
                }
            }
        }

        if(other.tag == "Goal")
        {
            //ゴールのアニメーションを移行
            other.GetComponent<Animator>().SetBool("bGoal", true);

            //プレイヤーのゴール処理
            PlayerController.instance.OnGoal();
        }
    }
}