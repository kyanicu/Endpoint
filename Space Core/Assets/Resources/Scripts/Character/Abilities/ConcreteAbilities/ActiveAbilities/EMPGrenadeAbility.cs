using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EMPGrenadeAbility : ActiveAbility
{
    protected override bool activationCondition => activationTimer <= 0f;
    private float force;

    void Start()
    {
        force = 17500f;
        Cooldown = 15f;
    }

    private new void Awake()
    {
        base.Awake();
        AbilityName = "EMP Grenade";
        AbilityShortName = "STUN";
        AbilityDescription = "Push RB to fire a grenade that stuns enemies within a radius.";
        AbilityImage = Resources.Load<Sprite>("Images/UI/HUD/Character Section/Ability Images/ability-stun-grenade@1x");
    }

    protected override void Activate()
    {
        GameObject empGrenadeObject = ObjectPooler.instance.SpawnFromPool("EMP", 
                                                                          owner.Weapon.FireLocation.transform.position, 
                                                                          Quaternion.Euler(owner.Weapon.FireLocation.transform.right));
        empGrenadeObject.GetComponent<Rigidbody2D>().AddForce(owner.Weapon.FireLocation.transform.right * force);
        activationTimer = Cooldown;
    }

}
