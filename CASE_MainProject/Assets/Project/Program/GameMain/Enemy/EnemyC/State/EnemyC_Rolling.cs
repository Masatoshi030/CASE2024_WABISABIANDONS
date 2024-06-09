using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyC_Rolling : EnemyState_C
{
    Rigidbody rb;

    [SerializeField, Header("速度"), ReadOnly]
    float rollSpeed;
    [SerializeField, Header("方向"), ReadOnly]
    Vector3 direction;
    [SerializeField, Header("回転するボディ")]
    GameObject rollingBody;
    [SerializeField, Header("回転軸"), ReadOnly, VectorRange(-1.0f, 1.0f)]
    Vector3 rollAxis;
    float angle;

    [SerializeField, Header("巻き込みオブジェクト")]
    List<GameObject> implicateObjects;
    Dictionary<GameObject, Vector3> positions = new Dictionary<GameObject, Vector3>();
    Dictionary<GameObject, float> angles = new Dictionary<GameObject, float>();

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("衝突時の遷移")]
    int collisionID;

    public override void Initialize()
    {
        base.Initialize();

        rb = enemy.transform.GetComponent<Rigidbody>();
        rollingBody = enemy.transform.Find("Body").gameObject;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        rollSpeed = enemyC.DamageValue;
        direction = enemyC.DamageVector;
        direction.y = 0.0f;
        direction.Normalize();
        enemy.transform.LookAt(enemy.transform.position + direction);

        Vector3 u = (Mathf.Abs(direction.z) < 0.0001f) ? Vector3.forward : Vector3.up;
        rollAxis = Vector3.Cross(direction, u);
        rollAxis.Normalize();
        enemy.IsVelocityZero = false;
    }

    public override void MainFunc()
    {
        base.MainFunc();

        angle -= rollSpeed;
        angle %= -360.0f;
        enemyC.gameObject.transform.Translate(direction * rollSpeed * Time.deltaTime);
        rollingBody.transform.Rotate(rollingBody.transform.forward * -1080 * Time.deltaTime);

        for(int i = 0; i <  implicateObjects.Count; i++)
        {　　
            implicateObjects[i].transform.position = enemyC.transform.position + positions[implicateObjects[i]];
            implicateObjects[i].transform.RotateAround(enemyC.transform.position, rollAxis, angle + angles[implicateObjects[i]]);
        }
    }

    public override void CollisionEnterSelf(Collision collision)
    {
        if(collision.transform.tag == "Ground" || collision.transform.tag == "Wall")
        {
            // 反射ベクトルの計算
            Vector3 point = collision.contacts[0].point;
            Vector3 localdirection = point - enemy.transform.position;
            enemy.EnemyRigidbody.velocity = Vector3.zero;

            Ray ray = new Ray(enemy.transform.position, localdirection);
            Vector3 normal = Vector3.zero;
            RaycastHit[] hits = Physics.RaycastAll(ray, 10.0f);
            for (int i = 0; i < hits.Length; i++)
            {
                if(hits[i].transform.gameObject == collision.gameObject)
                {
                    normal = hits[i].normal;
                }
            }
            
            normal = collision.contacts[0].normal;
            

            Vector3 reflect = direction - 2 * normal * Vector3.Dot(direction, normal);
            reflect.y = 0.0f;
            reflect.Normalize();
            
            // 圧力の状態で変化
            switch (enemyC.CState)
            {
                case EnemyC.PressureState.Empty: enemyC.CState = EnemyC.PressureState.Medium; break;
                case EnemyC.PressureState.Medium: enemyC.CState = EnemyC.PressureState.Full; break;
                case EnemyC.PressureState.Full:
                    // 最大の場合
                    foreach(GameObject obj in implicateObjects)
                    {
                        if (obj.GetComponent<Enemy>() != null)
                        {
                            Vector3 velocity = obj.transform.position - enemy.transform.position;
                            velocity.Normalize();
                            velocity.y = 5.0f;
                            Enemy enem = obj.GetComponent<Enemy>();
                            enem.GetComponent<NavMeshAgent>().enabled = false;
                            enem.EnemyRigidbody.AddForce(velocity * rollSpeed * 2.0f, ForceMode.Impulse);
                            if(enem.Machine.StateObject.GetComponent<EnemyC_IntervalDeath>())
                            {
                                EnemyC_IntervalDeath deathState = enem.Machine.StateObject.GetComponent<EnemyC_IntervalDeath>();
                                enem.Machine.TransitionTo(deathState.StateID);
                            }
                        }
                    }
                    positions.Clear();
                    angles.Clear();
                    implicateObjects.Clear();
                    machine.TransitionTo(collisionID);
                    break;
            }
            // 移動方向を反射ベクトルに変換
            direction = reflect;
            Vector3 u = (Mathf.Abs(direction.z) < 0.0001f) ? Vector3.forward : Vector3.up;
            rollAxis = Vector3.Cross(direction, u);
            rollAxis.Normalize();
            angle = 0.0f;
        }

        // 敵にぶつかった場合巻き込む
        if(collision.transform.tag == "Enemy")
        {
            enemy.EnemyRigidbody.velocity = Vector3.zero;
            // オブジェクトが未登録
            if (!implicateObjects.Contains(collision.gameObject))
            {
                Enemy enem = collision.gameObject.GetComponent<Enemy>();
                // ステートの取得が成功
                if(enem.Machine.StateObject.GetComponent<EnemyC_Caught>())
                {
                    EnemyC_Caught caughtState = enem.Machine.StateObject.GetComponent<EnemyC_Caught>();
                    caughtState.Caught = true;
                    enem.Machine.TransitionTo(caughtState.StateID);
                    enem.EnemyCollider.enabled = false;
                    // 衝突オブジェクトリストに追加
                    implicateObjects.Add(collision.gameObject);
                    // 衝突時のベクトルの差分を求める
                    Vector3 sub = collision.transform.position - enemyC.transform.position;
                    // 差分を辞書に登録
                    positions.Add(collision.gameObject, sub);
                    // 衝突時の回転角度を保存しておく
                    angles.Add(collision.gameObject, angle);
                }
            }
            else
            {
                Debug.Log("登録済み");
            }
        }
    }

    public override void TriggerEnterSelf(Collider other)
    {
        // 敵にぶつかった場合巻き込む
        if (other.transform.tag == "Enemy")
        {
            // オブジェクトが未登録
            if (!implicateObjects.Contains(other.gameObject))
            {
                Enemy enem = other.gameObject.GetComponent<Enemy>();
                // ステートの取得が成功
                if (enem.Machine.StateObject.GetComponent<EnemyC_Caught>())
                {
                    EnemyC_Caught caughtState = enem.Machine.StateObject.GetComponent<EnemyC_Caught>();
                    caughtState.Caught = true;
                    enem.Machine.TransitionTo(caughtState.StateID);
                    enem.EnemyCollider.enabled = false;
                    // 衝突オブジェクトリストに追加
                    implicateObjects.Add(other.gameObject);
                    // 衝突時のベクトルの差分を求める
                    Vector3 sub = other.transform.position - enemyC.transform.position;
                    // 差分を辞書に登録
                    positions.Add(other.gameObject, sub);
                    // 衝突時の回転角度を保存しておく
                    angles.Add(other.gameObject, angle);
                }
            }
            else
            {
                Debug.Log("登録済み");
            }
        }
    }
}
