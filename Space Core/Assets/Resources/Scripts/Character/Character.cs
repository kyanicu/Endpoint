using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public Weapon Weapon { get; set; }
    public GameObject RotationPoint { get; set; }
    [SerializeField]
    private ActiveAbility activeAbility;
    [SerializeField]
    private PassiveAbility passiveAbility;

    protected void Start()
    {
        RotationPoint = transform.Find("RotationPoint").gameObject;
    }

    public bool ActivateActiveAbility()
    {
        return activeAbility.AttemptActivation();
    }

    public abstract void TakeDamage(int damage);
    public abstract void Fire();
    public abstract void Reload();
    public abstract void Move(float axis);
    public abstract void AimWeapon(float angle);
    public abstract void Jump();
    public abstract void JumpCancel();
}
