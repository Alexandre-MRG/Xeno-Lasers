using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_PowerUP : PowerUP
{

    [SerializeField] int ammoAmount = 1;

    protected override void enablePowerUpPayload(ref PlayerController player)
    {
        base.enablePowerUpPayload(ref player);

        player.GetComponentInChildren<MissileLauncher>().manageAmmo(ammoAmount);
    }
}
