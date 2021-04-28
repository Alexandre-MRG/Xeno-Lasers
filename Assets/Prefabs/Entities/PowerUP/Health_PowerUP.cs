﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_PowerUP : PowerUP {

    [SerializeField] int healthBonus = 50;


    protected override void enablePowerUpPayload(ref PlayerController player)
    {
        base.enablePowerUpPayload(ref player);
        player.healPlayer(healthBonus);
    }



}
