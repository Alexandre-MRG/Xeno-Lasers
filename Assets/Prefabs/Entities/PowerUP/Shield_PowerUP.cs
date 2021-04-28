using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_PowerUP : PowerUP {

    [SerializeField] int shieldBonus = 50;


    protected override void enablePowerUpPayload(ref PlayerController player)
    {
        base.enablePowerUpPayload(ref player);
        player.boostShieldPlayer(shieldBonus);
    }
}
