using System;
using UnityEngine;

/// <summary>
/// Medium Enemy controller implementation.
/// </summary>
public class MediumEnemyAIController : AIController
{
    //counter that keeps track of the amount of damage the Medium Enemy has taken
    private float damageTaken = 0;
    private float attackMoveRange = 0.5f;

    protected override void Update()
    {
        CheckRolling();
        base.Update();
    }

    /// <summary>
    /// Method that controls the Medium Enemy's attack behavior
    /// </summary>
    protected override void Attack()
    {
        
        AttackMove(attackMoveRange, ATTACK_SPEED_MOD);

        //When the medium enemy takes 30 damage, roll away from the player
        if (damageTaken >= 30 && Character.isStunned <= 0)
        {
            short directionTowardsObject = GetMovementDirection(playerPos.x);
            Move(directionTowardsObject * -1 * Vector2.right);
            Character.movement.SpecialAbility();
            damageTaken = 0;
        }
        else
        {
            FireAtPlayer();
        }
    }

    /// <summary>
    /// override for recieve attack so that the medium enemy can keep track
    /// of the amount of damage it has taken.
    /// </summary>
    /// <param name="attackInfo">Information of the attack the enemy is receiving</param>
    public override void ReceiveAttack(AttackInfo attackInfo)
    {
        if (Character.Invincible > 0)
            return;

        base.ReceiveAttack(attackInfo);
        damageTaken += attackInfo.damage;
    }
}
