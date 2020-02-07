using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateOfFireOptimizer : WheelUpgrade
{
    protected override bool activationCondition => true;

    public static float OriginalTimeBetweenShots;
    public static float TimeDecreaseMod = .05f;
    public static float IncreasedFireRateMod = 1.5f;

    public override void DeactivateAbility()
    {
        PlayerController.instance.RateOfFireOptimizerActive = false;
        PlayerController.instance.Character.Weapon.RateOfFire = OriginalTimeBetweenShots;
    }

    public override void EnableAbility()
    {
        PlayerController.instance.RateOfFireOptimizerActive = true;
        OriginalTimeBetweenShots = PlayerController.instance.Character.Weapon.RateOfFire;
    }

    public static void ResetAbility()
    {
        PlayerController.instance.Character.Weapon.RateOfFire = OriginalTimeBetweenShots;
    }

    public static void ApplyAbility()
    {
        OriginalTimeBetweenShots = PlayerController.instance.Character.Weapon.RateOfFire;
    }

    public static void ApplyMod()
    {
        float appliedMod = PlayerController.instance.Character.Weapon.RateOfFire * TimeDecreaseMod;
        PlayerController.instance.Character.Weapon.RateOfFire -= appliedMod;
    }
}
