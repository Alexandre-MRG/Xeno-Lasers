using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_PowerUP : PowerUP {

    [SerializeField] int levelOfPowerAmount = 1;


    protected override void enablePowerUpPayload(ref PlayerController player)
    {
        base.enablePowerUpPayload(ref player);

        player.boostPowerPlayer(levelOfPowerAmount);
        player.RestartPowerDecayDelayPlayer();
    }
}
