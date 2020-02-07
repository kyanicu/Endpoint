public class IYBKYD : WheelUpgrade
{
    protected override bool activationCondition => throw new System.NotImplementedException();
    public static float DamageOnCancel = 15;

    public override void DeactivateAbility()
    {
        PlayerController.instance.IYBKYDActive = false;
    }

    public override void EnableAbility()
    {
        PlayerController.instance.IYBKYDActive = false;
    }
}
