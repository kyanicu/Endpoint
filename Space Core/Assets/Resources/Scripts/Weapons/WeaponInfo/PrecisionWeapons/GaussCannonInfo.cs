using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussCannonInfo : WeaponGenerationInfo
{
    public GaussCannonInfo()
    {
        name = "Gauss Cannon";
        description = "Typical ranged weapon with no special features.";

        BulletTag = "NormalBullet";

        MinSpread = 0.0f;
        MaxSpread = 0.0f;

        MinDamage = 30;
        MaxDamage = 40;

        StunTime = 0.35f;

        MinKnockbackImpulse = 10;
        MaxKnockbackImpulse = 10;

        MinKnockbackTime = 0.1f;
        MaxKnockbackTime = 0.1f;

        MinClipSize = 5;
        MaxClipSize = 15;

        MinRateOfFire = 0.25f;
        MaxRateOfFire = 1.0f;

        MinReloadTime = 0.5f;
        MaxReloadTime = 3.0f;

<<<<<<< HEAD:Space Core/Assets/Resources/Scripts/Weapons/WeaponInfo/PrecisionWeapons/GaussCannonInfo.cs
        MinRange = 15f;
        MaxRange = 25f;
=======
        MinRange = 25f;
        MaxRange = 60f;
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2:Space Core/Assets/Resources/Scripts/Weapons/WeaponInfo/PrecisionWeapons/SnipeyBoi.cs

        MaxBulletVeloc = 40f;
        MinBulletVeloc = 30f;
    }
}
