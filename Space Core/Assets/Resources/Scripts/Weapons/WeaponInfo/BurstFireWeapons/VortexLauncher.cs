public class VortexLauncherInfo : WeaponGenerationInfo
{
    public VortexLauncherInfo()
    {
        name = "Vortex Launcher";
        description = "Fire drill projectiles to do more damage over time to enemies!";
        BulletTag = "DrillShot";

        MinSpread = 0f;
        MaxSpread = 3f;

        MinDamage = 5;
        MaxDamage = 10;

        StunTime = 0.3f;
        
        MinKnockbackImpulse = 0;
        MaxKnockbackImpulse = 0;

        MinKnockbackTime = 0.0f;
        MaxKnockbackTime = 0.0f;

        MinClipSize = 10;
        MaxClipSize = 20;

        MinRateOfFire = 0.6f;
        MaxRateOfFire = 0.8f;

        MinReloadTime = 0.5f;
        MaxReloadTime = 3.0f;

        MinRange = 10f;
        MaxRange = 18f;

        MaxBulletVeloc = 14f;
        MinBulletVeloc = 8f;
    }
}
