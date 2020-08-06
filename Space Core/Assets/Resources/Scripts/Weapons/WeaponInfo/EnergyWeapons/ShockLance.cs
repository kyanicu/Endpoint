using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockLanceInfo : WeaponGenerationInfo
{
    //Stat ranges for Vohnemet Energy Weapon
    public ShockLanceInfo()
    {
        name = "Shock Lance";
        description = "Energy weapon that continually fires energy bolts forward.";
        BulletTag = "EnergyBurst";

        MinSpread = 0.0f;
        MaxSpread = 0.0f;

        MinDamage = 2;
        MaxDamage = 6;

        StunTime = 0.1f;

        MinKnockbackImpulse = 0;
        MaxKnockbackImpulse = 0;

        MinClipSize = 150;
        MaxClipSize = 200;

        MinRateOfFire = 0.005f;
        MaxRateOfFire = 0.01f;

        MinReloadTime = 0.7f;
        MaxReloadTime = 3.5f;

        MinRange = 7f;
        MaxRange = 10f;

        MaxBulletVeloc = 15f;
        MinBulletVeloc = 10f;
    }
}
