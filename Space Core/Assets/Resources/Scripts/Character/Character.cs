using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public Weapon Weapon { get; set; }
    public GameObject RotationPoint { get; set; }
    /// <summary>
    /// The Prefab or current child object reference to the Character's active Ability
    /// </summary>
    [SerializeField] private ActiveAbility activeAbility;
    /// <summary>
    /// The Prefab or current child object reference to the Character's passive Ability
    /// </summary>
    [SerializeField] private PassiveAbility passiveAbility;

    protected void Start()
    {
        RotationPoint = transform.Find("RotationPoint").gameObject;
        if(activeAbility) activeAbility = Instantiate(activeAbility, transform);
        if (passiveAbility) passiveAbility = Instantiate(passiveAbility, transform);
    }

    /// <summary>
    /// Attempts activation of the current active ability
    /// </summary>
    /// <returns>Was the ability activated</returns>
    public bool ActivateActiveAbility()
    {
        return activeAbility.AttemptActivation();
    }

    /// <summary>
    /// Set the passive ability
    /// </summary>
    /// <param name="ability">the ability object or prefab to be copied and instatiated, without effecting the original</param>
    public void SetPassiveAbility(PassiveAbility ability)
    {
        passiveAbility = Instantiate(ability, transform);
    }

    /// <summary>
    /// Set the passive ability
    /// </summary>
    /// <param name="ability">the ability object or prefab to be copied and instatiated, without effecting the original</param>
    public void SetActiveAbility(ActiveAbility ability)
    {
        activeAbility = Instantiate(ability, transform);
    }
    /// <summary>
    /// returns a reference to the active ability
    /// </summary>
    /// <returns>reference to the active ability</returns>
    public ActiveAbility GetActiveAbility()
    {
        return activeAbility;
    }
    /// <summary>
    /// returns a reference to the passive ability
    /// </summary>
    /// <returns>reference to the passive ability</returns>
    public PassiveAbility GetPassiveAbility()
    {
        return passiveAbility;
    }
    

    public abstract void TakeDamage(int damage);
    public abstract void Fire();
    public abstract void Reload();
    public abstract void Move(float axis);
    public abstract void AimWeapon(float angle);
    public abstract void Jump();
    public abstract void JumpCancel();
}
