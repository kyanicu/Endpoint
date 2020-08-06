<<<<<<< HEAD
﻿using System;
using UnityEngine;
=======
﻿using UnityEngine;
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2

/// <summary>
/// A controller is responsible for guiding the behavior of a character in the scene
/// </summary>
public abstract class Controller : MonoBehaviour
{
    //Character this controller is in control of
<<<<<<< HEAD
    public Character Character;
=======
    public Character Character { get; set; }
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2

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

<<<<<<< HEAD
    protected virtual void Update()
    {
        CheckFalling();
    }

=======
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
    /// <summary>
    /// Method for swapping the controller's character with another controller.
    /// </summary>
    /// <param name="newCharacter">New Character this controller will gain control of</param>
    /// <param name="controller">The controller that is getting this controller's old character</param>
<<<<<<< HEAD
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
=======
    public void SwapCharacter(Character newCharacter, Controller controller)
    {
        Character temp = Character;
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
        temp.AttackRecievedDeleage = controller.ReceiveAttack;
        temp.TriggerEntered2DDelegate = controller.TriggerEnter2D;
        Character = newCharacter;
        Character.AttackRecievedDeleage = ReceiveAttack;
        Character.TriggerEntered2DDelegate = TriggerEnter2D;
        controller.Character = temp;
<<<<<<< HEAD
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
=======
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
    }

    /// <summary>
    /// Move method that exposes the character's move method
    /// </summary>
<<<<<<< HEAD
    /// <param name="direction">direcion the player is moving in</param>
    public void Move(Vector2 direction)
    {
        Character.Move(direction.normalized);
=======
    /// <param name="axis">Axis the player is moving along</param>
    public void Move(float axis)
    {
        Character.Move(axis);
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
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
<<<<<<< HEAD
        Character.AudioSource.clip = Character.HitClip;
        Character.AudioSource.Play();

        // Update worldspace UI for enemies if this is an enemy type controller.
        if (this is HeavyEnemyAIController || this is LightEnemyAIController || this is MediumEnemyAIController)
        {
            Character.WorldspaceCanvas.GetComponent<WorldspaceCanvasManager>().UpdateAsEnemyCanvas(Character);
        }

=======
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
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
<<<<<<< HEAD
        if (Character.Invincible < 0)
        {
            Character.Invincible = 0;
        }

        if (Character.Invincible > 0)
=======
        if (Character.Invincible != 0)
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
            return;

        TakeDamage(attackInfo.damage);
        StartCoroutine(Character.Stun(attackInfo.stunTime));
<<<<<<< HEAD
        //Character.movement.TakeKnockback(attackInfo.knockbackImpulse, attackInfo.knockbackTime);
=======
        Character.movement.TakeKnockback(attackInfo.knockbackImpulse, attackInfo.knockbackTime);
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
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
<<<<<<< HEAD
    public virtual bool Fire()
=======
    public bool Fire()
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
    {
        return Character.Fire();
    }

<<<<<<< HEAD
    public virtual bool EndFire()
    {
        return Character.EndFire();
    }

=======
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
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
<<<<<<< HEAD

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
=======
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
}
