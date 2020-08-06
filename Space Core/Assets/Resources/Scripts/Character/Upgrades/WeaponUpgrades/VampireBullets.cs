public class VampireBullets : WheelUpgrade
{
    protected override bool activationCondition => throw new System.NotImplementedException();

    public override void DeactivateAbility()
    {
        Bullet.VampireBullet = false;
    }

    public override void EnableAbility()
    {
        Bullet.VampireBullet = true;
    }
}
