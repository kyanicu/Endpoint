using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public Weapon Weapon { get; set; }
    public bool isImmortal { get; set; }
    public GameObject RotationPoint { get; set; }
    public ActiveAbility ActiveAbility { get; set; }
    public PassiveAbility PassiveAbility { get; set; }

    protected void Start()
    {
        RotationPoint = transform.Find("RotationPoint").gameObject;
    }

    /// <summary>
    /// Attempts activation of the current active ability
    /// </summary>
    /// <returns>Was the ability activated</returns>
    public bool ActivateActiveAbility()
    {
        HUDController.instance.StartAbilityCooldown(ActiveAbility.Cooldown);
        return ActiveAbility.AttemptActivation();
    }    

    public abstract void TakeDamage(int damage);
    public abstract void Fire();
    public abstract void Reload();
    public abstract void Move(float axis);
    public abstract void AimWeapon(float angle);
    public abstract void Jump();
    public abstract void JumpCancel();
}
