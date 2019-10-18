using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public Weapon Weapon { get; set; }
    public GameObject RotationPoint { get; set; }

    protected void Start()
    {
        RotationPoint = transform.Find("RotationPoint").gameObject;
    }

    public abstract void TakeDamage(int damage);
    public abstract void Fire();
    public abstract void Reload();
    public abstract void Move(float axis);
    public abstract void AimWeapon();
    public abstract void Jump();
    public abstract void JumpCancel();
}
