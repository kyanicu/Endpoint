using System;
using UnityEngine;

/// <summary>
/// A controller is responsible for guiding the behavior of a character in the scene
/// </summary>
public abstract class Controller : MonoBehaviour
{
    //Character this controller is in control of
    public Character Character { get; set; }

    //Abstract methods all sub-controllers need to implement
    public abstract void DeselectHackTarget();
    public abstract void Die();
    public abstract void TriggerEnter2D(Collider2D collision);

    /// <summary>
    /// Basic start method to set the parent to a ControllerGroup object to maintain organization
    /// </summary>
    public virtual void Start()
    {
        GameObject parent = GameObject.FindWithTag("ControllerGroup");
        gameObject.transform.SetParent(parent.transform);
    }
     
    protected virtual void Update()
    {
        CheckFalling();
    }

    /// <summary>
    /// Method for swapping the controller's character with another controller.
    /// </summary>
    /// <param name="newCharacter">New Character this controller will gain control of</param>
    /// <param name="controller">The controller that is getting this controller's old character</param>
    public void SwapCharacter(Character newCharacter, ref AIController controller)
    {
        GameObject controllerGameObject = controller.gameObject;
        string oldClass = controller.Character.Class;
        Character temp = Character;
        //disable the other controller
        switch (oldClass)
        {
            case "heavy":
                controllerGameObject.GetComponent<HeavyEnemyAIController>().enabled = false;
                break;
            case "medium":
                controllerGameObject.GetComponent<MediumEnemyAIController>().enabled = false;
                break;
            case "light":
                controllerGameObject.GetComponent<LightEnemyAIController>().enabled = false;
                break;
        }
        //enable controller of the new character
        switch (temp.Class)
        {
            case "heavy":
                controller = controllerGameObject.GetComponent<HeavyEnemyAIController>();
                break;
            case "medium":
                controller = controllerGameObject.GetComponent<MediumEnemyAIController>();
                break;
            case "light":
                controller = controllerGameObject.GetComponent<LightEnemyAIController>();
                break;
        }
        temp.AttackRecievedDeleage = controller.ReceiveAttack;
        temp.TriggerEntered2DDelegate = controller.TriggerEnter2D;
        Character = newCharacter;
        Character.AttackRecievedDeleage = ReceiveAttack;
        Character.TriggerEntered2DDelegate = TriggerEnter2D;
        controller.Character = temp;
        //enable the controller object
        switch (temp.Class)
        {
            case "heavy":
                controllerGameObject.GetComponent<HeavyEnemyAIController>().enabled = true;
                break;
            case "medium":
                controllerGameObject.GetComponent<MediumEnemyAIController>().enabled = true;
                break;
            case "light":
                controllerGameObject.GetComponent<LightEnemyAIController>().enabled = true;
                break;
        }
    }

    public void HealCharacter(int health)
    {
        Character.HealCharacter(health);
        HUDController.instance.UpdatePlayer(Character);
    }

    /// <summary>
    /// Move method that exposes the character's move method
    /// </summary>
    /// <param name="direction">direcion the player is moving in</param>
    public void Move(Vector2 direction)
    {
        Character.Move(direction.normalized);
    }

    /// <summary>
    /// Jump method that exposes the character's jump method
    /// </summary>
    public void Jump()
    {
        Character.Jump();
    }

    /// <summary>
    /// Jump cancel method that exposes the character's jump cancel method
    /// </summary>
    public void JumpCancel()
    {
        Character.JumpCancel();
    }

    /// <summary>
    /// Method for applying damage to the controller's character
    /// </summary>
    /// <param name="damage">number of damage the character will take</param>
    public virtual void TakeDamage(float damage)
    {
        Character.Health -= damage;
        Character.AudioSource.clip = Character.HitClip;
        Character.AudioSource.Play();
        if (Character.Health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Method that will be called when the Character takes damage
    /// </summary>
    /// <param name="attackInfo">information about the attack that the character will take</param>
    public virtual void ReceiveAttack(AttackInfo attackInfo)
    {
        if (Character.Invincible < 0)
        {
            Character.Invincible = 0;
        }

        if (Character.Invincible > 0)
            return;

        TakeDamage(attackInfo.damage);
        StartCoroutine(Character.Stun(attackInfo.stunTime));
        //Character.movement.TakeKnockback(attackInfo.knockbackImpulse, attackInfo.knockbackTime);
        if (!Character.IsBlinking)
        {
            Character.IsBlinking = true;
            StartCoroutine(Character.beginFlashing());
        }
    }

    /// <summary>
    /// Method for exposing the character's reload method
    /// </summary>
    public virtual void Reload()
    {
        Character.Reload();
    }

    /// <summary>
    /// Method for exposing the character's fire method
    /// </summary>
    /// <returns></returns>
    public virtual bool Fire()
    {
        return Character.Fire();
    }

    /// <summary>
    /// Method for exposing the character's Aim Weapon method
    /// </summary>
    /// <param name="angle">The new angle to aim the character's weapon at</param>
    public virtual void AimWeapon(float angle)
    {
        Character.AimWeapon(angle, null);
    }

    /// <summary>
    /// Method for exposing the character's Activate Active Ability method
    /// </summary>
    public void ActivateActiveAbility()
    {
        Character.ActivateActiveAbility();
    }

    /// <summary>
    /// Checks if the character is falling
    /// </summary>
    private void CheckFalling()
    {
        if (Character.movement.velocity.y < -0.1)
        {
            Character.animationState = Character.AnimationState.falling;
            Character.animator.SetInteger("AnimationState", (int)Character.animationState);
        }
    }

    /// <summary>
    /// Sets animation state to rolling
    /// </summary>
    protected void CheckRolling()
    {
        MediumMovement mediumMovement = (Character.movement as MediumMovement);
        if (mediumMovement != null && mediumMovement.isCombatRolling)
        {
            Character.animationState = Character.AnimationState.special;
            Character.animator.SetInteger("AnimationState", (int)Character.animationState);
        }
        else if (mediumMovement != null && !mediumMovement.isCombatRolling 
            && Character.animationState == Character.AnimationState.special)
        {
            Character.animationState = Character.AnimationState.running;
            Character.animator.SetInteger("AnimationState", (int)Character.animationState);
        }
    }

    /// <summary>
    /// Checks if the heavy is at max speed
    /// </summary>
    protected void CheckMaxSpeed()
    {
        if (Character.movement.charCont.isGrounded && Character.movement.velocity.x >= Character.movement.runMax)
        {
            Character.animationState = Character.AnimationState.runextended;
            Character.animator.SetInteger("AnimationState", (int)Character.animationState);
        }
        else if (Character.movement.charCont.isGrounded && Character.animationState == Character.AnimationState.runextended)
        {
            Character.animationState = Character.AnimationState.running;
            Character.animator.SetInteger("AnimationState", (int)Character.animationState);
        }
    }
}
