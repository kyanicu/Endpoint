using System;

public class Tactical : WheelUpgrade
{
    protected override bool activationCondition => true;
    private static float reloadMod = 0.8f;
    private static float magIncreaseMod = 1.5f;

    public override void DeactivateAbility()
    {
        PlayerController.instance.Character.Weapon.ReloadTime /= reloadMod;
        PlayerController.instance.Character.Weapon.ClipSize = (int)Math.Round(PlayerController.instance.Character.Weapon.ClipSize / magIncreaseMod);
        PlayerController.instance.TacticalActive = false;
        HUDController.instance.UpdateAmmo(PlayerController.instance.Character);
        enabled = false;
    }

    public override void EnableAbility()
    {
        PlayerController.instance.Character.Weapon.ReloadTime *= reloadMod;
        PlayerController.instance.Character.Weapon.ClipSize = (int)Math.Round(PlayerController.instance.Character.Weapon.ClipSize * magIncreaseMod);
        HUDController.instance.UpdateAmmo(PlayerController.instance.Character);
        PlayerController.instance.TacticalActive = true;
    }

    public static void ResetAbility()
    {
        PlayerController.instance.Character.Weapon.ReloadTime /= reloadMod;
        PlayerController.instance.Character.Weapon.ClipSize = (int) Math.Round(PlayerController.instance.Character.Weapon.ClipSize / magIncreaseMod);
        HUDController.instance.UpdateAmmo(PlayerController.instance.Character);
    }

    public static void ApplyAbility()
    {
        PlayerController.instance.Character.Weapon.ReloadTime *= reloadMod;
        PlayerController.instance.Character.Weapon.ClipSize = (int)Math.Round(PlayerController.instance.Character.Weapon.ClipSize * magIncreaseMod);
        HUDController.instance.UpdateAmmo(PlayerController.instance.Character);
    }
}
