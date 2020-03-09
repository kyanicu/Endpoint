using System.Collections;
using UnityEngine;

/// <summary>
/// Light Enemy controller implementation.
/// </summary>
public class LightEnemyAIController : AIController
{
    private const float TIME_BETWEEN_JUMP_CHECKS = 3f;

    private float attackMoveRange = 0.3f;
    private bool jumpActive;
    private float jumpCheckTimer = TIME_BETWEEN_JUMP_CHECKS;

    protected override void Attack()
    {
        jumpCheckTimer -= Time.deltaTime;
        AttackMove(attackMoveRange, ATTACK_SPEED_MOD);

        if(BehindPlayer() || jumpCheckTimer > 0)
        {
            FireAtPlayer();
        }
        else if (!jumpActive)
        {
            jumpActive = true;
            StartCoroutine(JumpBehindPlayer());
        }
    }

    /// <summary>
    /// Method used to implement behavior for having the light enemy
    /// jump behind the player
    /// </summary>
    private IEnumerator JumpBehindPlayer()
    {
        while(!BehindPlayer())
        {
            Move(Vector2.zero);
            Jump();
            yield return new WaitForSeconds(.2f);
            short directionTowardsPlayer = GetMovementDirection(playerPos.x);
            if (directionTowardsPlayer > 0)
            {
                AimWeapon(0);
            }
            else
            {
                AimWeapon(180);
            }
            Character.movement.SpecialAbility();
        }
        jumpActive = false;
        jumpCheckTimer = TIME_BETWEEN_JUMP_CHECKS;
    }

    /// <summary>
    /// Checks to see if the light enemy is behind the player
    /// </summary>
    /// <returns>whether or not the enemy is behind the player</returns>
    private bool BehindPlayer()
    {
        if (PlayerController.instance.Character.facingDirection == Character.facingDirection)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
