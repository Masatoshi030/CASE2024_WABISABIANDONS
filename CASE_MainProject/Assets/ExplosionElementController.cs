using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;

public class ExplosionElementController : MonoBehaviour
{

    [SerializeField, Header("爆発エフェクト")]
    GameObject explosionObject;

    [SerializeField, Header("爆発判定までの時間")]
    float explosionOffsetTime = 0.5f;

    [SerializeField, Header("爆発判定までのタイマー")]
    float explosionOffsetTimer = 0.0f;

    [SerializeField, Header("爆発してから判定を無効にするまでの時間")]
    float explosionAfterDisableTime = 0.2f;

    [SerializeField, Header("爆発してから判定を無効にするまでのタイマー")]
    float explosionAfterDisableTimer = 0.0f;

    [SerializeField, Header("爆発検出数")]
    int explosionCount = 0;

    [SerializeField, Header("爆発")]
    bool bExplosion = false;

    [SerializeField, Header("爆発エフェクト生成済み")]
    bool bExplosionInstantiated = false;


    private void Update()
    {
        if (bExplosionInstantiated)
        {
            //タイマーカウント
            explosionOffsetTimer += Time.deltaTime;

            if (explosionOffsetTimer > explosionOffsetTime)
            {
                bExplosion = true;
            }
        }

        if (bExplosion)
        {
            //爆発後判定無効タイマーカウント
            explosionAfterDisableTimer += Time.deltaTime;

            if (explosionAfterDisableTimer > explosionAfterDisableTime)
            {
                this.GetComponent<SphereCollider>().enabled = false;
            }
        }
    }

    public void SetExplosion()
    {
        if (!bExplosionInstantiated)
        {
            //ランダム回転角度で爆発エフェクトを生成
            GameObject effect = Instantiate(explosionObject, transform.position, Quaternion.identity);

            //子どもにする
            effect.transform.parent = this.transform;

            //爆発生成済み
            bExplosionInstantiated = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (bExplosion)
        {
            if (other.tag == "Explosion")
            {
                other.GetComponent<ExplosionElementController>().SetExplosion();
            }
        }
    }
}
