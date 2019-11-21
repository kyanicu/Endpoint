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
    public Movement movement { get; protected set; }


    protected virtual void Start()
    {
        RotationPoint = transform.Find("RotationPoint").gameObject;
    }

    protected virtual void Reset()
    {
        //Const Values

        if (!(movement = GetComponent<Movement>()))
            movement = gameObject.AddComponent<BasicMovement>();
    }

    protected virtual void Awake()
    {

        if (!(movement = GetComponent<Movement>()))
            movement = gameObject.AddComponent<BasicMovement>();
        else
            movement = GetComponent<Movement>();
        
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

    public virtual void Move(float axis)
    {
        movement.Run(axis);
    }

    public virtual void Jump()
    {
        movement.Jump();
    }

    public virtual void JumpCancel()
    {
        movement.JumpCancel();
    }


    public abstract void Fire();
    public abstract void Reload();
    public abstract void AimWeapon(float angle);
    public abstract void TakeDamage(int damage);

}
