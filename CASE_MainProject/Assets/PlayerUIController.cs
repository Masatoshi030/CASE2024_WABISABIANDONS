using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{

    [SerializeField, Header("èˆãCÉQÅ[ÉW")]
    Image steamGauge;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        steamGauge.fillAmount = PlayerController.instance.heldSteam / PlayerController.instance.maxHeldSteam;
    }
}
