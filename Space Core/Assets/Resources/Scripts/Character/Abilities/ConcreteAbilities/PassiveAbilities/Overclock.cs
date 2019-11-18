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

    /// <summary>
    /// frame by frame activation is not needed
    /// </summary>
    protected override void Activate()
    {
        
    }

}
