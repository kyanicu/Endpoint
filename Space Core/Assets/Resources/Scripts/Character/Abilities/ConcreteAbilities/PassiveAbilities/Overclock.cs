using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overclock : PassiveAbility
{
    // Activate not needed to be called
    protected override bool activationCondition { get { return false; } }

    /// <summary>
    /// The modifier for the overclock ability
    /// </summary>
    [SerializeField]
    private float mod = 1.2f;

    /// <summary>
    /// On start, add the modifier to the character movement
    /// </summary>
    private new void Start()
    {
        base.Start();
        if (!isEnemy)
        {
            GetComponentInParent<Movement>().mod *= mod;
        }
    }

    // Initialize all of the data needed for the Ability UI.
    private new void Awake()
    {
        base.Awake();
        AbilityName = "Overclock";
        AbilityShortName = "OCLCK";
        AbilityDescription = "Your chassis has a passive 10% increase to movement speed.";
        AbilityImage = Resources.Load<Sprite>("Images/UI/HUD/Character Section/Ability Images/ability-overclock@1x");
    }

    /// <summary>
    /// frame by frame activation is not needed
    /// </summary>
    protected override void Activate()
    {
        
    }

}
