using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSwitch : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            this.GetComponent<ExplosionElementController>().SetExplosion();
        }
    }
}
