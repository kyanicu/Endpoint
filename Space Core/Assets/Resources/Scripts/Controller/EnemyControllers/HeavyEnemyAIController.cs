using UnityEngine;

/// <summary>
/// Heavy Enemy controller implementation.
/// </summary>
public class HeavyEnemyAIController : AIController
{
    private const float MAX_HOVER_TIME = 1.9f;
    private bool hovering;
    private float hoverTimer = MAX_HOVER_TIME;
    private float damageTaken = 0;
    private float attackMoveRange = 0.4f;

    /// <summary>
    /// Method for controling the heavy's 
    /// </summary>
    protected override void Attack()
    {
        AttackMove(attackMoveRange, ATTACK_SPEED_MOD);

        //When the heavy enemy takes 50 damage, start hovering
        if (damageTaken >= 50 && !hovering && Character.isStunned <= 0)
        {
            hovering = true;
            short directionTowardsObject = GetMovementDirection(playerPos.x);
            Move(directionTowardsObject * Vector2.right);
            Jump();
            hoverTimer = MAX_HOVER_TIME;
            damageTaken = 0;
        }
        else
        {
            FireAtPlayer();
        }
        
        if ((Character.movement as HeavyMovement).isHovering)
        {
            hoverTimer -= Time.deltaTime;
        }

        if (hoverTimer <= 0)
        {
            hovering = false;
            JumpCancel();
        }
    }

    /// <summary>
    /// override for recieve attack so that the heavy enemy can keep track
    /// of the amount of damage it has taken.
    /// </summary>
    /// <param name="attackInfo">Information of the attack the enemy is receiving</param>
    public override void ReceiveAttack(AttackInfo attackInfo)
    {
        if (Character.Invincible > 0)
            return;

        base.ReceiveAttack(attackInfo);

        if (!hovering)
        {
            damageTaken += attackInfo.damage;
        }
    }
}
