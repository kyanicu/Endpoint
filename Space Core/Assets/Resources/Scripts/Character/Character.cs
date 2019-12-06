using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public enum AnimationState { idle, running }

    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public Weapon Weapon { get; set; }
    public bool isImmortal { get; set; }
    public GameObject RotationPoint { get; set; }
    public ActiveAbility ActiveAbility { get; set; }
    public PassiveAbility PassiveAbility { get; set; }
    public Movement movement { get; protected set; }
    public GameObject MinimapIcon;

    public bool isStunned { get; set; }
    public int facingDirection { get { return (int)Mathf.Sign(transform.localScale.x); } }

    public AnimationState animationState { get; set; }
    public Animator animator;

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
        return ActiveAbility.AttemptActivation();
    }

    public virtual void Move(float axis)
    {
        if (isStunned)
        {
            axis = 0;
        }

        if (axis != 0 && animationState != AnimationState.running)
        {
            animationState = AnimationState.running;
            animator.SetInteger("AnimationState", (int) animationState);
        }
        else if (axis == 0 && animationState == AnimationState.running)
        {
            animationState = AnimationState.idle;
            animator.SetInteger("AnimationState", (int)animationState);
        }

        movement.Run(axis);
    }

    public virtual void Jump()
    {
        if (!isStunned)
            movement.Jump();
    }

    public virtual void JumpCancel()
    {
        if (!isStunned)
            movement.JumpCancel();
    }


    public virtual void ReceiveAttack(AttackInfo attackInfo)
    {
        if (isImmortal)
            return;

        TakeDamage(attackInfo.damage);
        StartCoroutine(Stun(attackInfo.stunTime));
        movement.TakeKnockback(attackInfo.knockbackImpulse, attackInfo.knockbackTime);
    }

    public IEnumerator Stun(float time)
    {
        isStunned = true;
        yield return new WaitForSeconds(time);
        isStunned = false;
    }

    protected virtual void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    protected abstract void Die();

    public abstract bool Fire();
    public abstract void Reload();
    public abstract void AimWeapon(float angle);

    public abstract void DeselectHackTarget();

}
