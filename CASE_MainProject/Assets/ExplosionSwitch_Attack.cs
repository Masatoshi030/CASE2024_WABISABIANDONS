using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ExplosionSwitch_Attack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Explosion")
        {
            other.GetComponent<ExplosionElementController>().SetExplosion();
        }
    }
}
