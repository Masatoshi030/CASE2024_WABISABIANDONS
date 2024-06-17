using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class HeadCollision_Manager : MonoBehaviour
{
    [SerializeField, Header("オーディオソース参照先")]
    AudioSource audioSource;

    [SerializeField, Header("音声クリップリスト")]
    AudioClip[] soundClips;

    [SerializeField, Header("火花エフェクト")]
    GameObject hibana_ParticleEffect;

    [SerializeField, Header("パーツ散開エフェクト")]
    GameObject partsSplit_ParticleEffect;

    [SerializeField, Header("可燃ガス着火判定")]
    GameObject explosionSwitchObject;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall" || other.tag == "Ground" || other.tag == "Enemy" || other.tag == "Dummy" || other.tag == "Valve" || other.tag == "Goal" || other.tag == "BrokenWall")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
            {
                //敵に突撃した時の処理
                if (other.tag == "Enemy")
                {
                    Debug.Log("head");
                    Enemy enemy = other.GetComponent<Enemy>();

                    // 死亡フラグの判定
                    bool isDeath = enemy.Damage(20.0f, transform.up);

                    // エネミーのトリガーを呼び出し
                    enemy.Machine.TriggerEnterOpponent(other);

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

                    //突撃可能フラグを有効にする
                    PlayerController.instance.bAttackPossible = true;

                    //倒したら
                    if (isDeath)
                    {
                        //パーツ散開エフェクトを生成する
                        Instantiate(partsSplit_ParticleEffect, transform.position, Quaternion.identity);

                        //ヒットストップ
                        HitStopManager.instance.HitStopEffect(0.5f, 0.25f);

                        //小振動
                        DualSense_Manager.instance.SetRumble_Type1();
                    }
                    //倒してない
                    else
                    {
                        //ヒットストップ
                        HitStopManager.instance.HitStopEffect(0.2f, 0.25f);

                        //小振動
                        DualSense_Manager.instance.SetLeftRumble(0.75f, 0.1f);
                    }
                }
                // ギミック衝突時の処理
                if (other.tag == "Valve")
                {
                    other.GetComponent<Valve_Base>().SetCommand();
                }

                if (other.tag == "BrokenWall")
                {
                    if (other.transform.parent.GetComponent<BrokenWallController>().bBroken == false)
                    {
                        if (PlayerController.instance.heldSteam / PlayerController.instance.maxHeldSteam > 0.9f)
                        {
                            other.transform.parent.GetComponent<BrokenWallController>().SetBreak();

                            //ヒットストップ
                            HitStopManager.instance.HitStopEffect(0.05f, 0.5f);

                            //小振動
                            DualSense_Manager.instance.SetLeftRumble(1.0f, 0.1f);

                            //衝突音
                            audioSource.PlayOneShot(soundClips[1]);

                            //火花を生成する
                            Instantiate(hibana_ParticleEffect, transform.position, Quaternion.identity);

                            return;
                        }
                    }
                }

                if (other.tag == "Goal")
                {
                    //ゴールのアニメーションを移行
                    other.GetComponent<Animator>().SetBool("bGoal", true);

                    //プレイヤーのゴール処理
                    PlayerController.instance.OnGoal();
                }

                //着火判定生成
                if (other.tag == "Wall" || other.tag == "Ground")
                {
                    Instantiate(explosionSwitchObject, transform.position, Quaternion.identity);
                }

                //突撃終了
                PlayerController.instance.StopAttack();

                //衝突音
                audioSource.PlayOneShot(soundClips[1]);

                //火花を生成する
                Instantiate(hibana_ParticleEffect, transform.position, Quaternion.identity);

                //小振動
                DualSense_Manager.instance.SetLeftRumble(0.25f, 0.1f);
            }
        }
    }
}