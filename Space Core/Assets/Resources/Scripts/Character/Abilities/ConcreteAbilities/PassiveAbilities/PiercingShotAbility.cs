using UnityEngine;

/// <summary>
/// Passive ability that enables piercing shots on the owner's weapon
/// </summary>
public class PiercingShotAbility : PassiveAbility
{
    //Bool to indicate when the activation condition should be called
    private bool NeedsActivation;
    protected override bool activationCondition => NeedsActivation;

    /// <summary>
    /// Activate function that will set the owner's weapon's bullet to be a piercing shot.
    /// </summary>
    protected override void Activate()
    {
        owner.Weapon.Bullet = Resources.Load<GameObject>("Prefabs/Weapons/PiercingBullet");
        NeedsActivation = false;
    }

    /// <summary>
    /// Needs activation set to true sp activate will be called after start
    /// </summary>
    protected new void Start()
    {
        base.Start();
        NeedsActivation = true;
    }

    // Initialize all of the data needed for the Ability UI.
    private new void Awake()
    {
        base.Awake();
        AbilityName = "Piercing Shot";
        AbilityShortName = "PSHOT";
        AbilityDescription = "Your bullets can pass through an enemy to damage another.";
        AbilityImage = Resources.Load<Sprite>("Images/UI/HUD/Character Section/Ability Images/ability-pierce-shot@1x");
    }
}
