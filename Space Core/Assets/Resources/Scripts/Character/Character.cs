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
    private List<Ability> abilities = new List<Ability>(2);

    protected void Start()
    {
        RotationPoint = transform.Find("RotationPoint").gameObject;
    }

    public bool ActivateAbility(int i)
    {
        if (i >= abilities.Count || abilities[i].type == Ability.AbilityType.PASSIVE)
        {
            return false;
        }
        else
        {
            abilities[i].Activate();
            return true;
        }
    }

    public abstract void TakeDamage(int damage);
    public abstract void Fire();
    public abstract void Reload();
    public abstract void Move(float axis);
    public abstract void AimWeapon(float angle);
    public abstract void Jump();
    public abstract void JumpCancel();
}
