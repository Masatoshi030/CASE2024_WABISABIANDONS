using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField, Header("消滅タイマー")]
    float destroyTimer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        destroyTimer -= Time.deltaTime;

        if(destroyTimer < 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
